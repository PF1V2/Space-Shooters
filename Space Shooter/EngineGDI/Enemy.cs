using System.Collections.Generic;

namespace EngineGDI
{
    // Hereda de GameObject
    // Implementa IDamageable (Interface) y IPoolable (Interface)
    public class Enemy : GameObject, IDamageable, IPoolable
    {
        public HealthComponent Health { get; private set; }
        private Animation animation;

        public Enemy() : base()
        {
            // Configuración inicial del Renderer
            Renderer.TexturePath = "Enemy1.png";
            Renderer.Size = new Vector2(40, 40);

            // Inicializamos la vida y nos suscribimos al evento de muerte
            Health = new HealthComponent(1);
            Health.OnDeath += HandleDeath; // Suscripción al evento (Punto 3)

            // Animación
            List<string> enemyFrames = new List<string> { "Enemy1.png", "Enemy1v2.png" };
            animation = new Animation(0.5f, enemyFrames, true);
        }

        // Método para configurar el enemigo cuando sale del Factory/Pool
        public void Configure(float x, float y)
        {
            Transform.Position = new Vector2(x, y);
            Reset();
        }

        // Obligatorio de IPoolable
        public void Reset()
        {
            Health.ResetHealth(1); // Reseteamos la vida
            IsActive = true;
        }

        // Manejador del evento OnDeath
        private void HandleDeath()
        {
            IsActive = false; // El enemigo deja de dibujarse/actualizarse
            // Aquí podrías reproducir un sonido de muerte si quisieras
        }

        // Obligatorio de IDamageable
        public void TakeDamage(int amount)
        {
            Health.TakeDamage(amount);
        }

        public void Move(float deltaX, float deltaY)
        {
            Transform.Position.X += deltaX;
            Transform.Position.Y += deltaY;
        }

        public override void Update()
        {
            if (!IsActive) return;

            animation.Update();
            // Actualizamos la textura del Renderer con el frame actual de la animación
            Renderer.TexturePath = animation.CurrentTexture;
        }
    }
}