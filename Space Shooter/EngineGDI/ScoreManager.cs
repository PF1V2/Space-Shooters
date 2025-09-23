
namespace EngineGDI
{
    public class ScoreManager
    {
        
        private static ScoreManager instance;
        public static ScoreManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ScoreManager();
                }
                return instance;
            }
        }
        private ScoreManager() { } 

       
        public int Score { get; private set; }

        
        public void AddScore(int points)
        {
            Score += points;
        }

        public void Reset()
        {
            Score = 0;
        }
    }
}