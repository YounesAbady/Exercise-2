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
    }
}
