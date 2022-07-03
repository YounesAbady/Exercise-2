using exercise_1.Models;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace exercise_1
{
    internal class DataHandler
    {
        static int counter;
        public static void ListRecipes(List<Recipe> recipes)
        {
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
        //    public static void EditRecipe()
        //    {
        //        ListRecipes();
        //        Console.WriteLine("Enter the number of recipe you want to edit");
        //        int recipeNumber = int.Parse(Console.ReadLine()) - 1;
        //        Recipe oldRecipe = _Recipes[recipeNumber];
        //        var userInput = AnsiConsole.Prompt(
        //new SelectionPrompt<string>()
        //    .Title("What do you want to [green]EDIT[/]?")
        //    .PageSize(7)
        //    .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
        //    .AddChoices(new[] {
        //                "Title", "Ingredients", "Instructions",
        //                "categories", "[red]Delete Recipe[/]"
        //    }));
        //        switch (userInput)
        //        {
        //            case "Title":
        //                Console.WriteLine("Enter New Title");
        //                oldRecipe.Title = Console.ReadLine();
        //                break;
        //            case "Ingredients":
        //                counter = 1;
        //                foreach (var ingerdient in oldRecipe.Ingredients)
        //                {
        //                    Console.WriteLine($"{counter}-{ingerdient} \n");
        //                    counter++;
        //                }
        //                Console.WriteLine("Enter the number you want to edit or enter add for new ingerdient");
        //                var input = Console.ReadLine();
        //                if (input.Equals("add"))
        //                {
        //                    Console.WriteLine("Enter new ingerdient");
        //                    string newData = Console.ReadLine();
        //                    oldRecipe.Ingredients.Add(newData);
        //                }
        //                else
        //                {
        //                    var oldIngerdient = oldRecipe.Ingredients[int.Parse(input) - 1];
        //                    Console.WriteLine("Enter new name or x to delete it");
        //                    string newData = Console.ReadLine();
        //                    if (newData.Equals("x"))
        //                        oldRecipe.Ingredients.Remove(oldIngerdient);
        //                    else
        //                        oldRecipe.Ingredients[int.Parse(input) - 1] = newData;
        //                }
        //                break;
        //            case "Instructions":
        //                counter = 1;
        //                foreach (var instruction in oldRecipe.Instructions)
        //                {
        //                    Console.WriteLine($"{counter}-{instruction} \n");
        //                    counter++;
        //                }
        //                Console.WriteLine("Enter the number you want to edit or enter add for new instruction");
        //                input = Console.ReadLine();
        //                if (input.Equals("add"))
        //                {
        //                    Console.WriteLine("Enter new instruction");
        //                    string newData = Console.ReadLine();
        //                    oldRecipe.Instructions.Add(newData);
        //                }
        //                else
        //                {
        //                    var oldInstruction = oldRecipe.Instructions[int.Parse(input) - 1];
        //                    Console.WriteLine("Enter new name or x to delete it");
        //                    string newData = Console.ReadLine();
        //                    if (newData.Equals("x"))
        //                        oldRecipe.Instructions.Remove(oldInstruction);
        //                    else
        //                        oldRecipe.Instructions[int.Parse(input) - 1] = newData;
        //                }
        //                break;
        //            case "categories":
        //                counter = 1;
        //                foreach (var category in oldRecipe.Categories)
        //                {
        //                    Console.WriteLine($"{counter}-{category} \n");
        //                    counter++;
        //                }
        //                Console.WriteLine("Enter the number you want to delete or enter add for new category");
        //                input = Console.ReadLine();
        //                if (input.Equals("add"))
        //                {
        //                    Categories.ListCategories();
        //                    Console.WriteLine("Enter Category number from list");
        //                    string newData = Console.ReadLine();
        //                    oldRecipe.Categories.Add(Categories.CategoriesNames[int.Parse(newData) - 1]);
        //                }
        //                else
        //                {
        //                    string oldCategory = oldRecipe.Categories[int.Parse(input) - 1];
        //                    oldRecipe.Categories.Remove(oldCategory);
        //                }
        //                break;
        //            case "Delete Recipe":
        //                _Recipes.Remove(oldRecipe);
        //                break;
        //            default:
        //                Console.WriteLine("Wrong Choice");
        //                break;
        //        }
        //        _Recipes[recipeNumber] = oldRecipe;
        //    }
    }
}
