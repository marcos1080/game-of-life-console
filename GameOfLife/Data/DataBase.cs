using System;
using GameOfLife.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace GameOfLife.Data
{
    public class DataBase
    {

        // Database table name
        private string _tableName;

        //Adopted for academic-purposes 
        //from Week 4 Tutorial ADO example.
        private static IConfigurationRoot Configuration { get; } =
            new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        public static string ConnectionString { get; } = Configuration["ConnectionString"];

        public DataBase(string tableName)
        {
            _tableName = tableName;
        }

        /// <summary>
        /// Load the game state from the database.
        /// </summary>
        /// <remarks>
        /// If no game state is found then null is returned. The calling method will need to
        /// handle the result.
        /// </remarks>
        /// <returns>GameOfLife object</returns>
        public IGameOfLife Load()
        {
            // The following resources were used as references to solve this problem.
            // https://docs.microsoft.com/en-us/dotnet/api/system.data.dataset?view=netframework-4.8
            // https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/populating-a-dataset-from-a-dataadapter

            Console.WriteLine("Loading previous game state...");

            // Defined out of the using block so if a failure occurs a null reference is returned.
            GameOfLife game = null;

            using (var connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand($"SELECT * FROM {_tableName}", connection)
                {
                    CommandType = CommandType.Text
                };

                SqlDataAdapter adapter = new SqlDataAdapter
                {
                    SelectCommand = command
                };

                DataSet dataSet = new DataSet();

                adapter.Fill(dataSet);

                DataTable table = dataSet.Tables[0];

                // Let the user know if there was no game found. A null is returned in this case.
                if (table.Rows.Count == 0)
                {
                    Console.WriteLine("No previous game present.\r\n");
                }
                else
                {
                    foreach (DataRow row in table.Rows)
                    {
                        int height = (int)row["Height"];
                        int width = (int)row["Width"];
                        Cell[][] cells = JsonConvert.DeserializeObject<Cell[][]>((string)row["Cells"]);

                        game = new GameOfLife(height, width, cells);
                    }
                }
            }

            return game;
        }
        
        /// <summary>
        /// Save the game state to database.
        /// </summary>
        /// <param name="game">The current game state.</param>
        /// <returns></returns>
        public async Task SaveAsync(IGameOfLife game)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand($"DELETE FROM {_tableName}", connection)
                {
                    CommandType = CommandType.Text
                };

                try
                {
                    await command.ExecuteNonQueryAsync();
                }
                catch (SqlException)
                {
                    // Could be thrown by there not being a valid table present or database communication issue.
                    throw new Exception("Error contacting the database");
                }

                String query = $"INSERT INTO {_tableName}(Height, Width, Cells) VALUES (@Height, @Width, @Cells)";
                command = new SqlCommand(query, connection)
                {
                    CommandType = CommandType.Text
                };

                // Adopted for academic-purposes.
                // from Week 4 Tutorial ADO example 6.
                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@Height",
                    SqlDbType = SqlDbType.Int,
                    Value = game.Height
                });

                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@Width",
                    SqlDbType = SqlDbType.Int,
                    Value = game.Width
                });

                command.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@Cells",
                    SqlDbType = SqlDbType.NVarChar,
                    Value = JsonConvert.SerializeObject(game.Cells)
                });

                if (await command.ExecuteNonQueryAsync() == 0)
                    throw new Exception("Error saving game");

                connection.Close();
            }
        }
    }
}
