using GameOfLife.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GameOfLife.UI
{
    class PlayGameMenu : UIBase
    {
        private readonly UIBase newGameUI;

        public PlayGameMenu()
        {
            newGameUI = new NewGameUI();
        }

        public override async Task Display()
        {
            Console.WriteLine("--- Play Game --- \r\n");
            Console.WriteLine("1. New Game");
            Console.WriteLine("2. Resume Game\r\n");
            Console.Write("Enter and Option: ");

            try
            {
                // Invalid input throws an ArgumentException.
                int option = GetValidInput(1, 2, false);

                switch (option)
                {
                    case 1:
                        await newGameUI.Display();
                        break;
                    case 2:
                        Console.WriteLine("--- Resume Game ---\r\n");

                        // If no game loaded exit method.
                        if (!Model.Instance.LoadGame())
                            return;

                        break;
                }

                // Start game loop.
                await Model.Instance.PlayGame();

                // Once the game has been interrupted, save to the database.
                await Model.Instance.SaveGame();

            } catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
