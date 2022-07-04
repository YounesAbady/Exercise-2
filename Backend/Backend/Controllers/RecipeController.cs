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
            startupPath = Environment.CurrentDirectory;
            fileName = @$"{startupPath}\Categories.json";
            jsonString = File.ReadAllText(fileName);
            _CategoriesNames = JsonSerializer.Deserialize<List<string>>(jsonString);
        }
        [HttpGet]
        [Route("api/ListRecipes")]
        public List<Recipe> ListRecipes()
        {
            return _Recipes;
        }
        [HttpGet]
        [Route("api/ListCategories")]
        public List<string> ListCategories()
        {
            return _CategoriesNames;
        }
        [HttpPost]
        [Route("api/AddCategory/{category}")]
        public void AddCategory(string category)
        {
            _CategoriesNames.Add(category);
            string startupPath = Environment.CurrentDirectory;
            string fileName = @$"{startupPath}\Categories.json";
            string jsonString = JsonSerializer.Serialize(_CategoriesNames);
            File.WriteAllText(fileName, jsonString);
        }
        [HttpPost]
        [Route("api/AddRecipe/{jsonRecipe}")]
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
        [Route("api/DeleteCategory/{category}")]
        public void DeleteCategory(string category)
        {
            _CategoriesNames.Remove(category);
            string startupPath = Environment.CurrentDirectory;
            string fileName = @$"{startupPath}\Categories.json";
            string jsonString = JsonSerializer.Serialize(_CategoriesNames);
            File.WriteAllText(fileName, jsonString);
        }
        [HttpPut]
        [Route("api/UpdateCategory/{position}/{newCategory}")]
        public void UpdateCategory(string position,string newCategory)
        {
            _CategoriesNames[int.Parse(position)-1] =newCategory;
            string startupPath = Environment.CurrentDirectory;
            string fileName = @$"{startupPath}\Categories.json";
            string jsonString = JsonSerializer.Serialize(_CategoriesNames);
            File.WriteAllText(fileName, jsonString);
        }
    }
}
