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
            Renderer.Size = new Vector2(40, 40);
            Health = new HealthComponent(1);
            Health.OnDeath += HandleDeath;
        }

        // Método para configurar el enemigo cuando sale del Factory/Pool
        public void Configure(float x, float y, int hp, List<string> frames)
        {
            Transform.Position = new Vector2(x, y);

            // CREAMOS LA ANIMACIÓN AQUÍ, con las imágenes específicas del nivel
            // (Si solo pasas una imagen en la lista, se verá estático, lo cual está bien)
            animation = new Animation(0.5f, frames, true);

            // Seteamos la primera textura inmediatamente para que no se vea vacío un frame
            Renderer.TexturePath = frames[0];

            Reset(hp);
        }

        // Obligatorio de IPoolable
        public void Reset(int hp = 1)
        {
            Health.ResetHealth(hp); // Usamos el valor que nos pasan
            IsActive = true;
        }

        public void Reset()
        {
            Reset(1);
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

            // Aseguramos que la animación exista antes de actualizarla
            if (animation != null)
            {
                animation.Update();
                Renderer.TexturePath = animation.CurrentTexture;
            }
        }
    }
}