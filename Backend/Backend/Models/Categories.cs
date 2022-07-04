using System.Text.Json;
using Spectre.Console;

namespace Backend.Models
{
    public class Categories
    {
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
    }
}

