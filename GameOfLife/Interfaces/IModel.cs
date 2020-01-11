using GameOfLife.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife.Interfaces
{
    interface IModel
    {
        /// <summary>
        /// Save template to disk
        /// </summary>
        /// <param name="template">Template to save.</param>
        /// <returns></returns>
        Task SaveTemplate(Template template);

        /// <summary>
        /// Load template from disk
        /// </summary>
        /// <param name="templateName">Name of template file without extension.</param>
        /// <returns>Template</returns>
        Task<Template> LoadTemplateAsync(string templateName);

        /// <summary>
        /// Get a list of templates.
        /// </summary>
        /// <returns>string[] of template names.</returns>
        string[] GetTemplateList();

        /// <summary>
        /// Load game state from database.
        /// </summary>
        /// <remarks>
        /// If no game state is loaded false is returned.
        /// </remarks>
        /// <returns>Boolean</returns>
        Boolean LoadGame();

        /// <summary>
        /// Save game state to database.
        /// </summary>
        /// <returns></returns>
        Task SaveGame();

        /// <summary>
        /// Start the game.
        /// </summary>
        /// <returns></returns>
        Task PlayGame();

        /// <summary>
        /// Make a new game state.
        /// </summary>
        /// <param name="template">Template to insert on to game.</param>
        /// <param name="height">Game board height.</param>
        /// <param name="width">Game board width.</param>
        /// <param name="x">Template insertion column.</param>
        /// <param name="y">Template insertion row.</param>
        void NewGame(Template template, int height, int width, int x, int y);
    }
}
