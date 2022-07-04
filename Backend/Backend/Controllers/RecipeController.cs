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
            Recipe recipe = JsonSerializer.Deserialize<Recipe>(jsonRecipe);
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
        public void UpdateCategory(string position, string newCategory)
        {
            foreach (Recipe recipe in _Recipes)
            {
                if (recipe.Categories.Contains(_CategoriesNames[int.Parse(position) - 1]))
                {
                    recipe.Categories[recipe.Categories.IndexOf(_CategoriesNames[int.Parse(position) - 1])] = newCategory;
                }
            }
            _CategoriesNames[int.Parse(position) - 1] = newCategory;
            string startupPath = Environment.CurrentDirectory;
            string fileName = @$"{startupPath}\Categories.json";
            string jsonString = JsonSerializer.Serialize(_CategoriesNames);
            File.WriteAllText(fileName, jsonString);
            fileName = @$"{startupPath}\Recipes.json";
            jsonString = JsonSerializer.Serialize(_Recipes);
            File.WriteAllText(fileName, jsonString);
        }
        [HttpDelete]
        [Route("api/delete-recipe/{id}")]
        public void DeleteRecipe(Guid id)
        {
            Recipe recipe = _Recipes.FirstOrDefault(x => x.Id == id);
            _Recipes.Remove(recipe);
            string startupPath = Environment.CurrentDirectory;
            var fileName = @$"{startupPath}\Recipes.json";
            var jsonString = JsonSerializer.Serialize(_Recipes);
            File.WriteAllText(fileName, jsonString);
        }
        [HttpPut]
        [Route("api/update-recipe/{jsonRecipe}/{id}")]
        public void UpdateRecipe(string jsonRecipe,Guid id)
        {
            Recipe oldRecipe = _Recipes.FirstOrDefault(x => x.Id == id);
            Recipe newRecipe = JsonSerializer.Deserialize<Recipe>(jsonRecipe);
            oldRecipe.Title = newRecipe.Title;
            oldRecipe.Categories = newRecipe.Categories;
            oldRecipe.Ingredients = newRecipe.Ingredients;
            oldRecipe.Instructions = newRecipe.Instructions;
            string startupPath = Environment.CurrentDirectory;
            var fileName = @$"{startupPath}\Recipes.json";
            var jsonString = JsonSerializer.Serialize(_Recipes);
            File.WriteAllText(fileName, jsonString);
        }
        [HttpGet]
        [Route("api/get-recipe/{id}")]
        public Recipe GetRecipe(Guid id) 
        {
            var recipe = _Recipes.FirstOrDefault(x => x.Id == id);
            return recipe;
        }
    }
}
