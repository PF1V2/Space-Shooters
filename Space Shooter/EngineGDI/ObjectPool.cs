using System.Collections.Generic;

namespace EngineGDI
{
    
    public class ObjectPool<T> where T : GameObject, IPoolable, new()
    {
        
        private Queue<T> pool = new Queue<T>();

        //pool perdir
        public T Get()
        {
            if (pool.Count > 0)
            {
                //sacar si hay
                T obj = pool.Dequeue();
                obj.Reset();      // reset pool
                obj.IsActive = true; // activar
                return obj;
            }
            else
            {
                
                T newObj = new T();
                newObj.IsActive = true;
                return newObj;
            }
        }

        
        public void ReturnToPool(T obj)
        {
            obj.IsActive = false; 
            pool.Enqueue(obj);    
        }
    }
}