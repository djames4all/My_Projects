using NUnit.Framework;
using System;
using System.Collections.Generic;
using RecipeApp;

namespace RecipeApp.Tests
{
    [TestFixture]
    public class RecipeAppTest
    {
        [Test]
        public void Test_CreateNewRecipe()
        {
            List<Recipe> recipes = new List<Recipe>();
            Program.CreateNewRecipe(recipes);

            Assert.AreEqual(1, recipes.Count);
        }

        [Test]
        public void Test_CalorieCalculation()
        {
            Recipe recipe = new Recipe("Test Recipe");
            recipe.AddIngredient("Ingredient 1", 100, "g", 200, "Protein");
            recipe.AddIngredient("Ingredient 2", 50, "g", 150, "Carbohydrates");
            recipe.AddIngredient("Ingredient 3", 30, "g", 80, "Fats");

            int totalCalories = recipe.CalculateTotalCalories();

            Assert.AreEqual(430, totalCalories);
        }

        [Test]
        public void Test_CalorieExceedanceNotification()
        {
            Recipe recipe = new Recipe("High Calorie Recipe");
            string notificationMessage = "";

            // Subscribe to the event
            recipe.OnCaloriesExceeded += (message) => { notificationMessage = message; };

            // Add ingredients that exceed 300 calories
            recipe.AddIngredient("Ingredient 1", 200, "g", 250, "Protein");
            recipe.AddIngredient("Ingredient 2", 100, "g", 200, "Carbohydrates");

            // Display the recipe
            recipe.DisplayRecipe();

            Assert.AreEqual("Warning: This recipe exceeds 300 calories! It's high in energy and might not be suitable for low-calorie diets.", notificationMessage);
        }
    }
}
