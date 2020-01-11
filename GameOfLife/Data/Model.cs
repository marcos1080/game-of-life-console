using GameOfLife.Interfaces;
using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace GameOfLife.Data
{
    public class Model : IModel
    {
        // Reference for the singleton pattern
        private static Model _model;

        // 
        private IGameOfLife game;
        private readonly TemplateIO _templateIO;
        private readonly DataBase _database;

        // Singleton constructor.
        private Model()
        {
            _templateIO = new TemplateIO(Constants.TemplatesDirectoryName, "json");
            _database = new DataBase("GameState");
        }

        // Singleton reference.
        public static Model Instance
        {
            get
            {
                if (_model == null)
                    _model = new Model();

                return _model;
            }
        }

        public async Task SaveTemplate(Template template)
        {
            await _templateIO.SaveToDiskAsync(template);
        }

        public async Task<Template> LoadTemplateAsync(string templateName)
        {
            return await _templateIO.LoadFromDiskAsync(templateName);
        }

        public string[] GetTemplateList()
        {
            return _templateIO.GetTemplateList();
        }

        public Boolean LoadGame()
        {
            try
            {
                game = _database.Load();

                if (game == null)
                    return false;

                return true;
            }
            catch (SqlException)
            {
                Console.WriteLine("No game table present in the database.\r\n");
                return false;
            }
        }

        public async Task SaveGame()
        {
            try
            {
                await _database.SaveAsync(game);
            }
            catch (Exception e)
            {
                Console.WriteLine($"\r\n{e.Message}\r\n\r\nGame state not saved\r\n");
            }
        }

        public async Task PlayGame()
        {
            // The solution to the read key to cancel input problem is based on this article.
            // https://johnthiriet.com/cancel-asynchronous-operation-in-csharp/#

            var source = new CancellationTokenSource();

            // Task to handle keyboard input to stop game.
            var cancelInput = Task.Run(() =>
            {
                Console.ReadKey(true);
                source.Cancel();
            });

            while (!source.IsCancellationRequested)
            {
                Console.Clear();
                Console.WriteLine(game.ToString());
                Console.Write("Press any key to stop game...");

                // Based on tutorial week 5
                await Task.WhenAny(cancelInput, game.TakeTurnAsync(source.Token));
            }

            // Need to clear screen one last time before showing menu.
            Console.Clear();
        }

        public void NewGame(Template template, int height, int width, int x, int y)
        {
            game = new GameOfLife(height, width);
            game.InsertTemplate(template, x, y);
        }
    }
}
