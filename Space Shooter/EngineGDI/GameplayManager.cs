using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace EngineGDI
{
    public class GameplayManager
    {
        private Player player;

        // Listas para saber qué objetos están ACTIVOS en pantalla
        private List<Enemy> enemies;
        private List<Bullet> bullets;

        // POOLS (Requisito 4): Aquí guardamos los objetos inactivos para reciclar
        private ObjectPool<Bullet> bulletPool;
        private ObjectPool<Enemy> enemyPool;

        // Variables para la lógica de movimiento de enemigos (Zig-Zag)
        private float enemySpeed = 50f;
        private int enemyDirection = 1;
        private float enemyVerticalStep = 25f;

        public GameplayManager()
        {
            // 1. Inicializamos los Pools
            bulletPool = new ObjectPool<Bullet>();
            enemyPool = new ObjectPool<Enemy>();

            // 2. Inicializamos las listas
            enemies = new List<Enemy>();
            bullets = new List<Bullet>();

            // 3. Creamos al Jugador
            player = new Player(350, 700, 200f);

            // 4. Spawneamos los enemigos iniciales
            InitializeEnemies();
        }

        private void InitializeEnemies()
        {
            // Aseguramos que la lista esté limpia (los objetos viejos ya debieron volver al pool en Reset)
            enemies.Clear();

            int rows = 5;
            int cols = 8;
            float initialX = 80;
            float initialY = 50;
            float spacing = 50;

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    float x = initialX + c * spacing;
                    float y = initialY + r * spacing;

                    // --- USO DEL FACTORY (Requisito 2) ---
                    // Le pedimos a la fábrica que nos consiga un enemigo (del pool) y lo configure.
                    Enemy newEnemy = EnemyFactory.SpawnBasicEnemy(x, y, enemyPool);

                    // Lo agregamos a la lista de "enemigos vivos"
                    enemies.Add(newEnemy);
                }
            }
        }

        public void Update()
        {
            // Actualizar Jugador
            player.Update();

            // Mover Enemigos (Lógica de Zig-Zag)
            UpdateEnemiesLogic();

            // --- DISPARO DEL JUGADOR ---
            if (Engine.IsKeyPressed(Keys.Space))
            {
                if (player.CanShoot())
                {
                    Engine.PlaySound("Disparo.wav");

                    // 1. Pedimos una bala al POOL
                    Bullet b = bulletPool.Get();

                    // 2. La configuramos (posición y velocidad)
                    //    Usamos Transform para la posición
                    b.Initialize(player.Transform.Position.X, player.Transform.Position.Y - 20, 400f);

                    // 3. La agregamos a la lista de balas activas
                    bullets.Add(b);
                }
            }

            // --- ACTUALIZACIÓN DE BALAS Y COLISIONES ---
            // Recorremos al revés para poder borrar elementos de la lista sin romper el loop
            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                Bullet b = bullets[i];
                b.Update();

                // A. Chequeo si salió de la pantalla
                if (b.Transform.Position.Y < 0)
                {
                    bulletPool.ReturnToPool(b); // ¡IMPORTANTE! Devolver al pool
                    bullets.RemoveAt(i);        // Sacar de la lista activa
                    continue;
                }

                // B. Chequeo de colisiones con enemigos
                for (int j = enemies.Count - 1; j >= 0; j--)
                {
                    Enemy e = enemies[j];

                    if (CheckCollision(b, e))
                    {
                        // Recibimos daño (Interface IDamageable)
                        e.TakeDamage(1);

                        // La bala golpeó, así que la reciclamos
                        bulletPool.ReturnToPool(b);
                        bullets.RemoveAt(i);

                        // Si el enemigo murió (usando el HealthComponent)
                        if (e.Health.IsDead)
                        {
                            // Sumamos puntos
                            ScoreManager.Instance.AddScore(10);

                            // Reciclamos al enemigo
                            enemyPool.ReturnToPool(e);
                            enemies.RemoveAt(j);
                        }

                        // Rompemos el bucle de enemigos porque la bala ya no existe
                        break;
                    }
                }
            }

            // Condición de Victoria
            if (enemies.Count == 0)
            {
                GameManager.Instance.CurrentState = GameState.Win;
            }
        }

        private void UpdateEnemiesLogic()
        {
            bool moveDown = false;
            float moveX = enemySpeed * enemyDirection * Program.deltaTime;

            foreach (var e in enemies)
            {
                e.Update(); // Para animaciones
                e.Move(moveX, 0); // Interface IMovable

                // Detectar bordes de pantalla
                float halfW = e.Renderer.Size.X / 2;

                if (enemyDirection == 1 && e.Transform.Position.X + halfW > Engine.Window.ClientSize.Width)
                {
                    moveDown = true;
                }
                else if (enemyDirection == -1 && e.Transform.Position.X - halfW < 0)
                {
                    moveDown = true;
                }
            }

            // Si tocaron un borde, bajan y cambian de dirección
            if (moveDown)
            {
                enemyDirection *= -1;
                foreach (var e in enemies)
                {
                    e.Move(0, enemyVerticalStep);

                    // Condición de Derrota: Si llegan muy abajo
                    if (e.Transform.Position.Y > 650)
                    {
                        GameManager.Instance.CurrentState = GameState.Lose;
                    }
                }
            }
        }

        // Método simple de colisión AABB (Caja contra Caja) adaptado a los nuevos componentes
        private bool CheckCollision(Bullet b, Enemy e)
        {
            float dx = Math.Abs(b.Transform.Position.X - e.Transform.Position.X);
            float dy = Math.Abs(b.Transform.Position.Y - e.Transform.Position.Y);

            float sumW = (b.Renderer.Size.X / 2) + (e.Renderer.Size.X / 2);
            float sumH = (b.Renderer.Size.Y / 2) + (e.Renderer.Size.Y / 2);

            return dx <= sumW && dy <= sumH;
        }

        public void Draw()
        {
            player.Draw();

            foreach (var e in enemies) e.Draw();
            foreach (var b in bullets) b.Draw();

            Engine.Draw(
                $"Score: {ScoreManager.Instance.Score}",
                10, 10, Brushes.White, new Font("Consolas", 14)
            );
        }

        // Resetea el juego para volver a jugar
        public void Reset()
        {
            // 1. Devolver todas las balas activas al pool
            foreach (var b in bullets)
            {
                bulletPool.ReturnToPool(b);
            }
            bullets.Clear();

            // 2. Devolver todos los enemigos activos al pool
            foreach (var e in enemies)
            {
                enemyPool.ReturnToPool(e);
            }
            enemies.Clear();

            // 3. Resetear jugador
            player = new Player(350, 700, 200f);

            // 4. Resetear enemigos
            enemyDirection = 1;
            InitializeEnemies();
        }
    }
}