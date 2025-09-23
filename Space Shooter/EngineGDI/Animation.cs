using System.Collections.Generic;

namespace EngineGDI
{
    public class Animation
    {
        private bool isLoop;
        private float speed;
        private float currentTime;
        private int currentFrame = 0;
        private List<string> textures = new List<string>();

        public bool IsFinished { get; private set; }

        public string CurrentTexture => textures[currentFrame];

        public Animation(float speed, List<string> textures = null, bool isloop = false) 
        { 
            this.speed = speed;
            this.IsFinished = false;
            this.isLoop = isloop;

            if (textures != null)
            {
                this.textures = textures;
            }
        }

        public void Update()
        {
            if (IsFinished) return;


            currentTime += Program.deltaTime;

            if (currentTime >= speed)
            {
                currentTime = 0;
                currentFrame++;

                if (currentFrame >= textures.Count)
                {
                    if (isLoop)
                    {
                        currentFrame = 0;
                    }
                    else
                    {
                        currentFrame = textures.Count - 1;
                        IsFinished = true;
                    }
                }
            }
        }
    }
}
