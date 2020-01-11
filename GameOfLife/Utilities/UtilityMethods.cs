using GameOfLife.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameOfLife.Utilities
{
    static class UtilityMethods
    {
        /// <summary>
        /// Display a jagged array of cells as a string.
        /// </summary>
        /// <param name="cells">Jagged array to represent.</param>
        /// <returns>String</returns>
        public static String JaggedArrayToString(Cell[][] cells)
        {
            StringBuilder sb = new StringBuilder();

            // Build graphical representation.
            foreach (var row in cells)
            {
                // New line
                string[] rowChars = new string[row.Length];

                // Add cell to the line.
                for (int column = 0; column < row.Length; column++)
                {
                    rowChars[column] = row[column] == Cell.Alive ? Constants.AliveCell : Constants.DeadCell;
                }

                // Add line to the string.
                sb.Append($"{string.Join("", rowChars)}\r\n");
            }

            return sb.ToString();
        }
    }
}
