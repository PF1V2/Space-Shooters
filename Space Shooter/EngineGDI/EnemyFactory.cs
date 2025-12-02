using System.Collections.Generic; 

namespace EngineGDI
{
    public static class EnemyFactory
    {
        
        public static Enemy SpawnBasicEnemy(float x, float y, int hp, List<string> frames, ObjectPool<Enemy> pool)
        {
            Enemy enemy = pool.Get();

            
            enemy.Configure(x, y, hp, frames);

            return enemy;
        }
    }
}