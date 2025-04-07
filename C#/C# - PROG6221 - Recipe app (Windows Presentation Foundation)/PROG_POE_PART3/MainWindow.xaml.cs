using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.Generic;
using System.Linq;

namespace PROG_POE_PART3
    // ST10393280 Daniel Luke James
{
    public partial class MainWindow : Window
    {
        // Lists to store recipes, ingredients, and steps
        private List<Recipe> recipes = new List<Recipe>();
        private List<Ingredient> currentIngredients = new List<Ingredient>();
        private List<string> currentSteps = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
            InitializeFoodGroupComboboxes();
            InitializeIngredientFilterCombobox();
            CalorieFilterSlider.ValueChanged += CalorieFilterSlider_ValueChanged;

            // Example of setting background color programmatically
            RecipeDetailsTextBlock.Background = Brushes.LightYellow;
        }

        // Initialize food group comboboxes with predefined values
        private void InitializeFoodGroupComboboxes()
        {
            var foodGroups = new List<string> { "Protein", "Carbohydrates", "Fats", "Vitamins", "Minerals", "Water" };
            IngredientFoodGroupComboBox.ItemsSource = foodGroups;
            FoodGroupFilterComboBox.ItemsSource = foodGroups;
        }

        // Initialize ingredient filter combobox with ingredient names from all recipes
        private void InitializeIngredientFilterCombobox()
        {
            var ingredientNames = recipes.SelectMany(r => r.Ingredients).Select(i => i.Name).Distinct().ToList();
            IngredientFilterComboBox.ItemsSource = ingredientNames;
        }

        // Add a new ingredient to the current recipe
        private void AddIngredient_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(IngredientQuantityTextBox.Text, out double quantity) &&
                int.TryParse(IngredientCaloriesTextBox.Text, out int calories) &&
                !string.IsNullOrWhiteSpace(IngredientNameTextBox.Text) &&
                !string.IsNullOrWhiteSpace(IngredientUnitTextBox.Text) &&
                IngredientFoodGroupComboBox.SelectedItem != null)
            {
                var ingredient = new Ingredient(
                    IngredientNameTextBox.Text,
                    quantity,
                    IngredientUnitTextBox.Text,
                    calories,
                    IngredientFoodGroupComboBox.SelectedItem.ToString());
                currentIngredients.Add(ingredient);
                ClearIngredientFields();
                MessageBox.Show("Ingredient successfully added.");
            }
            else
            {
                MessageBox.Show("Please fill all ingredient fields correctly.");
            }
        }

        // Clear ingredient input fields
        private void ClearIngredientFields()
        {
            IngredientNameTextBox.Clear();
            IngredientQuantityTextBox.Clear();
            IngredientUnitTextBox.Clear();
            IngredientCaloriesTextBox.Clear();
            IngredientFoodGroupComboBox.SelectedIndex = -1;
        }

        // Add a new step to the current recipe
        private void AddStep_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(StepDescriptionTextBox.Text))
            {
                currentSteps.Add(StepDescriptionTextBox.Text);
                StepDescriptionTextBox.Clear();
            }
            else
            {
                MessageBox.Show("Step description cannot be empty.");
            }
        }

        // Save the current recipe
        private void SaveRecipe_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(RecipeNameTextBox.Text) &&
                currentIngredients.Count > 0 &&
                currentSteps.Count > 0)
            {
                var recipe = new Recipe(RecipeNameTextBox.Text);
                foreach (var ingredient in currentIngredients)
                {
                    recipe.AddIngredient(ingredient.Name, ingredient.Quantity, ingredient.Unit, ingredient.Calories, ingredient.FoodGroup);
                }
                foreach (var step in currentSteps)
                {
                    recipe.AddStep(step);
                }
                recipes.Add(recipe);
                ClearRecipeFields();
                RefreshRecipeList();
                MessageBox.Show("Recipe successfully saved.");
            }
            else
            {
                MessageBox.Show("Please enter a recipe name, at least one ingredient, and one step.");
            }
        }

        // Clear recipe input fields and lists
        private void ClearRecipeFields()
        {
            RecipeNameTextBox.Clear();
            currentIngredients.Clear();
            currentSteps.Clear();
        }

        // Refresh the recipe list and comboboxes
        private void RefreshRecipeList()
        {
            RecipeListBox.ItemsSource = null;
            RecipeListBox.ItemsSource = recipes.OrderBy(r => r.Name).ToList();
            InitializeIngredientFilterCombobox(); // Refresh the ingredient filter combobox

            // Order the recipes alphabetically for the SelectRecipeComboBox
            SelectRecipeComboBox.ItemsSource = null;
            SelectRecipeComboBox.ItemsSource = recipes.OrderBy(r => r.Name).Select(r => r.Name).ToList();
        }

        // Apply filters to the recipe list
        private void ApplyFilters_Click(object sender, RoutedEventArgs e)
        {
            var filteredRecipes = recipes.AsEnumerable();

            if (IngredientFilterComboBox.SelectedItem != null)
            {
                filteredRecipes = filteredRecipes.Where(r => r.Ingredients.Any(i => i.Name == IngredientFilterComboBox.SelectedItem.ToString()));
            }

            if (FoodGroupFilterComboBox.SelectedItem != null)
            {
                filteredRecipes = filteredRecipes.Where(r => r.Ingredients.Any(i => i.FoodGroup == FoodGroupFilterComboBox.SelectedItem.ToString()));
            }

            filteredRecipes = filteredRecipes.Where(r => r.Ingredients.Sum(i => i.Calories) <= CalorieFilterSlider.Value);

            RecipeListBox.ItemsSource = filteredRecipes.OrderBy(r => r.Name).ToList();
        }

        // Clear filters and show all recipes
        private void ClearFilters_Click(object sender, RoutedEventArgs e)
        {
            IngredientFilterComboBox.SelectedIndex = -1;
            FoodGroupFilterComboBox.SelectedIndex = -1;
            CalorieFilterSlider.Value = 1000; // Assuming 1000 is the maximum value
            RecipeListBox.ItemsSource = recipes.OrderBy(r => r.Name).ToList();
        }

        // Display selected recipe details
        private void RecipeListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RecipeListBox.SelectedItem is Recipe selectedRecipe)
            {
                RecipeDetailsTextBlock.Text = selectedRecipe.ToString();
            }
        }

        // Update calorie filter label as slider value changes
        private void CalorieFilterSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            CalorieFilterLabel.Text = ((int)e.NewValue).ToString();
        }

        // Display details of the selected recipe from dropdown
        private void SelectRecipeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RecipeDetailsPanel.Visibility = Visibility.Collapsed;
            RecipeStepsPanel.Children.Clear();
            if (SelectRecipeComboBox.SelectedItem is string selectedRecipeName)
            {
                var selectedRecipe = recipes.FirstOrDefault(r => r.Name == selectedRecipeName);
                if (selectedRecipe != null)
                {
                    SelectedRecipeDetailsTextBlock.Text = selectedRecipe.ToString();
                    foreach (var step in selectedRecipe.Steps)
                    {
                        var checkBox = new CheckBox
                        {
                            Content = step,
                            Style = (Style)Resources["StepCheckBoxStyle"]
                        };
                        RecipeStepsPanel.Children.Add(checkBox);
                    }
                    RecipeDetailsPanel.Visibility = Visibility.Visible;
                }
            }
        }
    }

    // Class representing a recipe
    public class Recipe
    {
        public string Name { get; }
        public List<Ingredient> Ingredients { get; }
        public List<string> Steps { get; }

        public Recipe(string name)
        {
            Name = name;
            Ingredients = new List<Ingredient>();
            Steps = new List<string>();
        }

        // Add an ingredient to the recipe
        public void AddIngredient(string name, double quantity, string unit, int calories, string foodGroup)
        {
            Ingredients.Add(new Ingredient(name, quantity, unit, calories, foodGroup));
        }

        // Add a step to the recipe
        public void AddStep(string step)
        {
            Steps.Add(step);
        }

        // Override ToString() to display recipe details
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Recipe: {Name}");
            sb.AppendLine("Ingredients:");
            foreach (var ingredient in Ingredients)
            {
                sb.AppendLine($"- {ingredient}");
            }
            sb.AppendLine("Steps:");
            for (int i = 0; i < Steps.Count; i++)
            {
                sb.AppendLine($"{i + 1}. {Steps[i]}");
            }
            return sb.ToString();
        }
    }

    // Class representing an ingredient
    public class Ingredient
    {
        public string Name { get; }
        public double Quantity { get; }
        public string Unit { get; }
        public int Calories { get; }
        public string FoodGroup { get; }

        public Ingredient(string name, double quantity, string unit, int calories, string foodGroup)
        {
            Name = name;
            Quantity = quantity;
            Unit = unit;
            Calories = calories;
            FoodGroup = foodGroup;
        }

        // Override ToString() to display ingredient details
        public override string ToString()
        {
            return $"{Quantity} {Unit} of {Name} ({Calories} calories, {FoodGroup})";
        }
    }
}
