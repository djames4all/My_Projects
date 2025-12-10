using System;
using System.Collections.Generic;
using System.Linq;

namespace RecipeApp
    //ST10393280
{
    // User is notifed when calories exceed 300
    public delegate void CalorieNotification(string message);

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\x1b[32mWelcome to Sanele's Recipe App: \x1b[0m");

            // A list to store all the recipes
            List<Recipe> recipes = new List<Recipe>();

            while (true)
            {
                // Display menu options
                DisplayMenu();
                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    DisplayAllRecipes(recipes);
                }
                else if (choice == "2")
                {
                    CreateNewRecipe(recipes);
                }
                else if (choice == "3")
                {
                    break; 
                }
                else
                {
                    Console.WriteLine("\x1b[31mInvalid choice. Please select either 1, 2, or 3.\x1b[0m");
                }
            }

            Console.WriteLine("\n\x1b[32mThank you for using Recipe App!\x1b[0m");
        }

        // Method created to display the menu
        private static void DisplayMenu()
        {
            Console.WriteLine("\n\x1b[96mMenu:\x1b[0m");
            Console.WriteLine("1. Display all recipes");
            Console.WriteLine("2. Create a new recipe");
            Console.WriteLine("3. Exit");
            Console.Write("Please choose an one of the options (1, 2, or 3): ");
        }

        // Method created to display all recipes
        private static void DisplayAllRecipes(List<Recipe> recipes)
        {
            Console.WriteLine("\n\x1b[32mAll Recipes:\x1b[0m");
            if (recipes.Count == 0)
            {
                Console.WriteLine("No recipes are available.");
            }
            else
            {
                recipes = recipes.OrderBy(r => r.Name).ToList();
                for (int i = 0; i < recipes.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {recipes[i].Name}");
                }

                Console.Write("\nSelect a recipe to display (number): ");
                int recipeIndex;
                while (!int.TryParse(Console.ReadLine(), out recipeIndex) || recipeIndex <= 0 || recipeIndex > recipes.Count)
                {
                    Console.WriteLine("\x1b[31mInvalid input. Please enter a valid recipe number.\x1b[0m");
                }

                // Displays the selected recipe
                recipes[recipeIndex - 1].DisplayRecipe();
            }
        }

        // Method created for a new recipe
        private static void CreateNewRecipe(List<Recipe> recipes)
        {
            Console.WriteLine("\n\x1b[96mPlease enter the details for a new recipe:\x1b[0m");

            // Retrieves the recipe name
            Console.Write("Recipe name: ");
            string recipeName = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(recipeName))
            {
                Console.WriteLine("\x1b[31mRecipe name cannot be empty. Please enter a valid name.\x1b[0m");
                Console.Write("Recipe name: ");
                recipeName = Console.ReadLine();
            }

            // Creates an object for new recipes
            Recipe recipe = new Recipe(recipeName);

            // Retrieves the number of ingredients
            int ingredientCount;
            while (true)
            {
                Console.Write("Number of ingredients: ");
                string input = Console.ReadLine();
                if (int.TryParse(input, out ingredientCount) && ingredientCount > 0)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("\x1b[31mInvalid input. Please enter a positive integer for the number of ingredients.\x1b[0m");
                }
            }

            // Retrieves the details for each ingredient
            for (int i = 0; i < ingredientCount; i++)
            {
                Console.WriteLine($"\n\x1b[33mIngredient {i + 1}:\x1b[0m");

                Console.Write("Name: ");
                string name = Console.ReadLine();
                while (string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine("\x1b[31mThe ingredient name cannot be empty. Please enter a valid name.\x1b[0m");
                    Console.Write("Name: ");
                    name = Console.ReadLine();
                }

                Console.Write("Quantity: ");
                double quantity;
                while (!double.TryParse(Console.ReadLine(), out quantity) || quantity <= 0)
                {
                    Console.WriteLine("\x1b[31mThe quantity must be a positive number. Please enter again.\x1b[0m");
                    Console.Write("Quantity: ");
                }

                Console.Write("Unit of measurement: ");
                string unit = Console.ReadLine();
                while (string.IsNullOrWhiteSpace(unit))
                {
                    Console.WriteLine("\x1b[31mUnit of measurement cannot be empty. Please enter a valid unit.\x1b[0m");
                    Console.Write("Unit of measurement: ");
                    unit = Console.ReadLine();
                }

                Console.Write("Calories: ");
                int calories;
                while (!int.TryParse(Console.ReadLine(), out calories) || calories < 0)
                {
                    Console.WriteLine("\x1b[31mThe calories must be a non-negative number. Please enter again.\x1b[0m");
                    Console.Write("Calories: ");
                }

                // Displays the food group options
                Console.WriteLine("Food group options: 1. Protein 2. Carbohydrates 3. Fats 4. Vitamins 5. Minerals 6. Water");
                Console.Write("Select Food group (1-6): ");
                string foodGroup = GetFoodGroupFromUser();

                recipe.AddIngredient(name, quantity, unit, calories, foodGroup);
            }

            // Retrieves the number of steps
            int stepCount;
            while (true)
            {
                Console.Write("\nNumber of steps: ");
                string stepInput = Console.ReadLine();
                if (int.TryParse(stepInput, out stepCount) && stepCount > 0)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("\x1b[31mInvalid input. Please enter a positive integer for the number of steps.\x1b[0m");
                }
            }

            // Retrieves the details of each step
            for (int i = 0; i < stepCount; i++)
            {
                Console.WriteLine($"\n\x1b[33mStep {i + 1}:\x1b[0m");
                Console.Write("Description: ");
                string description = Console.ReadLine();
                while (string.IsNullOrWhiteSpace(description))
                {
                    Console.WriteLine("\x1b[31mStep description cannot be empty. Please enter a valid description.\x1b[0m");
                    Console.Write("Description: ");
                    description = Console.ReadLine();
                }

                recipe.AddStep(description);
            }

            recipes.Add(recipe);

            Console.WriteLine("\x1b[32mRecipe added successfully!\x1b[0m");
        }

        // The helper method to retrieve the food group from the user
        private static string GetFoodGroupFromUser()
        {
            while (true)
            {
                string input = Console.ReadLine();
                switch (input)
                {
                    case "1": return "Protein";
                    case "2": return "Carbohydrates";
                    case "3": return "Fats";
                    case "4": return "Vitamins";
                    case "5": return "Minerals";
                    case "6": return "Water";
                    default:
                        Console.WriteLine("\x1b[31mInvalid choice. Please select a valid food group (1-6).\x1b[0m");
                        break;
                }
            }
        }
    }

    // Created a class for recipe
    class Recipe
    {
        // Properties of recipe
        public string Name { get; private set; }
        private List<Ingredient> Ingredients { get; set; }
        private List<string> Steps { get; set; }
        public event CalorieNotification OnCaloriesExceeded;

        // Constructor initialized
        public Recipe(string name)
        {
            Name = name;
            Ingredients = new List<Ingredient>();
            Steps = new List<string>();
            OnCaloriesExceeded += NotifyCalorieExceedance;
        }

        // Method created to add an ingredient to the recipe
        public void AddIngredient(string name, double quantity, string unit, int calories, string foodGroup)
        {
            Ingredients.Add(new Ingredient(name, quantity, unit, calories, foodGroup));
        }

        // Method created to add a step to the recipe
        public void AddStep(string description)
        {
            Steps.Add(description);
        }

        // Method created to display the recipe details
        public void DisplayRecipe()
        {
            Console.WriteLine($"\n\x1b[32mRecipe: {Name}\x1b[0m");
            int totalCalories = 0;

            // Displays each ingredient and calculates total calories
            foreach (var ingredient in Ingredients)
            {
                Console.WriteLine($"{ingredient.Quantity} {ingredient.Unit} of {ingredient.Name} ({ingredient.Calories} calories, {ingredient.FoodGroup})");
                totalCalories += ingredient.Calories;
            }

            Console.WriteLine($"\nTotal calories: {totalCalories}");
            if (totalCalories > 300)
            {
                OnCaloriesExceeded?.Invoke("Warning: This recipe exceeds 300 calories! It's high in energy and might not be suitable for low-calorie diets.");
            }

            Console.WriteLine("\n\x1b[32mSteps:\x1b[0m");
            for (int i = 0; i < Steps.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {Steps[i]}");
            }

            DisplayCalorieExplanation(totalCalories);
        }

        private void NotifyCalorieExceedance(string message)
        {
            Console.WriteLine($"\x1b[31m{message}\x1b[0m");
        }

        // Method created to display explanation of calorie ranges
        private void DisplayCalorieExplanation(int totalCalories)
        {
            if (totalCalories < 100)
            {
                Console.WriteLine("\x1b[32mThis is a low-calorie recipe, suitable for weight loss diets.\x1b[0m");
            }
            else if (totalCalories < 300)
            {
                Console.WriteLine("\x1b[33mThis is a moderate-calorie recipe, suitable for balanced diets.\x1b[0m");
            }
            else
            {
                Console.WriteLine("\x1b[31mThis is a high-calorie recipe, suitable for those needing high energy intake.\x1b[0m");
            }
        }
    }

    // Class created for the ingredients
    class Ingredient
    {
        // Properties of the Ingredient
        public string Name { get; private set; }
        public double Quantity { get; private set; }
        public string Unit { get; private set; }
        public int Calories { get; private set; }
        public string FoodGroup { get; private set; }

        // Constructor initialized
        public Ingredient(string name, double quantity, string unit, int calories, string foodGroup)
        {
            Name = name;
            Quantity = quantity;
            Unit = unit;
            Calories = calories;
            FoodGroup = foodGroup;
        }
    }
}
