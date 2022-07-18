using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Client
{
    internal class DataHandler
    {
        public static IConfiguration Config = new ConfigurationBuilder()
        .AddJsonFile("appSettings.json")
        .AddEnvironmentVariables()
        .Build();
        public static HttpClient Client = new HttpClient();
        static int counter;
        public static void ListRecipesUi(List<Recipe> recipes)
        {
            ArgumentNullException.ThrowIfNull(recipes);
            int recipesCounter = 1;
            var table = new Table();
            table.Centered();
            table.Collapse();
            AnsiConsole.Write(
                new FigletText("Recipes")
                    .Centered()
                    .Color(Color.Green));
            table.AddColumn(new TableColumn("[aqua]Title[/]").Centered());
            table.AddColumn(new TableColumn("[aqua]Ingredients[/]").Centered());
            table.AddColumn(new TableColumn("[aqua]Instructions[/]").Centered());
            table.AddColumn(new TableColumn("[aqua]Categories[/]").Centered());
            table.Border(TableBorder.Rounded);
            foreach (Recipe recipe in recipes)
            {
                var ingredients = new StringBuilder();
                counter = 1;
                foreach (string item in recipe.Ingredients)
                {
                    ingredients.Append($"{counter} - {item} \n");
                    counter++;
                }
                counter = 1;
                var instructions = new StringBuilder();
                foreach (string item in recipe.Instructions)
                {
                    instructions.Append($"{counter} - {item} \n");
                    counter++;
                }
                counter = 1;
                var categories = new StringBuilder();
                foreach (string item in recipe.Categories)
                {
                    categories.Append($"{counter} - {item} \n");
                    counter++;
                }
                table.AddRow($"{recipesCounter} - {recipe.Title}", ingredients.ToString(), instructions.ToString(), categories.ToString());
                table.AddEmptyRow();
                table.AddRow("___________", "___________", "___________", "___________");
                recipesCounter++;
            }
            AnsiConsole.Write(table);
        }
        public static void AddCategory()
        {
            AnsiConsole.Write(new Markup("Enter category [aqua]name :[/]"));
            var category = Console.ReadLine();
            if (string.IsNullOrEmpty(category))
                throw new InvalidOperationException("Cant be empty");
            var jsonCategory = JsonSerializer.Serialize(category);
            var content = new StringContent(jsonCategory, Encoding.UTF8, "application/json");
            var request = Client.PostAsync($"{Config["BaseAddress"]}/api/add-category/{category}", content);
            var response = request.Result;
            if (request.IsCompletedSuccessfully)
                AnsiConsole.Write(new Markup("[green]Done !![/]\n\n"));
        }
        public static void AddRecipe()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase
            };
            string input = null;
            Recipe recipe = new Recipe();
            AnsiConsole.Write(new Markup("Enter recipe [aqua]name :[/]"));
            recipe.Title = Console.ReadLine();
            if (string.IsNullOrEmpty(recipe.Title))
                throw new InvalidOperationException("Cant be empty");
            for (counter = 1; input != "x"; counter++)
            {
                AnsiConsole.Write(new Markup($"Enter ingredient number {counter} or [red]x[/] to go to the next step:"));
                input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                    throw new InvalidOperationException("Cant be empty");
                if (input == "x") break;
                recipe.Ingredients.Add(input);
            }
            input = null;
            for (counter = 1; input != "x"; counter++)
            {
                AnsiConsole.Write(new Markup($"Enter instruction number {counter} or [red]x[/] to go to the next step:"));
                input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                    throw new InvalidOperationException("Cant be empty");
                if (input == "x") break;
                recipe.Instructions.Add(input);
            }
            var listRequest = Client.GetStringAsync($"{Config["BaseAddress"]}/api/list-categories");
            var listResponse = listRequest.Result;
            if (listResponse is not null)
            {
                var result = JsonSerializer.Deserialize<List<string>>(listResponse, options);
                Categories.ListCategories(result);
                input = null;
                while (input != "x")
                {
                    AnsiConsole.Write(new Markup($"Enter category number {counter} or [red]x[/] to go to the next step:"));
                    input = Console.ReadLine();
                    if (string.IsNullOrEmpty(input))
                        throw new InvalidOperationException("Cant be empty");
                    if (input == "x") break;
                    recipe.Categories.Add(result[int.Parse(input) - 1]);
                }
            }
            input = null;
            var jsonRecipe = JsonSerializer.Serialize(recipe);
            var content = new StringContent(jsonRecipe, Encoding.UTF8, "application/json");
            var request = Client.PostAsync($"{Config["BaseAddress"]}/api/add-recipe/{jsonRecipe}", content);
            var response = request.Result;
            if (request.IsCompletedSuccessfully)
                AnsiConsole.Write(new Markup("[green]Done !![/]\n\n"));
        }
        public static void ListCategories()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase
            };
            var listRequest = Client.GetStringAsync($"{Config["BaseAddress"]}/api/list-categories");
            var listResponse = listRequest.Result;
            if (listResponse is not null)
            {
                var result = JsonSerializer.Deserialize<List<string>>(listResponse, options);
                Categories.ListCategories(result);
            }
        }
        public static void ListRecipesWeb()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase
            };
            var listRequest = Client.GetStringAsync($"{Config["BaseAddress"]}/api/list-recipes");
            var listResponse = listRequest.Result;
            if (listResponse is not null)
            {
                var result = JsonSerializer.Deserialize<List<Recipe>>(listResponse, options);
                DataHandler.ListRecipesUi(result);
            }
        }
        public static void EditRecipes()
        {
            bool deleted = false;
            ListRecipesWeb();
            AnsiConsole.Write(new Markup("Enter the number of recipe you want to [green]edit:[/]"));
            int recipeNumber = int.Parse(Console.ReadLine()) - 1;
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase
            };
            var listRequest = Client.GetStringAsync($"{Config["BaseAddress"]}/api/list-recipes");
            var listResponse = listRequest.Result;
            if (listResponse is not null)
            {
                var result = JsonSerializer.Deserialize<List<Recipe>>(listResponse, options);
                Recipe oldRecipe = result[recipeNumber];
                var userInput = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("What do you want to [green]EDIT[/]?")
            .PageSize(7)
            .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
            .AddChoices(new[] {
                                "Title", "Ingredients", "Instructions",
                                "Categories", "[red]Delete Recipe[/]"
            }));
                switch (userInput)
                {
                    case "Title":
                        AnsiConsole.Write(new Markup("Enter new [green]title:[/]"));
                        oldRecipe.Title = Console.ReadLine();
                        if (string.IsNullOrEmpty(oldRecipe.Title))
                            throw new InvalidOperationException("Cant be empty");
                        break;
                    case "Ingredients":
                        counter = 1;
                        foreach (var ingerdient in oldRecipe.Ingredients)
                        {
                            Console.WriteLine($"{counter}-{ingerdient} \n");
                            counter++;
                        }
                        AnsiConsole.Write(new Markup("Enter the number you want to [green]edit[/] or enter [green]add[/] for new ingerdient:"));
                        var input = Console.ReadLine();
                        if (string.IsNullOrEmpty(input))
                            throw new InvalidOperationException("Cant be empty");
                        if (input.Equals("add"))
                        {
                            AnsiConsole.Write(new Markup("Enter new [green]ingerdient:[/]"));
                            string newData = Console.ReadLine();
                            if (string.IsNullOrEmpty(newData))
                                throw new InvalidOperationException("Cant be empty");
                            oldRecipe.Ingredients.Add(newData);
                        }
                        else
                        {
                            var oldIngerdient = oldRecipe.Ingredients[int.Parse(input) - 1];
                            AnsiConsole.Write(new Markup("Enter new name or [red]x to delete it:[/]"));
                            string newData = Console.ReadLine();
                            if (string.IsNullOrEmpty(newData))
                                throw new InvalidOperationException("Cant be empty");
                            if (newData.Equals("x"))
                                oldRecipe.Ingredients.Remove(oldIngerdient);
                            else
                                oldRecipe.Ingredients[int.Parse(input) - 1] = newData;
                        }
                        break;
                    case "Instructions":
                        counter = 1;
                        foreach (var instruction in oldRecipe.Instructions)
                        {
                            Console.WriteLine($"{counter}-{instruction} \n");
                            counter++;
                        }
                        AnsiConsole.Write(new Markup("Enter the number you want to [green]edit[/] or enter [green]add[/] for new instruction:"));
                        input = Console.ReadLine();
                        if (input.Equals("add"))
                        {
                            AnsiConsole.Write(new Markup("Enter new [green]instruction:[/]"));
                            string newData = Console.ReadLine();
                            if (string.IsNullOrEmpty(newData))
                                throw new InvalidOperationException("Cant be empty");
                            oldRecipe.Instructions.Add(newData);
                        }
                        else
                        {
                            var oldInstruction = oldRecipe.Instructions[int.Parse(input) - 1];
                            AnsiConsole.Write(new Markup("Enter new [green]name[/] or [red]x to delete it[/]:"));
                            string newData = Console.ReadLine();
                            if (string.IsNullOrEmpty(newData))
                                throw new InvalidOperationException("Cant be empty");
                            if (newData.Equals("x"))
                                oldRecipe.Instructions.Remove(oldInstruction);
                            else
                                oldRecipe.Instructions[int.Parse(input) - 1] = newData;
                        }
                        break;
                    case "Categories":
                        counter = 1;
                        foreach (var category in oldRecipe.Categories)
                        {
                            Console.WriteLine($"{counter}-{category} \n");
                            counter++;
                        }
                        AnsiConsole.Write(new Markup("Enter the [green]number[/] you want to [red]delete[/] or enter add for new category:"));
                        input = Console.ReadLine();
                        if (input.Equals("add"))
                        {
                            ListCategories();
                            AnsiConsole.Write(new Markup("Enter [green]category[/] number from list:"));
                            string newData = Console.ReadLine();
                            oldRecipe.Categories.Add(Categories.CategoriesNames[int.Parse(newData) - 1]);
                        }
                        else
                        {
                            string oldCategory = oldRecipe.Categories[int.Parse(input) - 1];
                            oldRecipe.Categories.Remove(oldCategory);
                        }
                        break;
                    case "[red]Delete Recipe[/]":
                        var deleteRequest = Client.DeleteAsync($"{Config["BaseAddress"]}/api/delete-recipe/{oldRecipe.Id}");
                        var deleteResponse = deleteRequest.Result;
                        if (deleteRequest.IsCompletedSuccessfully)
                        {
                            AnsiConsole.Write(new Markup("[green]Done !![/]\n\n"));
                            deleted = true;
                        }
                        break;
                    default:
                        Console.WriteLine("Wrong Choice");
                        break;
                }
                if (!deleted)
                {
                    var jsonRecipe = JsonSerializer.Serialize(oldRecipe);
                    var content = new StringContent(jsonRecipe, Encoding.UTF8, "application/json");
                    var request = Client.PutAsync($"{Config["BaseAddress"]}/api/update-recipe/{jsonRecipe}/{oldRecipe.Id}", content);
                    var response = request.Result;
                    if (request.IsCompletedSuccessfully)
                        AnsiConsole.Write(new Markup("[green]Done ![/]\n\n"));
                }
            }
        }
    }
}