using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineGDI
{
    public class Shooter
    {
        private float fireRate; 
        private float cooldownTimer; 

        public Shooter(float fireRate)
        {
            this.fireRate = fireRate;
            this.cooldownTimer = 0;
        }

       
        public void Update()
        {
            if (cooldownTimer > 0)
            {
                cooldownTimer -= Program.deltaTime;
            }
        }

        
        public bool CanShoot()
        {
            if (cooldownTimer <= 0)
            {
                cooldownTimer = fireRate; 
                return true;
            }
            return false;
        }
    }
}