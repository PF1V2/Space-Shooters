namespace EngineGDI
{
    // Hereda de GameObject (Herencia)
    // Implementa IPoolable para poder ser reciclada en el ObjectPool
    public class Bullet : GameObject, IPoolable
    {
        public float Speed { get; set; }

        public Bullet() : base() // Llama al constructor de GameObject para crear Transform y Renderer
        {
            Renderer.TexturePath = "Bullet.png";
            // Ajustamos el tamaño visual de la bala
            Renderer.Size = new Vector2(10, 20);
        }

        // Método obligatorio de IPoolable: limpia el objeto para reusarlo
        public void Reset()
        {
            Speed = 0;
            Transform.Position = Vector2.Zero;
            IsActive = false;
        }

        // Método para "disparar" la bala (configurarla)
        public void Initialize(float startX, float startY, float speed)
        {
            Transform.Position = new Vector2(startX, startY);
            Speed = speed;
            IsActive = true;
        }

        public override void Update()
        {
            if (!IsActive) return;

            // Movemos la bala hacia arriba usando el componente Transform
            Transform.Position.Y -= Speed * Program.deltaTime;
        }
    }
}