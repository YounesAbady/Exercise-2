using exercise_1.Models;
using Spectre.Console;
using System.Text;
using System.Text.Json;
namespace exercise_1 // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var client = new HttpClient();
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase
            };
            //DataHandler.Deserialize();
            //Categories.Deserialize();
            var userInput = AnsiConsole.Prompt(
    new SelectionPrompt<string>()
        .Title("What's your [green]option[/]?")
        .PageSize(7)
        .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
        .AddChoices(new[] {
                    "For adding a category", "For adding a recipe", "For listing categories",
                    "For listing recipes", "For Editing categories","For editing Recipes","[red]Close the application[/]"

        }));
            string input = null;
            int counter;
            while (userInput != "x")
            {
                switch (userInput)
                {

                    case "For adding a category":
                        Console.WriteLine("Enter Category name :");
                        var category = Console.ReadLine();
                        var jsonCategory =JsonSerializer.Serialize(category);
                        var content = new StringContent(jsonCategory, Encoding.UTF8,"application/json");
                        var request = client.PostAsync($"https://localhost:7018/api/AddCategory/{category}",content);
                        var response = request.Result;
                        if (request.IsCompletedSuccessfully)
                            Console.WriteLine("Done!");
                        //Categories.AddCategory();
                        break;
                    case "For adding a recipe":
                        //DataHandler.AddRecipe();
                        break;
                    case "For listing categories":
                        var listRequest = client.GetStringAsync("https://localhost:7018/api/ListCategories");
                        var listResponse = listRequest.Result;

                        if (listResponse is not null)
                        {
                            var result = JsonSerializer.Deserialize<List<string>>(listResponse, options);
                            Categories.ListCategories(result);
                        }
                        break;
                    case "For listing recipes":
                        listRequest = client.GetStringAsync("https://localhost:7018/api/ListRecipes");
                        listResponse = listRequest.Result;

                        if (listResponse is not null)
                        {
                            var result = JsonSerializer.Deserialize<List<Recipe>>(listResponse, options);
                            DataHandler.ListRecipes(result);
                        }
                        break;
                    case "For Editing categories":
                        //Categories.EditCategory();
                        break;
                    case "Close the application":
                        //DataHandler.Serialize();
                        //Categories.Serialize();
                        Environment.Exit(0);
                        break;
                    case "For editing Recipes":
                        //DataHandler.EditRecipe();
                        break;
                    default:
                        Console.WriteLine("Enter a valid option!");
                        break;
                }

                //Console.WriteLine("Enter number for new opreation or x for closing the app");
                userInput = AnsiConsole.Prompt(
    new SelectionPrompt<string>()
        .Title("What's your [green]option[/]?")
        .PageSize(10)
        .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
        .AddChoices(new[] {
            "For adding a category", "For adding a recipe", "For listing categories",
            "For listing recipes", "For Editing categories","For editing Recipes","Close the application"

        }));
                Console.Clear();
                //userInput = Console.ReadLine();
            }

        }


    }
}
