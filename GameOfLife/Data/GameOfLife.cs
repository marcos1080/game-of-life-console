using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GameOfLife.Interfaces;
using GameOfLife.Utilities;

namespace GameOfLife
{
    /// <inheritdoc />
    public class GameOfLife : IGameOfLife
    {
        // Limit the size of the template.
        public const int MaxHeight = 50;
        public const int MaxWidth = 50;
        public const int MinHeight = 3;
        public const int MinWidth = 3;

        // variables for the object.
        private int _height;
        private int _width;
        private Cell[][] _cells;

        /// <summary>
        /// Constructs a Game of Life representation
        /// </summary>
        /// <param name="height">Height of the game board</param>
        /// <param name="width">Width of the game board</param>
        /// <param name="cells">The state of the game board to reload, if required</param>
        /// <remarks>
        /// You MUST NOT modify the signature (aka parameters) of this constructor. Marks will be deducted if modifications are made.
        /// You should validate the input to this constructor.
        /// </remarks>
        public GameOfLife(int height, int width, Cell[][] cells = null)
        {
            Height = height;
            Width = width;

            Cells = cells ?? BuildCellArray();
        }

        public int Height {
            get { return _height; }
            private set
            {
                if ((value < MinHeight) || (value > MaxHeight))
                    throw new ArgumentOutOfRangeException();

                _height = value;
            }
        }

        public int Width {
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
            private set
            {
                if ((value.Length != Height) || (value[0].Length != Width))
                    throw new ArgumentException("Jagged array dimensions don't match either Width or Height!");

                _cells = value;
            }
        }

        public void InsertTemplate(ITemplate template, int templateX, int templateY)
        {
            // Validate input.
            if ((templateX < 0) || (templateX + template.Width > Width))
                throw new ArgumentOutOfRangeException();

            if ((templateY < 0) || (templateY + template.Height > Height))
                throw new ArgumentOutOfRangeException();

            // Insert template. Loop through template to get cell states.
            for (int row = 0; row < template.Height; row++)
            {
                for (int column = 0; column < template.Width; column++)
                {
                    if (template.Cells[row][column] == Cell.Alive)
                        // Add offsets to place live cell on game board.
                        Cells[row + templateY][column + templateX] = Cell.Alive;
                }
            }
        }

        public async Task TakeTurnAsync(CancellationToken token = default)
        {
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(1), token);
            }
            catch (TaskCanceledException)
            {
                // If the token is cancelled do not process the next turn. Return with the current state.
                return;
            }

            // New jagged cell array to store the new state.
            Cell[][] newState = BuildCellArray();

            for (int row = 0; row < Height; row++)
            {
                for (int column = 0; column < Width; column++)
                {
                    int liveNeighbours = CountLiveNeighbours(row, column);

                    // Game logic here.

                    // 1.Any live cell with fewer than two live neighbours dies, as if caused by under-population. 
                    // 2.Any live cell with two or three live neighbours lives on to the next generation. 
                    // 3.Any live cell with more than three live neighbours dies, as if by over - population. 
                    // 4.Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.

                    if (Cells[row][column] == Cell.Alive)
                    {
                        // Test for rules 1 - 3. Current cell is live.
                        if ((liveNeighbours >= 2) && (liveNeighbours <= 3))
                            // Rule 2.
                            newState[row][column] = Cell.Alive;

                        // Rules 1 & 3 are automatically covered. The new cell array has all cells set to dead.
                        // This means we don't need to set them here.
                    }
                    else if (liveNeighbours == 3)
                        // Rule 4.
                        newState[row][column] = Cell.Alive;
                }
            }

            Cells = newState;
        }

        /// <summary>
        /// Build a new blank game jagged array.
        /// </summary>
        /// <remarks>
        /// Sets all cells to a dead state.
        /// </remarks>
        /// <returns>
        /// A jagged array of cells.
        /// </returns>
        private Cell[][] BuildCellArray()
        {
            Cell[][] cells = new Cell[Height][];

            for (int row = 0; row < Height; row++)
            {
                // Initialise each row as dead cells.
                cells[row] = Enumerable.Repeat(Cell.Dead, Width).ToArray();
            }

            return cells;
        }

        /// <summary>
        /// Count the number of living cells directly surrounding the supplied cell.
        /// </summary>
        /// <param name="y">Row co-ordinate of the cell.</param>
        /// <param name="x">Column co-ordinate of the cell.</param>
        /// <returns>
        /// An int of the number of living neighbour cells.
        /// </returns>
        private int CountLiveNeighbours(int y, int x)
        {
            // Counter for live neighbours.
            int count = 0;

            for (int row = y - 1; row <= y + 1; row++)
            {
                for (int column = x - 1; column <= x + 1; column++)
                {
                    // Check if the cell being checked is in the game array bounds.
                    if ((row >= 0) && (row < Height) && (column >= 0) && (column < Width) && !(row == y && column == x))
                    {
                        if (Cells[row][column] == Cell.Alive)
                            count++;
                    }
                }
            }

            return count;
        }

        /// <summary>
        /// A text-based representation of the current state of the game board.
        /// </summary>
        /// <remarks>
        /// When rendering cells, use '<see cref="Constants.AliveCell"/>' for alive cells & '<see cref="Constants.DeadCell"/>' for dead cells.
        /// </remarks>
        /// <returns>
        /// A string representing the current state of the game board.
        /// </returns>
        public override string ToString()
        {
            return UtilityMethods.JaggedArrayToString(Cells);
        }
    }
}
