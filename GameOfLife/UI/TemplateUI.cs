using GameOfLife.Data;
using GameOfLife.Interfaces;
using GameOfLife.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GameOfLife
{
    public class TemplateUI : UIBase
    {
        // Valid keys for cell input.
        private static readonly ConsoleKey Alive = ConsoleKey.O;
        private static readonly ConsoleKey Dead = ConsoleKey.X;

        // Template vriables
        private string name;
        private int height;
        private int width;

        // List that holds the valid keys for the template cell input.
        private readonly List<ConsoleKey> validKeys;

        public TemplateUI()
        {
            // Populate the list with the valid keyboard keys for template cells.
            validKeys = new List<ConsoleKey>() {
                Dead, 
                Alive
            };
        }

        public override async Task Display()
        {
            Console.WriteLine("---Create Template---\r\n");

            // Functionality separated out into private methods for readability.
            name = GetName();
            height = GetDimension(Template.MinHeight, Template.MaxHeight, "Enter Height: ");
            width = GetDimension(Template.MinWidth, Template.MaxWidth, "Enter Width: ");
            Template template = InputTemplate();

            // Save template to disk.
            await Model.Instance.SaveTemplate(template);
        }

        /// <summary>
        /// Prompts, receives and checks user input to see if it is a valid candidate for a file name.
        /// </summary>
        /// <returns>String</returns>
        private string GetName()
        {
            do
            {
                Console.Write("Enter template name: ");

                string input = Console.ReadLine();
                
                // Validate
                if (input.Length == 0)
                    Console.WriteLine("Name cannot be empty! Try again...");
                else if (input.Length > Template.MaxNameLength)
                    Console.WriteLine("Name is too long! Maximum {0} characters. Try again...", Template.MaxNameLength);
                else if (!ValidFileName(input))
                    Console.WriteLine("There are invalid characters in the template name. Try again...");
                else
                    return input;

            } while (true);
        }

        // 
        /// <summary>
        /// Checks if a string is a valid file name.
        /// </summary>
        /// <param name="input">Strign to check</param>
        /// <remarks>
        /// Must make sure the template name doesn't have characters that would prvent it from being saved as a file.
        /// </remarks>
        /// <returns>Boolean</returns>
        private Boolean ValidFileName(string input)
        {
            var match = input.IndexOfAny(Path.GetInvalidPathChars()) != -1;

            return match ? false : true;
        }

        /// <summary>
        /// Prompts the user to enter a number, then checks to see if it is valid.
        /// </summary>
        /// <param name="min">Minimim range value.</param>
        /// <param name="max">Maximim range value.</param>
        /// <param name="message">Prompt.</param>
        /// <returns>Integer</returns>
        private int GetDimension(int min, int max, string message)
        {
            do
            {
                try
                {
                    Console.Write(message);
                    int option = GetValidInput(min, max);
                    return option;

                } catch (ArgumentException e)
                {
                    Console.WriteLine(e.Message);
                }
            } while (true);

        }

        /// <summary>
        /// Receive input for template from user.
        /// </summary>
        /// <returns>Template</returns>
        private Template InputTemplate()
        {
            Cell[][] cells = new Cell[height][];

            Console.WriteLine("Enter Cells ('0' is Alive, 'X' is Dead)");

            // Loop through template cell array.
            for (int row = 0; row < height; row++)
            {
                Cell[] cellRow = new Cell[width];
                for (int column = 0; column < width; column++)
                {
                    // Get key
                    ConsoleKeyInfo currentKey;
                    do
                    {
                        // Setting the true flag prevents the key from being displayed.
                        currentKey = Console.ReadKey(true);
                    } while (!validKeys.Contains(currentKey.Key));

                    // Display and add to template
                    Console.Write(char.ToUpper(currentKey.KeyChar));
                    cellRow[column] = currentKey.Key == Alive ? Cell.Alive : Cell.Dead;
                }

                // Add row to cells.
                cells[row] = cellRow;
                Console.WriteLine();
            }

            return new Template(name, height, width, cells);
        }
    }
}