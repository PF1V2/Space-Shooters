using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineGDI
{
    public class Enemy
    {
        public float X { get; private set; }
        public float Y { get; private set; }

        private string spritePath = "Enemy1.png";
        public float Width { get; private set; }
        public float Height { get; private set; }

        public HealthComponent Health { get; private set; }

        private Animation animation;

        public Enemy(float startX, float startY)
        {
            this.X = startX;
            this.Y = startY;
            this.Width = 40;
            this.Height = 40;

            this.Health = new HealthComponent(1);

            List<string> enemyFrames = new List<string> { "Enemy1.png", "Enemy1v2.png" };
            animation = new Animation(0.5f, enemyFrames, true); 
        }

        public void Move(float deltaX, float deltaY)
        {
            
            this.X += deltaX;
            this.Y += deltaY;
        }

        public void Update()
        {
            animation.Update();
        }

        public void Draw()
        {
            Engine.Draw(animation.CurrentTexture, this.X, this.Y, 1, 1, 0, 0.5f, 0.5f);
        }
    }
}