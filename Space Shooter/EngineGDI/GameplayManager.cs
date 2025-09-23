using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms; 

namespace EngineGDI
{
    public class GameplayManager
    {
        
        private Player player;
        private List<Enemy> enemies;
        private List<Bullet> bullets;

        
        private float enemySpeed = 50f;
        private int enemyDirection = 1;
        private float enemyVerticalStep = 25f;


        public GameplayManager()
        {
            
            player = new Player(350, 700, 200f);

            
            enemies = new List<Enemy>();
            bullets = new List<Bullet>();

            
            InitializeEnemies();
        }

        private void InitializeEnemies()
        {
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
                    enemies.Add(new Enemy(x, y));
                }
            }
        }


        
        public void Update()
        {
            
            player.Update();

            
            UpdateEnemies();

            foreach (Enemy enemy in enemies)
            {
                enemy.Update();
            }

            if (Engine.IsKeyPressed(Keys.Space))
            {

                Bullet newBullet = player.Shoot(); 
                if (newBullet != null) 
                {
                    bullets.Add(newBullet);
                }
            }


            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                Bullet bullet = bullets[i];
                bullet.Update();

                
                if (bullet.Y < 0)
                {
                    bullets.RemoveAt(i);
                    continue; 
                }

                
                for (int j = enemies.Count - 1; j >= 0; j--)
                {
                    Enemy enemy = enemies[j];

                    if (CheckCollision(bullet, enemy))
                    {
                        
                        bullets.RemoveAt(i);
                        enemy.Health.TakeDamage(1);

                        if (enemy.Health.IsDead)
                        {
                            
                            enemies.RemoveAt(j);
                            ScoreManager.Instance.AddScore(10);
                        }
                        break;
                    }
                }
            }

            if (enemies.Count == 0)
            {
                GameManager.Instance.CurrentState = GameState.Win;
            }
        }

        public void Draw()
        {
            
            player.Draw();

            foreach (Enemy enemy in enemies)
            {
                enemy.Draw();
            }

            foreach (Bullet bullet in bullets)
            {
                bullet.Draw();
            }

            Engine.Draw(
                $"Score: {ScoreManager.Instance.Score}",
                10,
                10,
                Brushes.White,
                new Font("Consolas", 14)
            );
        }

        
        private void UpdateEnemies()
        {
            bool moveDown = false;
            float horizontalMove = enemySpeed * enemyDirection * Program.deltaTime;

            foreach (Enemy enemy in enemies)
            {
                enemy.Move(horizontalMove, 0);
            }

            foreach (Enemy enemy in enemies)
            {
                if (enemyDirection == 1 && enemy.X + (enemy.Width / 2) > Engine.Window.ClientSize.Width)
                {
                    moveDown = true;
                    break;
                }
                else if (enemyDirection == -1 && enemy.X - (enemy.Width / 2) < 0)
                {
                    moveDown = true;
                    break;
                }
            }

            if (moveDown)
            {
                enemyDirection *= -1;
                foreach (Enemy enemy in enemies)
                {
                    enemy.Move(0, enemyVerticalStep);
                    if (enemy.Y > 650) 
                    {
                        GameManager.Instance.CurrentState = GameState.Lose;
                    }
                }
            }
        }

       

        private bool CheckCollision(Bullet bullet, Enemy enemy)
        {

            float distanceX = Math.Abs(bullet.X - enemy.X);
            float distanceY = Math.Abs(bullet.Y - enemy.Y);

            
            float sumHalfWidths = (bullet.Width / 2) + (enemy.Width / 2);
            float sumHalfHeight = (bullet.Height / 2) + (enemy.Height / 2);

            
            return distanceX <= sumHalfWidths && distanceY <= sumHalfHeight;
        }

        public void Reset()
        {
            
            enemies.Clear();
            bullets.Clear();

            
            player = new Player(300, 700, 200f);

            
            InitializeEnemies();
        }
    }
}