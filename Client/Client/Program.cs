global using Spectre.Console;
global using System.Text.Json;
using System.Text;
namespace Client // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            AnsiConsole.Write(
                new FigletText("Welcome")
                    .Centered()
                    .Color(Color.Gold1));
            var userInput = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What's your [green]option[/]?")
                    .PageSize(7)
                    .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                    .AddChoices(new[] {
                        "For adding a category", "For adding a recipe", "For listing categories",
                        "For listing recipes", "For editing categories","For editing recipes","[red]Close the application[/]"
                    }));
            string input = null;
            int counter;
            while (userInput != "x")
            {
                switch (userInput)
                {
                    case "For adding a category":
                        await DataHandler.AddCategory();
                        break;
                    case "For adding a recipe":
                        await DataHandler.AddRecipe();
                        break;
                    case "For listing categories":
                        await DataHandler.ListCategories();
                        break;
                    case "For listing recipes":
                        await DataHandler.ListRecipesWeb();
                        break;
                    case "For editing categories":
                        await Categories.EditCategories();
                        break;
                    case "[red]Close the application[/]":
                        Environment.Exit(0);
                        break;
                    case "For editing recipes":
                        await DataHandler.EditRecipes();
                        break;
                    default:
                        Console.WriteLine("[red]Enter a valid option![/]");
                        break;
                }
                userInput = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("What's your [green]option[/]?")
                        .PageSize(10)
                        .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                        .AddChoices(new[] {
                            "For adding a category", "For adding a recipe", "For listing categories",
                            "For listing recipes", "For editing categories","For editing recipes","[red]Close the application[/]"
                        }));
                Console.Clear();
            }
        }
    }
}