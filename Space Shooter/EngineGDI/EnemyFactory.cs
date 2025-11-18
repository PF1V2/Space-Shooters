namespace EngineGDI
{
    
    public static class EnemyFactory
    {
        
        public static Enemy SpawnBasicEnemy(float x, float y, ObjectPool<Enemy> pool)
        {
            
            Enemy enemy = pool.Get();

            
            enemy.Configure(x, y);

            
            return enemy;
        }
    }
}