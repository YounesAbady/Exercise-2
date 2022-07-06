using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class DataHandler
    {
        static int counter;
        private static string _baseAdress = "https://localhost:7018";
        public static void ListRecipesUi(List<Recipe> recipes)
        {
            ArgumentNullException.ThrowIfNull(recipes);
            int recipesCounter = 1;
            var root = new Tree("[lime]Recipes[/]");
            foreach (Recipe recipe in recipes)
            {
                var recipeTitle = root.AddNode($"{recipesCounter}-[maroon]{recipe.Title}[/]");
                counter = 1;
                var ingerdientsNode = recipeTitle.AddNode("[red]Ingredients:[/]");
                foreach (var ingerdient in recipe.Ingredients)
                {
                    ingerdientsNode.AddNode($"{counter}-{ingerdient}");
                    counter++;
                }
                var instructionsNode = recipeTitle.AddNode("[red]Instructions:[/]");
                counter = 1;
                foreach (var instructions in recipe.Instructions)
                {
                    instructionsNode.AddNode($"{counter}-{instructions}");
                    counter++;
                }
                counter = 1;
                var categoriesNode = recipeTitle.AddNode("[red]Categories:[/]");
                foreach (var category in recipe.Categories)
                {
                    categoriesNode.AddNode($"{counter}-{category}");
                    counter++;
                }
                recipesCounter++;
            }
            AnsiConsole.Write(root);
        }
        public static void AddCategory()
        {
            Console.WriteLine("Enter Category name :");
            var category = Console.ReadLine();
            if (string.IsNullOrEmpty(category))
                throw new InvalidOperationException("Cant be empty");
            var client = new HttpClient();
            var jsonCategory = JsonSerializer.Serialize(category);
            var content = new StringContent(jsonCategory, Encoding.UTF8, "application/json");
            var request = client.PostAsync($"{_baseAdress}/api/add-category/{category}", content);
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
            var client = new HttpClient();
            string input = null;
            Recipe recipe = new Recipe();
            Console.WriteLine("Enter recipe name");
            recipe.Title = Console.ReadLine();
            if (string.IsNullOrEmpty(recipe.Title))
                throw new InvalidOperationException("Cant be empty");
            for (counter = 1; input != "x"; counter++)
            {
                Console.WriteLine($"Enter ingredient number {counter} or x to go to the next step");
                input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                    throw new InvalidOperationException("Cant be empty");
                if (input == "x") break;
                recipe.Ingredients.Add(input);
            }
            input = null;
            for (counter = 1; input != "x"; counter++)
            {
                Console.WriteLine($"Enter instruction number {counter} or x to go to the next step");
                input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                    throw new InvalidOperationException("Cant be empty");
                if (input == "x") break;
                recipe.Instructions.Add(input);
            }
            var listRequest = client.GetStringAsync($"{_baseAdress}/api/list-categories");
            var listResponse = listRequest.Result;
            if (listResponse is not null)
            {
                var result = JsonSerializer.Deserialize<List<string>>(listResponse, options);
                Categories.ListCategories(result);
                input = null;
                while (input != "x")
                {
                    Console.WriteLine("Enter Category number from list or x to go to the next step");
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
            var request = client.PostAsync($"{_baseAdress}/api/add-recipe/{jsonRecipe}", content);
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
            var client = new HttpClient();
            var listRequest = client.GetStringAsync($"{_baseAdress}/api/list-categories");
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
            var client = new HttpClient();
            var listRequest = client.GetStringAsync($"{_baseAdress}/api/list-recipes");
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
            Console.WriteLine("Enter the number of recipe you want to edit");
            int recipeNumber = int.Parse(Console.ReadLine()) - 1;
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase
            };
            var client = new HttpClient();
            var listRequest = client.GetStringAsync($"{_baseAdress}/api/list-recipes");
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
                        Console.WriteLine("Enter New Title");
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
                        Console.WriteLine("Enter the number you want to edit or enter add for new ingerdient");
                        var input = Console.ReadLine();
                        if (string.IsNullOrEmpty(input))
                            throw new InvalidOperationException("Cant be empty");
                        if (input.Equals("add"))
                        {
                            Console.WriteLine("Enter new ingerdient");
                            string newData = Console.ReadLine();
                            if (string.IsNullOrEmpty(newData))
                                throw new InvalidOperationException("Cant be empty");
                            oldRecipe.Ingredients.Add(newData);
                        }
                        else
                        {
                            var oldIngerdient = oldRecipe.Ingredients[int.Parse(input) - 1];
                            Console.WriteLine("Enter new name or x to delete it");
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
                        Console.WriteLine("Enter the number you want to edit or enter add for new instruction");
                        input = Console.ReadLine();
                        if (input.Equals("add"))
                        {
                            Console.WriteLine("Enter new instruction");
                            string newData = Console.ReadLine();
                            if (string.IsNullOrEmpty(newData))
                                throw new InvalidOperationException("Cant be empty");
                            oldRecipe.Instructions.Add(newData);
                        }
                        else
                        {
                            var oldInstruction = oldRecipe.Instructions[int.Parse(input) - 1];
                            Console.WriteLine("Enter new name or x to delete it");
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
                        Console.WriteLine("Enter the number you want to delete or enter add for new category");
                        input = Console.ReadLine();
                        if (input.Equals("add"))
                        {
                            ListCategories();
                            Console.WriteLine("Enter Category number from list");
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
                        client = new HttpClient();
                        var deleteRequest = client.DeleteAsync($"{_baseAdress}/api/delete-recipe/{oldRecipe.Id}");
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
                    var request = client.PutAsync($"{_baseAdress}/api/update-recipe/{jsonRecipe}/{oldRecipe.Id}", content);
                    var response = request.Result;
                    if (request.IsCompletedSuccessfully)
                        AnsiConsole.Write(new Markup("[green]Done ![/]\n\n"));
                }
            }
        }
    }
}