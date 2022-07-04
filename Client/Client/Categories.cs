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
    }
}
