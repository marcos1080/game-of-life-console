using GameOfLife.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife.UI
{
    public abstract class UIBase
    {
        protected const int InitialOptionNumber = 1;

        /// <summary>
        /// Display the contents of the UI element.
        /// </summary>
        /// <remarks>
        /// Also handles the user input.
        /// </remarks>
        /// <returns></returns>
        public abstract Task Display();

        /// <summary>
        /// Check for user input between two integers.
        /// </summary>
        /// <param name="min">Minimum range value.</param>
        /// <param name="max">Maximum range value.</param>
        /// <param name="waitForEnter">
        /// Flag that determines if the user needs to press enter before checking the value.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Method throws an exception if the input was outside of bounds or a non integer value.
        /// </exception>
        /// <returns>Integer of chosen value.</returns>
        protected static int GetValidInput(int min, int max, Boolean waitForEnter = true)
        {
            string input;

            // Decide whether to wait for the enter key to be pressed before gathering input.
            if (waitForEnter)
                input = Console.ReadLine();
            else
                // Setting the true boolean value for the ReadKey method to prevent printing to the screen.
                input = Console.ReadKey(true).KeyChar.ToString();

            try
            {
                // Will throw a FormatException if the value is not an integer.
                int number = int.Parse(input);

                if (number < min || number > max)
                    throw new ArgumentException($"\r\nInput is out of bounds. Must be between {min} and {max}\r\n");

                // If only 1 char was required need to print it to screen.
                if (!waitForEnter)
                    Console.WriteLine(number);

                return number;
            }
            catch (FormatException)
            {
                throw new ArgumentException("\r\nInput not a valid number! Try again...\r\n");
            }
        }
    }
}
