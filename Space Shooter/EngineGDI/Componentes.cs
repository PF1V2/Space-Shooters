namespace EngineGDI
{
    //Vector2
    public struct Vector2
    {
        public float X;
        public float Y;

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static Vector2 Zero => new Vector2(0, 0);
    }

    //Transform
    public class Transform
    {
        public Vector2 Position;
        public Vector2 Scale;
        public float Rotation;

        public Transform()
        {
            Position = Vector2.Zero;
            Scale = new Vector2(1, 1);
            Rotation = 0;
        }
    }

    //Renderer
    public class Renderer
    {
        public string TexturePath { get; set; }
        public Vector2 Size { get; set; }
        public float OffsetX { get; set; } = 0.5f; // Pivot en el centro por defecto
        public float OffsetY { get; set; } = 0.5f;

        public void Draw(Transform transform)
        {
            if (!string.IsNullOrEmpty(TexturePath))
            {
                
                Engine.Draw(TexturePath,
                            transform.Position.X,
                            transform.Position.Y,
                            transform.Scale.X,
                            transform.Scale.Y,
                            transform.Rotation,
                            OffsetX,
                            OffsetY);
            }
        }
    }
}