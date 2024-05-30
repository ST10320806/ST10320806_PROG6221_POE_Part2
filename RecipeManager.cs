using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10320806_PROG6221_Part2
{
    internal class RecipeManager
    {
        // Dictionary to store recipe name and their details (ingredients and steps)
        private Dictionary<string, (List<string> Ingredients, List<string> Steps)> recipeBook = new Dictionary<string, (List<string> Ingredients, List<string> Steps)>();

        // List to store scaling factors
        public List<int> sFactor { get; set; } = new List<int>();

        public void AddRecipe() // Method for adding multiple recipes
        {
            Console.WriteLine("\n*****************************************\nENTERING RECIPES: ");
            Console.WriteLine("How many recipes would you like to enter?: ");
            int amountRecipes = Convert.ToInt32(Console.ReadLine());

            for (int i = 0; i < amountRecipes; i++)
            {
                Console.WriteLine($"Entering recipe {i + 1}:");
                Console.WriteLine("Enter recipe name: ");
                string rName = Console.ReadLine().Trim();

                List<string> ingredients = new List<string>();
                List<string> steps = new List<string>();

                try
                {
                    Console.WriteLine("Enter the number of ingredients you would like to enter");
                    int iAmount = Convert.ToInt32(Console.ReadLine());
                    for (int x = 0; x < iAmount; x++) // Loop to get ingredients
                    {
                        Console.WriteLine($"Ingredient {x + 1}:");

                        Console.WriteLine("Enter Ingredient Name: ");
                        string iName = Console.ReadLine();
                        Console.WriteLine("Enter ingredient quantity");
                        int iQuantity = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Enter unit of measurement: ");
                        string uMeasurement = Console.ReadLine();
                        Console.WriteLine("Enter amount of calories in ingredient: ");
                        int iCalories = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Select the food group for this ingredient:"); // Prompt the user to select the food group
                        Console.WriteLine("1. Protein");
                        Console.WriteLine("2. Carbohydrates");
                        Console.WriteLine("3. Dairy");
                        Console.WriteLine("4. Fruits");
                        Console.WriteLine("5. Vegetables");
                        int foodGroupChoice = int.Parse(Console.ReadLine()); // Get the user's choice for the food group
                        string foodGroup = "";

                        switch (foodGroupChoice) // user selection of the food group
                        {
                            case 1:
                                foodGroup = "Protein";
                                break;
                            case 2:
                                foodGroup = "Carbohydrates";
                                break;
                            case 3:
                                foodGroup = "Dairy";
                                break;
                            case 4:
                                foodGroup = "Fruits";
                                break;
                            case 5:
                                foodGroup = "Vegetables";
                                break;
                        }


                        string ingredient = $"{iQuantity} {uMeasurement} of {iName} \n Calories: {iCalories} \n Food Group: {foodGroup}";
                        ingredients.Add(ingredient);
                    }

                    Console.WriteLine("Enter amount of steps: ");
                    int stepsCount = Convert.ToInt32(Console.ReadLine());
                    for (int x = 0; x < stepsCount; x++) // Loop to get steps
                    {
                        Console.WriteLine($"Step {x + 1}:");
                        string step = Console.ReadLine().Trim();
                        steps.Add(step);
                    }

                    // Add recipe to the dictionary
                    recipeBook[rName] = (ingredients, steps);
                    Console.WriteLine("RECIPE SUCCESSFULLY CAPTURED\n*****************************************");
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input. Please enter a valid number.");
                    i--; // Retry the current recipe
                }
            }
        }

        public void DisplayRecipe() // Method for displaying recipes
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Enter the name of the recipe you want to display:");
            string recipeName = Console.ReadLine().Trim();

            if (recipeBook.ContainsKey(recipeName))
            {
                var recipe = recipeBook[recipeName];
                Console.WriteLine($"\nRecipe: {recipeName}");
                Console.WriteLine("Ingredients:");
                foreach (var ingredient in recipe.Ingredients)
                {
                    Console.WriteLine(ingredient);
                }
                Console.WriteLine("Steps:");
                foreach (var step in recipe.Steps)
                {
                    Console.WriteLine(step);
                }
                Console.WriteLine("*****************************************");
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void ScaleRecipe(int scaleFactor) // Method for scaling ingredients
        {
            sFactor.Add(scaleFactor); // Add scale factor to the list

            foreach (var recipeName in recipeBook.Keys.ToList())
            {
                var recipe = recipeBook[recipeName];
                for (int i = 0; i < recipe.Ingredients.Count; i++)
                {
                    string ingredient = recipe.Ingredients[i];
                    string[] parts = ingredient.Split(' ');

                    if (int.TryParse(parts[0], out int quantity))
                    {
                        string unit = parts[1];
                        string name = string.Join(" ", parts, 3, parts.Length - 3);
                        int sQuantity = quantity * scaleFactor;
                        recipe.Ingredients[i] = $"{sQuantity} {unit} of {name}";
                    }
                }

                // Update the dictionary with scaled ingredients
                recipeBook[recipeName] = (recipe.Ingredients, recipe.Steps);
            }

            Console.WriteLine("\nRecipe scaled successfully.");
            DisplayRecipe();
        }
        public void CalculateTotalCalories()
        {
            
            Console.WriteLine("Enter the name of the recipe to calculate total calories:");
            string recipeName = Console.ReadLine().Trim();


            var recipe = recipeBook[recipeName];
            int totalCalories = 0;

            foreach (var ingredient in recipe.Ingredients)
            {
                // Splitting the ingredient string to extract the calorie count
                string[] parts = ingredient.Split(new string[] { "\n Calories: " }, StringSplitOptions.None);

                if (parts.Length > 1)
                {
                    // Extracting the calorie count and converting it to an integer
                    if (int.TryParse(parts[1].Trim(), out int calories))
                    {
                        totalCalories += calories;
                    }
                }
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Total calories in {recipeName}: {totalCalories}");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void ResetQuantities() // Method for resetting ingredients
        {
            if (sFactor.Count == 0)
            {
                Console.WriteLine("No scaling factor found.");
                return;
            }

            int scaleFactor = sFactor.Last(); // Get the last scaling factor

            foreach (var recipeName in recipeBook.Keys.ToList())
            {
                var recipe = recipeBook[recipeName];
                for (int i = 0; i < recipe.Ingredients.Count; i++)
                {
                    string ingredient = recipe.Ingredients[i];
                    string[] parts = ingredient.Split(' ');

                    if (int.TryParse(parts[0], out int quantity))
                    {
                        string unit = parts[1];
                        string name = string.Join(" ", parts, 3, parts.Length - 3);
                        int originalQuantity = quantity / scaleFactor;
                        recipe.Ingredients[i] = $"{originalQuantity} {unit} of {name}";
                    }
                }

                // Update the dictionary with reset ingredients
                recipeBook[recipeName] = (recipe.Ingredients, recipe.Steps);
            }

            Console.WriteLine("\nQuantities reset successfully.");
            DisplayRecipe();
        }

        public void ClearRecipes() // Method for clearing all recipes
        {
            recipeBook.Clear();
            sFactor.Clear();
            Console.WriteLine("\nAll recipes have been cleared.");
        }
    }
}
