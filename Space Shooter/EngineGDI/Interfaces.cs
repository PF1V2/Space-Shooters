namespace EngineGDI
{
    //recibir daño 
    public interface IDamageable
    {
        void TakeDamage(int amount);
    }

    //reset pool
    public interface IPoolable
    {
        void Reset();
    }

    //mover
    public interface IMovable
    {
        void Move(float x, float y);
    }
}