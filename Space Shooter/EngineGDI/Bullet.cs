namespace EngineGDI
{
    
    public class Bullet : GameObject, IPoolable
    {
        public float Speed { get; set; }

        public Bullet() : base() 
        {
            Renderer.TexturePath = "Bullet.png";
            
            Renderer.Size = new Vector2(10, 20);
        }

        
        public void Reset()
        {
            Speed = 0;
            Transform.Position = Vector2.Zero;
            IsActive = false;
        }

        
        public void Initialize(float startX, float startY, float speed)
        {
            Transform.Position = new Vector2(startX, startY);
            Speed = speed;
            IsActive = true;
        }

        public override void Update()
        {
            if (!IsActive) return;

            
            Transform.Position.Y -= Speed * Program.deltaTime;
        }
    }
}