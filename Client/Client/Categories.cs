using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class Categories
    {
        public static List<string> CategoriesNames { get; set; } = new List<string>();
        public static void ListCategories(List<string> categories)
        {
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
        public static void EditCategories() 
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase
            };
            var client = new HttpClient();
            string input=null;
            var listRequest = client.GetStringAsync("https://localhost:7018/api/list-categories");
            var listResponse = listRequest.Result;

            if (listResponse is not null)
            {
                var result = JsonSerializer.Deserialize<List<string>>(listResponse, options);
                Categories.ListCategories(result);
                Console.WriteLine("Please select number of category to edit");
                input = Console.ReadLine();
                Console.WriteLine("If you want to delete it enter x or enter new name to edit it");
                string newName = Console.ReadLine();
                if (newName == "x")
                {
                    var request = client.DeleteAsync($"https://localhost:7018/api/delete-category/{result[int.Parse(input) - 1]}");
                    var response = request.Result;
                    if (request.IsCompletedSuccessfully)
                        AnsiConsole.Write(new Markup("[green]Done !![/]\n\n"));
                }
                else
                {
                    var jsonCategory = JsonSerializer.Serialize(newName);
                    var content = new StringContent(jsonCategory, Encoding.UTF8, "application/json");
                    var request = client.PutAsync($"https://localhost:7018/api/update-category/{input}/{newName}", content);
                    var response = request.Result;
                    if (request.IsCompletedSuccessfully)
                        AnsiConsole.Write(new Markup("[green]Done !![/]\n\n"));
                }
            }
        }
    }
}
