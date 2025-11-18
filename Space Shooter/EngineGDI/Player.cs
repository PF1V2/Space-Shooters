using System.Windows.Forms;

namespace EngineGDI
{
    // Hereda de GameObject
    // Implementa IMovable (Interface) y IDamageable (Interface)
    public class Player : GameObject, IDamageable, IMovable
    {
        public float Speed { get; private set; }
        public HealthComponent Health { get; private set; }

        private Shooter shooter;

        public Player(float startX, float startY, float speed) : base()
        {
            Transform.Position = new Vector2(startX, startY);
            Speed = speed;

            Renderer.TexturePath = "Player.png";
            Renderer.Size = new Vector2(40, 40);

            Health = new HealthComponent(3); // 3 vidas
            shooter = new Shooter(0.5f);
        }

        public override void Update()
        {
            // Movimiento usando Input
            if (Engine.IsKeyDown(Keys.Left)) Move(-Speed * Program.deltaTime, 0);
            if (Engine.IsKeyDown(Keys.Right)) Move(Speed * Program.deltaTime, 0);

            // Limitar posición en pantalla usando Transform y Renderer
            float halfWidth = Renderer.Size.X / 2;

            if (Transform.Position.X - halfWidth < 0)
                Transform.Position.X = halfWidth;

            if (Transform.Position.X + halfWidth > Engine.Window.ClientSize.Width)
                Transform.Position.X = Engine.Window.ClientSize.Width - halfWidth;

            shooter.Update();
        }

        // Obligatorio de IMovable
        public void Move(float x, float y)
        {
            Transform.Position.X += x;
            Transform.Position.Y += y;
        }

        // Chequea si puede disparar
        public bool CanShoot()
        {
            return shooter.CanShoot();
        }

        // Obligatorio de IDamageable
        public void TakeDamage(int amount)
        {
            Health.TakeDamage(amount);
        }
    }
}