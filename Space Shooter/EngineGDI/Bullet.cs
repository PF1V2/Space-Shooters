
namespace EngineGDI
{
    public class Bullet
    {
        private float x;
        private float y;
        private float speed;
        private float width;
        private float height;

        
        private string spritePath = "Bullet.png";

        public float X
        {
            get { return x; }
            private set { x = value; }
        }
        public float Y
        {
            get { return y; }
            private set { y = value; }
        }
        public float Speed
        {
            get { return speed; }
            private set { speed = value; }
        }
        public float Width
        {
            get { return width; }
            private set { width = value; }
        }
        public float Height
        {
            get { return height; }
            private set { height = value; }
        }

        public Bullet(float startX, float startY, float speed)
        {
            this.X = startX;
            this.Y = startY;
            this.Speed = speed;

            
            this.Width = 10; 
            this.Height = 20; 
        }

        public void Update()
        {
            this.Y -= this.Speed * Program.deltaTime;
        }

        public void Draw()
        {
            
            Engine.Draw(spritePath, this.X, this.Y, 1, 1, 0, 0.5f, 0.5f);
        }
    }
}