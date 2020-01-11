namespace GameOfLife.Interfaces
{
    /// <summary>
    /// Representation of an initial game seed.
    /// </summary>
    /// <remarks>
    /// You may add properties and functions to this interface, but you MUST NOT remove those provided to you.
    /// </remarks>
    public interface ITemplate
    {
        /// <summary>
        /// Name (and filename) of a template
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Height of the template
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Width of the template
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Jagged Array of the alive and dead cells that make up this template, within the bounds of the <see cref="Height"/> and <see cref="Width"/>
        /// </summary>
        Cell[][] Cells { get; }
    }
}
