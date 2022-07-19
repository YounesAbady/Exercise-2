using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Client
{
    internal class Categories
    {
        public static IConfiguration Config = new ConfigurationBuilder()
        .AddJsonFile("appSettings.json")
        .AddEnvironmentVariables()
        .Build();
        public static HttpClient Client = new HttpClient();
        public static List<string> CategoriesNames { get; set; } = new List<string>();
        public static void ListCategories(List<string> categories)
        {
            ArgumentNullException.ThrowIfNull(categories);
            int counter = 1;
            // Create the tree
            var root = new Tree("Categories");
            foreach (var category in categories)
            {
                // Add some nodes
                var node = root.AddNode($"{counter}-[aqua]{category}[/]");
                counter++;
            }
            // Render the tree
            AnsiConsole.Write(root);
        }
        public static async Task EditCategories()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase
            };
            string input = null;
            var listRequest = await Client.GetStringAsync($"{Config["BaseAddress"]}/api/list-categories");
            if (listRequest is not null)
            {
                var result = JsonSerializer.Deserialize<List<string>>(listRequest, options);
                Categories.ListCategories(result);
                AnsiConsole.Write(new Markup("Please select number of category to [green]edit :[/]"));
                input = Console.ReadLine();
                AnsiConsole.Write(new Markup("If you want to [red]delete it enter x[/] or enter new name to edit it:"));
                string newName = Console.ReadLine();
                if (string.IsNullOrEmpty(newName))
                    throw new InvalidOperationException("Cant be empty");
                if (newName == "x")
                {
                    var request = await Client.DeleteAsync($"{Config["BaseAddress"]}/api/delete-category/{result[int.Parse(input) - 1]}");
                    if (request.IsSuccessStatusCode)
                        AnsiConsole.Write(new Markup("[green]Done !![/]\n\n"));
                }
                else
                {
                    var jsonCategory = JsonSerializer.Serialize(newName);
                    var content = new StringContent(jsonCategory, Encoding.UTF8, "application/json");
                    var request = await Client.PutAsync($"{Config["BaseAddress"]}/api/update-category/{input}/{newName}", content);
                    if (request.IsSuccessStatusCode)
                        AnsiConsole.Write(new Markup("[green]Done !![/]\n\n"));
                }
            }
        }
    }
}
