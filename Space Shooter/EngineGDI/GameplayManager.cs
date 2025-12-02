using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace EngineGDI
{
    public class GameplayManager
    {
        private Player player;
        private List<Enemy> enemies;
        private List<Bullet> bullets;

        private ObjectPool<Bullet> bulletPool;
        private ObjectPool<Enemy> enemyPool;

        private float enemySpeed = 50f;
        private int enemyDirection = 1;
        private float enemyVerticalStep = 25f;

        
        private int currentLevel = 1;

        public GameplayManager()
        {
            bulletPool = new ObjectPool<Bullet>();
            enemyPool = new ObjectPool<Enemy>();

            enemies = new List<Enemy>();
            bullets = new List<Bullet>();

            player = new Player(350, 700, 200f);

            
            StartLevel(1);
        }

        
        private void StartLevel(int level)
        {
            currentLevel = level;
            enemies.Clear(); 

            
            int rows = 0;
            int cols = 0;
            int enemyHealth = 1;
            List<string> levelFrames = new List<string>();

           
            if (currentLevel == 1)
            {
                
                rows = 5;
                cols = 8;
                enemySpeed = 50f;
                levelFrames = new List<string> { "Enemy1.png", "Enemy1v2.png" };
                enemyHealth = 1;
            }
            else if (currentLevel == 2)
            {
                
                rows = 3;       // Menos filas
                cols = 5;       // Menos columnas
                enemySpeed = 120f; // ¡Más del doble de rápido!
                levelFrames = new List<string> { "Enemy2.png", "Enemy2v2.png" };
                enemyHealth = 2;   // Aguantan 2 disparos
            }

            
            float initialX = 80;
            float initialY = 50;
            float spacing = 50;

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    float x = initialX + c * spacing;
                    float y = initialY + r * spacing;

                    
                    Enemy newEnemy = EnemyFactory.SpawnBasicEnemy(x, y, enemyHealth, levelFrames, enemyPool);
                    enemies.Add(newEnemy);
                }
            }
        }

        public void Update()
        {
            player.Update();
            UpdateEnemiesLogic();

            // Disparo Jugador
            if (Engine.IsKeyPressed(Keys.Space))
            {
                if (player.CanShoot())
                {
                    Engine.PlaySound("Disparo.wav"); 
                    Bullet b = bulletPool.Get();
                    b.Initialize(player.Transform.Position.X, player.Transform.Position.Y - 20, 400f);
                    bullets.Add(b);
                }
            }

            // Actualización de Balas
            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                Bullet b = bullets[i];
                b.Update();

                if (b.Transform.Position.Y < 0)
                {
                    bulletPool.ReturnToPool(b);
                    bullets.RemoveAt(i);
                    continue;
                }

                for (int j = enemies.Count - 1; j >= 0; j--)
                {
                    Enemy e = enemies[j];
                    if (CheckCollision(b, e))
                    {
                        e.TakeDamage(1); // Le hace 1 de daño

                        bulletPool.ReturnToPool(b);
                        bullets.RemoveAt(i);

                        if (e.Health.IsDead)
                        {
                            ScoreManager.Instance.AddScore(10);
                            enemyPool.ReturnToPool(e);
                            enemies.RemoveAt(j);
                        }
                        break;
                    }
                }
            }

            
            if (enemies.Count == 0)
            {
                if (currentLevel == 1)
                {
                    // Si terminamos el nivel 1, pasamos al 2
                    StartLevel(2);
                }
                else
                {
                    // Si terminamos el nivel 2, ganamos el juego
                    GameManager.Instance.CurrentState = GameState.Win;
                }
            }
        }

        private void UpdateEnemiesLogic()
        {
            bool moveDown = false;
            float moveX = enemySpeed * enemyDirection * Program.deltaTime;

            foreach (var e in enemies)
            {
                e.Update();
                e.Move(moveX, 0);

                float halfW = e.Renderer.Size.X / 2;
                if (enemyDirection == 1 && e.Transform.Position.X + halfW > Engine.Window.ClientSize.Width)
                    moveDown = true;
                else if (enemyDirection == -1 && e.Transform.Position.X - halfW < 0)
                    moveDown = true;
            }

            if (moveDown)
            {
                enemyDirection *= -1;
                foreach (var e in enemies)
                {
                    e.Move(0, enemyVerticalStep);
                    if (e.Transform.Position.Y > 650)
                        GameManager.Instance.CurrentState = GameState.Lose;
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

            // Dibujamos Nivel y Score
            Engine.Draw($"Level: {currentLevel} | Score: {ScoreManager.Instance.Score}", 10, 10, Brushes.White, new Font("Consolas", 14));
        }

        public void Reset()
        {
            foreach (var b in bullets) bulletPool.ReturnToPool(b);
            foreach (var e in enemies) enemyPool.ReturnToPool(e);
            bullets.Clear();
            enemies.Clear();
            player = new Player(350, 700, 200f);

            // Volvemos al Nivel 1 al reiniciar
            StartLevel(1);
        }
    }
}