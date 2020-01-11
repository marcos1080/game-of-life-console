using System;
using System.Threading.Tasks;

namespace GameOfLife.UI
{
    class MainMenu : UIBase
    {
        private readonly UIBase templateUI;
        private readonly UIBase playGameMenu;

        public MainMenu()
        {
            templateUI = new TemplateUI();
            playGameMenu = new PlayGameMenu();
        }

        public override async Task Display()
        {
            // Used to control exiting the main loop.
            Boolean run = true;

            do
            {
                Console.WriteLine("--- Game of Life ---");
                Console.WriteLine("1. Create Template");
                Console.WriteLine("2. Play Game");
                Console.WriteLine("3. Exit\r\n");
                Console.Write("Enter and Option: ");

                try
                {
                    // Invalid input will result in an ArgumentException.
                    int option = GetValidInput(1, 3, false);
                    Console.WriteLine();

                    switch (option)
                    {
                        case 1:
                            await templateUI.Display();
                            break;
                        case 2:
                            await playGameMenu.Display();
                            break;
                        case 3:
                            Console.WriteLine("Goodbye.");
                            run = false;
                            break;
                    }
                } catch (ArgumentException e)
                {
                    Console.WriteLine(e.Message);
                }
                
            } while (run);
        }
    }
}
