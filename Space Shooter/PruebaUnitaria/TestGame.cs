using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EngineGDI;

namespace PruebaUnitaria
{
    [TestClass]
    public class TestGame
    {
        [TestMethod]
        public void TestHealthTakesDamage()
        {
            
            int vidaInicial = 100;
            HealthComponent hp = new HealthComponent(vidaInicial);

            
            hp.TakeDamage(20);

            
            Assert.AreEqual(80, hp.CurrentHealth);
        }

        [TestMethod]
        public void TestDeathEvent()
        {
            
            HealthComponent hp = new HealthComponent(10); 
            bool eventoDisparado = false;

            // Nos suscribimos al evento manualmente para ver si salta
            hp.OnDeath += () =>
            {
                eventoDisparado = true;
            };

            // Act
            hp.TakeDamage(10); // Le quitamos toda la vida

            // Assert
            Assert.IsTrue(hp.IsDead, "El componente debería estar muerto");
            Assert.IsTrue(eventoDisparado, "El evento OnDeath debería haberse disparado");
        }

    }
}
