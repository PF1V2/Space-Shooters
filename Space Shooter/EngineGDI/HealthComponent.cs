using System; 

namespace EngineGDI
{
    public class HealthComponent
    {
        public int CurrentHealth { get; private set; }
        public bool IsDead => CurrentHealth <= 0;

        //delegado
        public event Action OnDeath;          
        public event Action<int> OnDamage;    

        public HealthComponent(int startingHealth)
        {
            CurrentHealth = startingHealth;
        }

        public void TakeDamage(int damageAmount)
        {
            if (IsDead) return;

            CurrentHealth -= damageAmount;
            
            
            OnDamage?.Invoke(damageAmount);

            if (IsDead)
            {
                
                OnDeath?.Invoke();
            }
        }
        
        
        public void ResetHealth(int health)
        {
            CurrentHealth = health;
        }
    }
}