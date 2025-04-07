Welcome to the Recipe App! This document will guide you on how to compile, run, and use the application effectively. 
Additionally, it highlights the changes made from the previous version of the program.


GitHub Link:
https://github.com/VCSTDN/prog6221-poe-ST10393280.git

YouTube Link:
https://youtu.be/uYbhrvhtZGg


How to Compile and Run?
Ensure you have the .NET framework installed on your system.
A compatible IDE, such as Visual Studio, is recommended for ease of development and debugging.

Instructions
Clone the Repository
Open the solution file RecipeApp.sln in Visual Studio.
In Visual Studio, go to Build
After a successful build, run the application by clicking the Start button

How the Program Works?
User Interface
The application features a graphical user interface (GUI) with the following components:

Recipe List and Filters:
Filter recipes by ingredient, food group, or maximum calories.
Apply and clear filters using the respective buttons.
Select a recipe from the list to view details.

Recipe Details and Entry:
Enter details for new recipes, including name, ingredients, and steps.
Save new recipes to the list.

Recipe Steps:
Add steps to a recipe and mark them as completed using checkboxes.

User Interactions:
Filter Recipes: Users can filter recipes based on selected criteria.
Add Ingredients: Users input ingredient details and add them to the current recipe.
Add Steps: Users input steps for recipe preparation.
Save Recipe: Users save the complete recipe, which is then added to the recipe list.
View Recipe: Users can view recipe details by selecting a recipe from the list.

Changes Between the Old and New Program:

Description of Changes:
The new version of the Recipe App introduces a modernized GUI, replacing the old console-based interface. 

Key changes include:
Graphical User Interface (GUI):

The new program utilizes WPF (Windows Presentation Foundation) for a more intuitive and visually appealing interface.
Users interact with the app through buttons, text boxes, combo boxes, and sliders instead of console inputs.

Filtering Functionality:
Enhanced filtering options allow users to filter recipes by ingredients, food groups, and calorie limits dynamically.
A slider control is introduced for setting calorie limits.

Ingredient and Step Management:
The new app provides a structured layout for adding ingredients and steps.
Ingredients and steps are displayed in a user-friendly manner, making the process more intuitive.

Recipe Display and Selection:
Recipes are listed and can be selected from a list box, with details displayed on selection.
A dropdown menu is added for selecting and displaying specific recipes, along with their preparation steps.