namespace EngineGDI
{
    
    public abstract class GameObject
    {
        
        public Transform Transform { get; private set; }
        public Renderer Renderer { get; private set; }

        
        public bool IsActive { get; set; } = true;

        public GameObject()
        {
            
            Transform = new Transform();
            Renderer = new Renderer();
        }

        
        public virtual void Update() { }

        public virtual void Draw()
        {
            if (IsActive)
            {
                
                Renderer.Draw(Transform);
            }
        }
    }
}