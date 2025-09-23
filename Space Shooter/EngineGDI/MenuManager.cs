using System.Collections.Generic; 
using System.Drawing;             
using System.Windows.Forms;

namespace EngineGDI
{
    public class MenuManager
    {
        
        private List<string> menuOptions;
        
        private int selectedOption;

        
        private Brush unselectedColor = Brushes.Gray;
        private Brush selectedColor = Brushes.White;
        private Font optionFont = new Font("Consolas", 24);
        private Font optionFont2 = new Font("Consolas", 40);

        public MenuManager()
        {
            menuOptions = new List<string>
            {
                "PLAY",
                "EXIT"
            };
            selectedOption = 0; 
        }

        public void Update()
        {
            
            if (Engine.IsKeyPressed(Keys.Up))
            {
                selectedOption--;
                if (selectedOption < 0)
                {
                    selectedOption = menuOptions.Count - 1; 
                }
            }

            
            if (Engine.IsKeyPressed(Keys.Enter))
            {
                switch (selectedOption)
                {
                    case 0: 
                            
                        GameManager.Instance.CurrentState = GameState.Gameplay;
                        break;

                    case 1: 
                            
                        Engine.CloseWindow();
                        break;
                }
            }

            if (Engine.IsKeyPressed(Keys.Down))
            {
                selectedOption++;
                if (selectedOption >= menuOptions.Count)
                {
                    selectedOption = 0; 
                }
            }

            
        }

        public void Draw()
        {
            
            Engine.Draw("Enemy1.png", 400, 200, 10, 10, 0, 0.5f, 0.5f);
            Engine.Draw("Player.png", 410, 600, 10, 10, 0, 0.5f, 0.5f);


            
            Engine.Draw("Space Shooters",100, 150, selectedColor, new Font("Consolas", 50, FontStyle.Bold));

            for (int i = 0; i < menuOptions.Count; i++)
            {
                Brush currentColor = (i == selectedOption) ? selectedColor : unselectedColor;

                
                Engine.Draw(menuOptions[i], 300, 300 + i * 70, currentColor, optionFont2);
            }
        }
    }
}