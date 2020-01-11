using System.Threading;
using System.Threading.Tasks;

namespace GameOfLife.Interfaces
{
    /// <summary>
    /// Defines the state of a cell (Alive or Dead)
    /// </summary>
    /// <remarks>
    /// DO NOT modify this enumeration! Marks will be deducted if this enumeration is changed.
    /// </remarks>
    public enum Cell
    {
        Alive,
        Dead
    }

    /// <summary>
    /// Representation of a session of the Game of Life
    /// </summary>
    /// <remarks>
    /// You may add properties and functions to this interface, but you MUST NOT remove those provided to you.
    /// </remarks>
    public interface IGameOfLife
    {
        /// <summary>
        /// Height of the game board
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Width of the game board
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Jagged Array representing the current state of the game board with alive and dead cells
        /// </summary>
        Cell[][] Cells { get; }

        /// <summary>
        /// Places the provided template at the specified co-ordinates
        /// </summary>
        /// <remarks>
        /// You should validate the input to this function, as well as if the insertion is even possible (is it out of bounds?)
        /// </remarks>
        /// <param name="template">The template to insert</param>
        /// <param name="templateX">The x (horizontal) co-ordinate to place the template</param>
        /// <param name="templateY">The y (vertical) co-ordinate to place the template</param>
        void InsertTemplate(ITemplate template, int templateX, int templateY);

        /// <summary>
        /// "Ticks" the game from one generation to the next.
        /// </summary>
        /// <remarks>
        /// This method should generate a new <see cref="Cells"/> jagged array
        /// based on the game rules as specified in the assignment specification.
        /// It should also use the <see cref="Task.Delay(System.TimeSpan,CancellationToken)"/> method to
        /// 'tick' the game forward.
        /// </remarks>
        /// <param name="token">
        /// A token indicating if the turn has been cancelled.
        /// If none is provided it will default to an empty token.
        /// </param>
        /// <returns>
        /// A task used in asynchronous operations - it should be awaited by the caller.
        /// </returns>
        Task TakeTurnAsync(CancellationToken token = default);
    }
}
