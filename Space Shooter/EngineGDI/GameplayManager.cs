using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace EngineGDI
{
    public class GameplayManager
    {
        private Player player;

        //objetos acitvos
        private List<Enemy> enemies;
        private List<Bullet> bullets;

        // POOLS 
        private ObjectPool<Bullet> bulletPool;
        private ObjectPool<Enemy> enemyPool;

        //movimiento enemigos
        private float enemySpeed = 50f;
        private int enemyDirection = 1;
        private float enemyVerticalStep = 25f;

        public GameplayManager()
        {
            //pools
            bulletPool = new ObjectPool<Bullet>();
            enemyPool = new ObjectPool<Enemy>();

            //listas
            enemies = new List<Enemy>();
            bullets = new List<Bullet>();

            //jugador
            player = new Player(350, 700, 200f);

            //Spawn ene
            InitializeEnemies();
        }

        private void InitializeEnemies()
        {
            
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

                    
                    // factory
                    Enemy newEnemy = EnemyFactory.SpawnBasicEnemy(x, y, enemyPool);

                    // list enemigos vivos
                    enemies.Add(newEnemy);
                }
            }
        }

        public void Update()
        {
            
            player.Update();

            
            UpdateEnemiesLogic();

            
            if (Engine.IsKeyPressed(Keys.Space))
            {
                if (player.CanShoot())
                {
                    Engine.PlaySound("Disparo.wav");

                    //pool bala
                    Bullet b = bulletPool.Get();

                    //pos y vel jugador
                    b.Initialize(player.Transform.Position.X, player.Transform.Position.Y - 20, 400f);

                    // 3. La agregamos a la lista de balas activas
                    bullets.Add(b);
                }
            }

            
            
            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                Bullet b = bullets[i];
                b.Update();

                //salePantalla
                if (b.Transform.Position.Y < 0)
                {
                    bulletPool.ReturnToPool(b); // Devolver al pool
                    bullets.RemoveAt(i);        // Sacar de la lista activa
                    continue;
                }

                //Enemigos
                for (int j = enemies.Count - 1; j >= 0; j--)
                {
                    Enemy e = enemies[j];

                    if (CheckCollision(b, e))
                    {
                        // Daño Idamagable
                        e.TakeDamage(1);

                        // La bala golpeó, así que la reciclamos
                        bulletPool.ReturnToPool(b);
                        bullets.RemoveAt(i);

                        
                        if (e.Health.IsDead)
                        {
                            // Sumamos puntos
                            ScoreManager.Instance.AddScore(10);

                            // Reciclamos al enemigo
                            enemyPool.ReturnToPool(e);
                            enemies.RemoveAt(j);
                        }

                        
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

                    // Condición de Derrota (si llega abajo)
                    if (e.Transform.Position.Y > 650)
                    {
                        GameManager.Instance.CurrentState = GameState.Lose;
                    }
                }
            }
        }

        
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
            
            foreach (var b in bullets)
            {
                bulletPool.ReturnToPool(b);
            }
            bullets.Clear();

            
            foreach (var e in enemies)
            {
                enemyPool.ReturnToPool(e);
            }
            enemies.Clear();

            
            player = new Player(350, 700, 200f);

            
            enemyDirection = 1;
            InitializeEnemies();
        }
    }
}