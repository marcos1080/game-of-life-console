using GameOfLife.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife.UI
{
    public class NewGameUI : UIBase
    {
        public override async Task Display()
        {
            string[] templateNames = Model.Instance.GetTemplateList();

            if (templateNames.Length == 0)
            {
                Console.WriteLine("\r\nNo templates found. Please create one before continuing.\r\n");
                return;
            }

            // Display Menu
            Console.WriteLine("\r\n--- New Game ---\r\n");
            Console.WriteLine("Templates:");

            // Using a for rather than a foreach here in order to have an index number for the template.
            for (int i = 0; i < templateNames.Length; i++)
            {
                // InitialOptionNumber set as a const in the parent class.
                Console.WriteLine($"{i + InitialOptionNumber}. {templateNames[i]}");
            }

            // Get option from the user.
            Console.Write("\r\nSelect a Template: ");
            // No exceptions are caught in this method by design. They are handled by the calling method.
            int option = GetValidInput(InitialOptionNumber, templateNames.Length);

            Console.WriteLine();

            // Load Template
            Template template = await Model.Instance.LoadTemplateAsync(templateNames[option - InitialOptionNumber]);

            if (template == null)
            {
                Console.WriteLine("An error occured while loading the template.");
                return;
            }

            // Display template data.
            Console.WriteLine("Template\r\n");
            Console.WriteLine(String.Format("{0,-8}{1}", "Name:", template.Name));
            Console.WriteLine(String.Format("{0,-8}{1}", "Height:", template.Height));
            Console.WriteLine(String.Format("{0,-8}{1}\r\n", "Width:", template.Width));
            Console.WriteLine(template);

            // Get input for the new game state.
            Console.Write($"Enter game height (must be at least {template.Height}): ");
            int height = GetValidInput(template.Height, GameOfLife.MaxHeight);

            Console.Write($"Enter game width (must be at least {template.Width}): ");
            int width = GetValidInput(template.Width, GameOfLife.MaxWidth);

            int max = width - template.Width;
            Console.Write($"Enter template x coordinate (cannot be more than {max}): ");
            int x = GetValidInput(0, max);

            max = height - template.Height;
            Console.Write($"Enter template y coordinate (cannot be more than {max}): ");
            int y = GetValidInput(0, max);

            // Create a new game.
            Model.Instance.NewGame(template, height, width, x, y);
        }
    }
}
