using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineGDI
{
    public class HealthComponent
    {
        public int CurrentHealth { get; private set; }

        // Propiedad calculada que nos dice si el objeto está "muerto"
        public bool IsDead => CurrentHealth <= 0;

        public HealthComponent(int startingHealth)
        {
            CurrentHealth = startingHealth;
        }

        public void TakeDamage(int damageAmount)
        {
            CurrentHealth -= damageAmount;
        }
    }
}