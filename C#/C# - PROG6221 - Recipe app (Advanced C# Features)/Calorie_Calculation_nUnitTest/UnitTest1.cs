using NUnit.Framework;
using System;
using System.Collections.Generic;
using RecipeApp; // Import the RecipeApp namespace to access the Recipe class

namespace RecipeAppTest
{
    [TestFixture]
    public class RecipeTests
    {
        [Test]
        public void TotalCalories_NoIngredients_ReturnsZero()
        {
            // Arrange
            Recipe recipe = new Recipe("Test Recipe");

            // Act
            int totalCalories = recipe.CalculateTotalCalories();

            // Assert
            Assert.AreEqual(0, totalCalories);
        }

        [Test]
        public void TotalCalories_WithIngredients_ReturnsCorrectSum()
        {
            // Arrange
            Recipe recipe = new Recipe("Test Recipe");
            recipe.AddIngredient("Ingredient 1", 100, "g", 50, "Protein");
            recipe.AddIngredient("Ingredient 2", 200, "ml", 80, "Carbohydrates");

            // Act
            int totalCalories = recipe.CalculateTotalCalories();

            // Assert
            Assert.AreEqual(130, totalCalories);
        }

        [Test]
        public void TotalCalories_Exceeding300TriggersNotification()
        {
            // Arrange
            Recipe recipe = new Recipe("Test Recipe");
            recipe.AddIngredient("Ingredient 1", 150, "g", 200, "Protein");
            recipe.AddIngredient("Ingredient 2", 200, "ml", 150, "Carbohydrates");

            bool notificationReceived = false;
            recipe.OnCaloriesExceeded += (message) => { notificationReceived = true; };

            // Act
            recipe.DisplayRecipe(); // Displaying the recipe will calculate calories and trigger notification if exceeds 300

            // Assert
            Assert.IsTrue(notificationReceived);
        }

        [Test]
        public void TotalCalories_NotExceeding300DoesNotTriggerNotification()
        {
            // Arrange
            Recipe recipe = new Recipe("Test Recipe");
            recipe.AddIngredient("Ingredient 1", 50, "g", 100, "Protein");
            recipe.AddIngredient("Ingredient 2", 100, "ml", 150, "Carbohydrates");

            bool notificationReceived = false;
            recipe.OnCaloriesExceeded += (message) => { notificationReceived = true; };

            // Act
            recipe.DisplayRecipe(); // Displaying the recipe will calculate calories but should not trigger notification

            // Assert
            Assert.IsFalse(notificationReceived);
        }

        // Add more test cases to cover various scenarios (e.g., calorie ranges).
    }
}