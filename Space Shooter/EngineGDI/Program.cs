using System;
using System.Drawing;
using System.Windows.Forms;

namespace EngineGDI
{
    static class Program
    {
       
        public static float deltaTime;
        private static DateTime startTime;
        private static float lastFrameTime;

        private static MenuManager menuManager;

        private static GameplayManager gameplayManager;

        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            
            Engine.Initialize("Space Shooters", 700, 900, false);
            GameManager.Instance.Init();

            menuManager = new MenuManager();
            startTime = DateTime.Now;
            gameplayManager = new GameplayManager();

            while (Engine.IsWindowOpen)
            {
                
                var currentTime = (float)(DateTime.Now - startTime).TotalSeconds;
                deltaTime = currentTime - lastFrameTime;
                lastFrameTime = currentTime;

                Engine.UpdateWindow();

                Engine.ClearDebug();
                Engine.Clear(Color.Black);

                
                switch (GameManager.Instance.CurrentState)
                {
                    case GameState.MainMenu:

                        menuManager.Update();
                        menuManager.Draw();
                        break;

                    case GameState.Gameplay:
                        gameplayManager.Update();
                        gameplayManager.Draw();
                        break;

                    case GameState.Win:
                        Engine.Draw("¡YOU WIN!", 200, 300, Brushes.LawnGreen, new Font("Consolas", 48));
                        Engine.Draw($"Final Score: {ScoreManager.Instance.Score}", 250, 400, Brushes.White, new Font("Consolas", 16));
                        Engine.Draw("Press ENTER to go back to the menu", 220, 450, Brushes.Gray, new Font("Consolas", 12));

                        if (Engine.IsKeyPressed(Keys.Enter))
                        {
                            gameplayManager.Reset();
                            ScoreManager.Instance.Reset();
                            GameManager.Instance.CurrentState = GameState.MainMenu;
                        }
                        break;

                    case GameState.Lose:
                        Engine.Draw("¡YOU LOOSE!", 200, 300, Brushes.Red, new Font("Consolas", 48));
                        Engine.Draw("Press ENTER to go back to the menu", 220, 400, Brushes.Gray, new Font("Consolas", 12));

                        if (Engine.IsKeyPressed(Keys.Enter))
                        {
                            gameplayManager.Reset();
                            ScoreManager.Instance.Reset();
                            GameManager.Instance.CurrentState = GameState.MainMenu;
                        }
                        break;
                }

                
                Engine.Window.Invalidate();
            }
        }
    }
}