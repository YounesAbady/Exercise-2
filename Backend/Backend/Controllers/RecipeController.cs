using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Backend.Controllers
{
    public class RecipeController
    {
        private static List<Recipe> _Recipes { get; set; } = new List<Recipe>();
        private static List<string> _CategoriesNames { get; set; } = new List<string>();
        public RecipeController()
        {
            string startupPath = Environment.CurrentDirectory;
            string fileName = @$"{startupPath}\Recipes.json";
            string jsonString = File.ReadAllText(fileName);
            _Recipes = JsonSerializer.Deserialize<List<Recipe>>(jsonString);
            fileName = @$"{startupPath}\Categories.json";
            jsonString = File.ReadAllText(fileName);
            _CategoriesNames = JsonSerializer.Deserialize<List<string>>(jsonString);
        }
        [HttpGet]
        [Route("api/list-recipes")]
        public List<Recipe> ListRecipes()
        {
            return _Recipes;
        }
        [HttpGet]
        [Route("api/list-categories")]
        public List<string> ListCategories()
        {
            return _CategoriesNames;
        }
        [HttpPost]
        [Route("api/add-category/{category}")]
        public void AddCategory(string category)
        {
            _CategoriesNames.Add(category);
            string startupPath = Environment.CurrentDirectory;
            string fileName = @$"{startupPath}\Categories.json";
            string jsonString = JsonSerializer.Serialize(_CategoriesNames);
            File.WriteAllText(fileName, jsonString);
        }
        [HttpPost]
        [Route("api/add-recipe/{jsonRecipe}")]
        public void AddRecipe(string jsonRecipe)
        {
            Recipe recipe=JsonSerializer.Deserialize<Recipe>(jsonRecipe);
            _Recipes.Add(recipe);
            string startupPath = Environment.CurrentDirectory;
            string fileName = @$"{startupPath}\Recipes.json";
            string jsonString = JsonSerializer.Serialize(_Recipes);
            File.WriteAllText(fileName, jsonString);
        }
        [HttpDelete]
        [Route("api/delete-category/{category}")]
        public void DeleteCategory(string category)
        {
            _CategoriesNames.Remove(category);
            foreach (Recipe recipe in _Recipes) 
            {
                if (recipe.Categories.Contains(category))
                    recipe.Categories.Remove(category);
            }
            string startupPath = Environment.CurrentDirectory;
            string fileName = @$"{startupPath}\Categories.json";
            string jsonString = JsonSerializer.Serialize(_CategoriesNames);
            File.WriteAllText(fileName, jsonString);
            fileName = @$"{startupPath}\Recipes.json";
            jsonString = JsonSerializer.Serialize(_Recipes);
            File.WriteAllText(fileName, jsonString);
        }
        [HttpPut]
        [Route("api/update-category/{position}/{newCategory}")]
        public void UpdateCategory(string position,string newCategory)
        {
            foreach (Recipe recipe in _Recipes) 
            {
                if (recipe.Categories.Contains(_CategoriesNames[int.Parse(position) - 1]))
                {
                    recipe.Categories[recipe.Categories.IndexOf(_CategoriesNames[int.Parse(position) - 1])] = newCategory;
                }
            }
            _CategoriesNames[int.Parse(position)-1] =newCategory;

            string startupPath = Environment.CurrentDirectory;
            string fileName = @$"{startupPath}\Categories.json";
            string jsonString = JsonSerializer.Serialize(_CategoriesNames);
            File.WriteAllText(fileName, jsonString);
            fileName = @$"{startupPath}\Recipes.json";
            jsonString = JsonSerializer.Serialize(_Recipes);
            File.WriteAllText(fileName, jsonString);
        }
    }
}
