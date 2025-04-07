using System;

namespace ROG_POE_Part1
    //ST10393280 - Daniel James

    /* ANSI escape codes used for text colour:
       \x1b[32m is for green,  
       \x1b[96m is for cyan,
       \x1b[33m is for yellow,
       \x1b[31m is for red,
       \x1b[0m: resets the text color to default */

        {
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\x1b[32mWelcome to Sanele's Recipe App: \x1b[0m");

            // Creating an object for a new recipe
            Recipe recipe = new Recipe(); 

            // While loop 
            while (true) 
            {
                Console.WriteLine("\n\x1b[96mPlease enter the details for a new recipe:\x1b[0m");

                // Prompts the user to enter number of ingredients
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
                        // Error validation implemented for number of ingredients 
                        Console.WriteLine("\x1b[31mInvalid input. Please enter a positive integer for the number of ingredients.\x1b[0m");
                    }
                }

                // Prompts the user to enter details of the ingredients
                for (int i = 0; i < ingredientCount; i++)
                {
                    Console.WriteLine($"\n\x1b[33mIngredient {i + 1}:\x1b[0m");
                    Console.Write("Name: ");
                    string name = Console.ReadLine();
                    Console.Write("Quantity: ");
                    double quantity;

                    while (!double.TryParse(Console.ReadLine(), out quantity) || quantity <= 0)
                    {
                        // Error validation implemented for quantity
                        Console.WriteLine("\x1b[31mThe quantity must be a positive number. Please enter again.\x1b[0m"); 
                        Console.Write("Quantity: ");
                    }
                    Console.Write("Unit of measurement: ");
                    string unit = Console.ReadLine();

                    recipe.AddIngredient(name, quantity, unit); 
                }

                // Prompts the user to enter the number of steps for the recipe 
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
                        // Error validation implemented for number of steps
                        Console.WriteLine("\x1b[31mInvalid input. Please enter a positive integer for the number of steps.\x1b[0m"); 
                    }
                }

                // Implementation of a for loop for the recipe's steps and details
                for (int i = 0; i < stepCount; i++)
                {
                    Console.WriteLine($"\n\x1b[33mStep {i + 1}:\x1b[0m");
                    Console.Write("Description: ");
                    string description = Console.ReadLine();

                    recipe.AddStep(description); 
                }

                // Output of the full recipe
                recipe.DisplayRecipe();

                // Prompts the user to scale the recipe
                Console.WriteLine("\n\x1b[96mDo you want to scale the recipe? (Y/N)\x1b[0m"); 
                char scaleChoice = char.ToUpper(Console.ReadKey().KeyChar);
                Console.WriteLine();
                switch (scaleChoice)
                {
                    case 'Y':
                        recipe.ScaleRecipe();

                        // Prompts the user to reset only if the recipe is scaled
                        Console.WriteLine("\n\x1b[96mDo you want to reset ingredient quantities to original values? (Y/N)\x1b[0m");
                        char resetChoice = char.ToUpper(Console.ReadKey().KeyChar);
                        Console.WriteLine();

                        // Implementation of switch - case
                        switch (resetChoice)
                        {
                            case 'Y':
                                Console.WriteLine("\x1b[96mAre you sure you want to reset ingredient quantities to original values? (Y/N)\x1b[0m"); 
                                char confirmResetChoice = char.ToUpper(Console.ReadKey().KeyChar);
                                Console.WriteLine();
                                if (confirmResetChoice == 'Y')
                                {
                                    recipe.ResetQuantities(); 
                                }
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }

                // Prompt the user to enter new recipe or exit the application
                Console.WriteLine("\n\x1b[96mDo you want to enter a new recipe? (Y/N)\x1b[0m");
                char continueChoice = char.ToUpper(Console.ReadKey().KeyChar);
                Console.WriteLine();
                if (continueChoice == 'N')
                    break;

                recipe.Clear(); 
            }

            Console.WriteLine("\n\x1b[32mThank you for using Recipe App!\x1b[0m"); 
        }
    }

    // Created a class for recipe as an array
    class Recipe
    {
        private string[] ingredients;
        private double[] originalQuantities;
        private double[] scaledQuantities;
        private string[] units;
        private string[] steps;

        public Recipe()
        {
            ingredients = new string[0];
            originalQuantities = new double[0];
            scaledQuantities = new double[0];
            units = new string[0];
            steps = new string[0];
        }

        // Adding ingredients to the recipe
        public void AddIngredient(string name, double quantity, string unit)
        {
            Array.Resize(ref ingredients, ingredients.Length + 1);
            Array.Resize(ref originalQuantities, originalQuantities.Length + 1);
            Array.Resize(ref scaledQuantities, scaledQuantities.Length + 1);
            Array.Resize(ref units, units.Length + 1);

            // Initiates the scaled quantities with the original values from the recipe
            int index = ingredients.Length - 1;
            ingredients[index] = name;
            originalQuantities[index] = quantity;
            scaledQuantities[index] = quantity; 
            units[index] = unit;
        }

        public void AddStep(string description)
        {
            Array.Resize(ref steps, steps.Length + 1);

            int index = steps.Length - 1;
            steps[index] = description;
        }

        // Outputs the full recipe
        public void DisplayRecipe()
        {
            Console.WriteLine("\n\x1b[32mRecipe:\x1b[0m"); 
            for (int i = 0; i < ingredients.Length; i++)
            {
                Console.WriteLine($"{scaledQuantities[i]} {units[i]} of {ingredients[i]}");
            }
            Console.WriteLine("\n\x1b[32mSteps:\x1b[0m"); 
            for (int i = 0; i < steps.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {steps[i]}");
            }
        }

        // Scales the recipe by a factor
        public void ScaleRecipe()
        {
            /* When I run the code on the VM Visual Studio 2022, the program works by entering "0.5"
               When I run the exact same code on my local machine Visual Studio 2022, the program works by entering "0,5"
               Entering 2 or 3 works on both VM and local machine (Only 0.5 / 0,5 is the variation) */
            Console.WriteLine("\n\x1b[96mPlease enter the scaling factor (0.5 for half, 2 for double, or 3 for triple):\x1b[0m"); 
            double factor;

            while (true)
            {
                if (double.TryParse(Console.ReadLine(), out factor))
                {
                    if (factor == 0.5 || factor == 2 || factor == 3)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("\x1b[31mInvalid scaling factor. Please enter 0.5, 2, or 3.\x1b[0m"); 
                    }
                }
                else
                {
                    Console.WriteLine("\x1b[31mInvalid input. Please enter a valid number.\x1b[0m"); 
                }
            }

            for (int i = 0; i < scaledQuantities.Length; i++)
            {
                scaledQuantities[i] = originalQuantities[i] * factor;
            }

            Console.WriteLine("\n\x1b[32mRecipe scaled successfully!\x1b[0m"); 
            DisplayRecipe();
        }

        // Resets the ingredient quantities to the original values
        public void ResetQuantities()
        {
            Console.WriteLine("\n\x1b[32mResetting ingredient quantities to original values...\x1b[0m");
            Array.Copy(originalQuantities, scaledQuantities, originalQuantities.Length);
            DisplayRecipe();
        }

        // Clears all the recipe data
        public void Clear()
        {
            ingredients = new string[0];
            originalQuantities = new double[0];
            scaledQuantities = new double[0];
            units = new string[0];
            steps = new string[0];
        }
    }
}
