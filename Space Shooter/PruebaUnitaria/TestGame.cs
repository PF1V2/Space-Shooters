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
            // Arrange (Preparar)
            int vidaInicial = 100;
            HealthComponent hp = new HealthComponent(vidaInicial);

            // Act (Actuar)
            hp.TakeDamage(20);

            // Assert (Verificar)
            // Esperamos que 100 - 20 sea 80
            Assert.AreEqual(80, hp.CurrentHealth);
        }

        [TestMethod]
        public void TestDeathEvent()
        {
            // Arrange
            HealthComponent hp = new HealthComponent(10); // Tiene 10 de vida
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

        [TestMethod]
        public void TestObjectPoolRecycling()
        {
            // Arrange
            ObjectPool<Bullet> pool = new ObjectPool<Bullet>();

            // Act - Paso 1: Sacamos una bala y la "usamos"
            Bullet b1 = pool.Get();
            b1.IsActive = true;

            // Act - Paso 2: La devolvemos al pool
            pool.ReturnToPool(b1);

            // Act - Paso 3: Pedimos una bala de nuevo
            Bullet b2 = pool.Get();

            // Assert
            // Verificamos que b2 sea EXACTAMENTE el mismo objeto en memoria que b1
            // Si son iguales, significa que el pool RECICLÓ b1 en lugar de crear basura nueva.
            Assert.AreSame(b1, b2, "El pool debería devolver el mismo objeto reciclado");
            Assert.IsTrue(b2.IsActive, "El objeto reciclado debería estar activo");
        }
    }
}
