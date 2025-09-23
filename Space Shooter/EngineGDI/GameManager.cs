using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineGDI
{
    public class GameManager
    {
        
        private static GameManager instance;
        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameManager();
                }
                return instance;
            }
        }

        
        public GameState CurrentState { get; set; }

        
        private GameManager() { }

        
        public void Init()
        {
            CurrentState = GameState.MainMenu;
        }
    }
}