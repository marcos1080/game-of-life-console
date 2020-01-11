using System;
using System.Text;
using GameOfLife.Interfaces;
using GameOfLife.Utilities;

namespace GameOfLife
{
    /// <inheritdoc />
    public class Template : ITemplate
    {
        private Cell[][] _cells;
        private int _height;
        private int _width;

        /// <summary>
        /// Constructs a Template representation
        /// </summary>
        /// <param name="name">Name of the template</param>
        /// <param name="height">Height of the template</param>
        /// <param name="width">Width of the template</param>
        /// <param name="cells">Cells that make up this template</param>
        /// <remarks>
        /// You MUST NOT modify the signature (aka parameters) of this constructor. Marks will be deducted if modifications are made.
        /// You should validate the input to this constructor.
        /// </remarks>

        // Limit the size of the template.
        public const int MaxHeight = 25;
        public const int MaxWidth = 25;
        public const int MinHeight = 2;
        public const int MinWidth = 2;
        public const int MaxNameLength = 50;

        public Template(string name, int height, int width, Cell[][] cells)
        {
            Name = name;
            Height = height;
            Width = width;
            Cells = cells;
        }

        public string Name { get; }
        public int Height
        {
            get { return _height; }
            private set
            {
                if ((value < MinHeight) || (value > MaxHeight))
                    throw new ArgumentOutOfRangeException();

                _height = value;
            }
        }

        public int Width
        {
            get { return _width; }
            private set
            {
                if ((value < MinWidth) || (value > MaxWidth))
                    throw new ArgumentOutOfRangeException();

                _width = value;
            }
        }
        public Cell[][] Cells {
            get { return _cells; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();

                if ((value.Length != Height) || (value[0].Length != Width))
                    throw new ArgumentException("Jagged array dimensions don't match either Width or Height!");

                _cells = value;
            }
        }

        /// <summary>
        /// A text-based representation of this template.
        /// </summary>
        /// <remarks>
        /// When rendering cells, use '<see cref="Constants.AliveCell"/>' for alive cells & '<see cref="Constants.DeadCell"/>' for dead cells.
        /// </remarks>
        /// <returns>
        /// A string representing this template.
        /// </returns>
        public override string ToString()
        {
            return UtilityMethods.JaggedArrayToString(Cells);
        }
    }
}
