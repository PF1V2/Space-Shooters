using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EngineGDI
{
    public class Player
    {
        
        public float X { get; private set; }
        public float Y { get; private set; }
        public float Speed { get; private set; }
        public float Width { get; private set; }
        public float Height { get; private set; }

        private string spritePath;

        private Shooter shooter;

        public Player(float startX, float startY, float moveSpeed)
        {
            this.X = startX;
            this.Y = startY;
            this.Speed = moveSpeed;

            
            this.spritePath = "Player.png";

            
            this.Width = 40;
            this.Height = 40;

            this.shooter = new Shooter(0.5f);
        }

        
        public void Update()
        {

            
            if (Engine.IsKeyDown(Keys.Left))
            {
                this.X -= this.Speed * Program.deltaTime;
            }

            
            if (Engine.IsKeyDown(Keys.Right))
            {
                this.X += this.Speed * Program.deltaTime;
            }

            
            float halfWidth = this.Width / 2;

            
            if (this.X - halfWidth < 0)
            {
                this.X = halfWidth; 
            }

            
            if (this.X + halfWidth > Engine.Window.ClientSize.Width)
            {
                
                this.X = Engine.Window.ClientSize.Width - halfWidth;
            }

            shooter.Update();

        }


        public Bullet Shoot()
        {

            if (shooter.CanShoot())
            {
                float bulletSpawnX = this.X;
                float bulletSpawnY = this.Y - (this.Height / 2);
                return new Bullet(bulletSpawnX, bulletSpawnY, 400f);
            }
            
            return null;
        }

        
        public void Draw()
        {
            Engine.Draw(spritePath, this.X, this.Y, 1, 1, 0, 0.5f, 0.5f);
        }
    }
}