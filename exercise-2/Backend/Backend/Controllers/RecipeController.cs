﻿using Backend.Models;
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
            //string fileName = @"C:\Users\youne\source\repos\exercise-1\exercise-1\Recipes.json";
            string startupPath = Environment.CurrentDirectory;
            string fileName = @$"{startupPath}\Categories.json";
            string jsonString = JsonSerializer.Serialize(_CategoriesNames);
            File.WriteAllText(fileName, jsonString);
        }
    }
}