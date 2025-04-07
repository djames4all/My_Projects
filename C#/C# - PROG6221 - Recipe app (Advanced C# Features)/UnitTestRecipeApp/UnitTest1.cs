using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using NUnit.Framework;
using RecipeApp;

namespace RecipeApp
//ST10393280
{
    public class RecipeCaloriesTests
    {

        [Test]
        // Testing the dispaly of the recipe app
        public void TestRecipeDisplay()
        {
            // Arrange
            Recipe recipe = new Recipe("Test Recipe");
            recipe.AddIngredient("Ingredient 1", 100, "grams", 50, "Protein");
            recipe.AddIngredient("Ingredient 2", 200, "ml", 150, "Carbohydrates");
            recipe.AddStep("Step 1: Mix ingredients");
            recipe.AddStep("Step 2: Bake for 30 minutes");

            // Act
            string expectedOutput = $"Recipe: Test Recipe\n100 grams of Ingredient 1 (50 calories, Protein)\n200 ml of Ingredient 2 (150 calories, Carbohydrates)\n\nTotal calories: 200\nSteps:\n1. Step 1: Mix ingredients\n2. Step 2: Bake for 30 minutes\nThis is a moderate-calorie recipe, suitable for balanced diets.";

            // Assert
            Assert.AreEqual(expectedOutput, GetConsoleOutput(recipe.DisplayRecipe));
        }

        [Test]
        // Testing the Calculation of Total Calories
        public void TestRecipeTotalCaloriesCalculation()
        {
            // Arrange
            Recipe recipe = new Recipe("Test Recipe");
            recipe.AddIngredient("Ingredient 1", 100, "grams", 50, "Protein");
            recipe.AddIngredient("Ingredient 2", 200, "ml", 150, "Carbohydrates");

            // Act
            int totalCalories = recipe.CalculateTotalCalories();

            // Assert
            Assert.AreEqual(200, totalCalories);
        }

        [Test]
        //Testing the notification for Total Calories Exceedance
        public void TestCalorieExceedanceNotification()
        {
            // Arrange
            Recipe recipe = new Recipe("High Calorie Recipe");
            recipe.OnCaloriesExceeded += Console.WriteLine;
            recipe.AddIngredient("Ingredient 1", 200, "grams", 250, "Fats");

            // Act & Assert
            Assert.Throws<CalorieExceedanceException>(() => recipe.DisplayRecipe());
        }
    }
}
