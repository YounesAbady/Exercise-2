using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class Recipe
    {
        public Recipe()
        {
        }
        public Recipe(string title, List<string> ingredients, List<string> instructions, List<string> categories)
        {
            this.Title = title;
            Ingredients = ingredients;
            Instructions = instructions;
            Categories = categories;
        }
        [JsonProperty("Title")]
        public string Title { get; set; } = string.Empty;
        [JsonProperty("Ingredients")]
        public List<string> Ingredients { get; set; } = new List<string>();
        [JsonProperty("Instructions")]

        public List<string> Instructions { get; set; } = new List<string>();
        [JsonProperty("Categories")]
        public List<string> Categories { get; set; } = new List<string>();
    }
}
