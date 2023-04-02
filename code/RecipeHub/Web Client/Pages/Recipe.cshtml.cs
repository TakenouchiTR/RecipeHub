using System.Reflection.Metadata.Ecma335;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared_Resources.Model.PlannedMeals;
using Shared_Resources.Utils.Dates;
using Shared_Resources.Utils.Units;
using Web_Client.Model.States;
using Web_Client.Service.Recipes;
using Web_Client.ViewModel.Recipes;

namespace Web_Client.Pages
{
    /// <summary>
    /// Model for the Individual Recipe Page
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.RazorPages.PageModel" />
    public class RecipeModel : PageModel
    {
        /// <summary>
        /// Gets or sets the view model.
        /// </summary>
        /// <value>
        /// The view model.
        /// </value>
        [BindProperty]
        public RecipeViewModel ViewModel { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id{ get; set; }

        /// <summary>
        /// Gets or sets the planned meal dates.
        /// </summary>
        /// <value>
        /// The planned meal dates.
        /// </value>
        public DateTime[] PlannedMealDates { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecipeModel"/> class.<br />
        /// <br />
        /// Precondition: None<br />
        /// Postcondition: Fields are set to default values.
        /// </summary>
        public RecipeModel()
        {
            this.ViewModel = new RecipeViewModel();
            var startDate = DateTime.Now.Date;

            this.PlannedMealDates = DateUtils.GenerateDateTimesFromWeekToNextWeek(startDate)
                                             .Where(date => date.Date >= startDate).ToArray();
        }

        /// <summary>
        /// Called when the page is opened.<br />
        /// <br />
        /// Precondition: None<br />
        /// Postcondition: Values necessary for web page have been set.<br />
        /// </summary>
        /// <param name="id">The id of the individual recipe.</param>
        public void OnGet(int id)
        {
            try
            {
                this.ViewModel.Initialize(id);
                this.ViewModel.GetMissingIngredientsForRecipe();
                RecipePageState.ViewModel = this.ViewModel;
            }
            catch (UnauthorizedAccessException exception)
            {
                TempData["Message"] = exception.Message;
                Response.Redirect("/Index");
            }
        }

        /// <summary>
        /// Called when [post add planned meal].
        /// </summary>
        /// <returns>the content of the planned meal confirmation.</returns>
        public IActionResult OnPostAddPlannedMeal()
        {
            this.ViewModel = RecipePageState.ViewModel!;
            string date = Request.Form["Date"][0]!;
            int mealCategory = int.Parse(Request.Form["MealCategory"][0]!);
            try
            {
                this.ViewModel.AddRecipeToPlannedMeals(DateTime.Parse(date), (MealCategory)mealCategory);

            }
            catch (ArgumentException exception)
            {
                return BadRequest(exception.Message);
            }
            RecipePageState.ViewModel = this.ViewModel;
            return Content(this.ViewModel.PlannedMealAddedMessage);
        }

        public IActionResult OnPostCookRecipe()
        {
            this.ViewModel = RecipePageState.ViewModel!;

            this.ViewModel.RemoveIngredientsForRecipe();

            RecipePageState.ViewModel = this.ViewModel;
            return RedirectToPage("Ingredients");
        }

        /// <summary>
        /// Gets the text for missing ingredients.
        /// </summary>
        /// <returns></returns>
        public string GetTextForMissingIngredients()
        {
            if (this.ViewModel.MissingIngredients.Length == 0)
            {
                return "You have all the ingredients for this meal!";
            }
            var message = new StringBuilder();
            message.Append(
                "You are missing some ingredients. If you cook this meal, they will be removed from your pantry (if present):");
            foreach (var ingredient in this.ViewModel.MissingIngredients)
            {
                var unit = BaseUnitUtils.GetBaseUnitSign(ingredient.MeasurementType);
                message.Append($"\n - {ingredient.Name}: {ingredient.Amount} {unit}");
            }

            return message.ToString();
        }
    }
}
