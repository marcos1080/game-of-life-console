using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GameOfLife.Data
{
    public class TemplateIO
    {
        private readonly string _extension;
        private readonly string _folder;

        public TemplateIO(string folder, string extension)
        {
            _extension = extension;
            _folder = folder;
        }

        /// <summary>
        /// Save a template to a file.
        /// </summary>
        /// <param name="template">Template to be saved.</param>
        /// <returns></returns>
        public async Task SaveToDiskAsync(Template template)
        {
            string JSON = JsonConvert.SerializeObject(template);

            // Test if directory exists
            if (!Directory.Exists(_folder))
                Directory.CreateDirectory(_folder);

            string path = Path.Combine(_folder, $"{template.Name}.{_extension}");

            using (var writer = new StreamWriter(path))
            {
                try
                {
                    await writer.WriteLineAsync(JSON);
                    Console.WriteLine("\nTemplate Created.\n");
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"IOException:\n{ex.Message}");
                }
            }
        }

        /// <summary>
        /// Load a template from a file.
        /// </summary>
        /// <param name="templateName"></param>
        /// <returns></returns>
        public async Task<Template> LoadFromDiskAsync(string templateName)
        {
            string path = Path.Combine(_folder, $"{templateName}.{_extension}");

            using (var reader = new StreamReader(path))
            {
                try
                {
                    string fileContents = await reader.ReadLineAsync();
                    Template template = JsonConvert.DeserializeObject<Template>(fileContents);
                    return template;
                }
                catch (IOException ex)
                {
                    // If an exception is thrown then the method returns a null.
                    Console.WriteLine($"IOException:\n{ex.Message}");
                }
            }

            return null;
        }

        /// <summary>
        /// Get an array of all the templates in the directory.
        /// </summary>
        /// <returns>String[]</returns>
        public string[] GetTemplateList()
        {
            string[] filePaths = Directory.GetFiles(_folder, $"*.{_extension}");
            string[] templateNames = new string[filePaths.Length];

            // Remove extensions.
            for(int i = 0; i < filePaths.Length; i++)
            {
                templateNames[i] = Path.GetFileNameWithoutExtension(filePaths[i]);
            }

            return templateNames;
        }
    }
}
