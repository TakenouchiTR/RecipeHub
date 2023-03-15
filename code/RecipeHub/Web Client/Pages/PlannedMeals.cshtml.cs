using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared_Resources.Model.PlannedMeals;
using Web_Client.ViewModel.PlannedMeals;

namespace Web_Client.Pages
{
    /// <summary>
    /// Model for the Planned Meals Page.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.RazorPages.PageModel" />
    public class PlannedMealsModel : PageModel
    {
        /// <summary>
        /// Gets or sets the view model.
        /// </summary>
        /// <value>
        /// The view model.
        /// </value>
        public PlannedMealsViewModel ViewModel { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlannedMealsModel"/> class.
        /// </summary>
        public PlannedMealsModel()
        {
            this.ViewModel= new PlannedMealsViewModel();
            this.ViewModel.Initialize();
        }

        /// <summary>
        /// Called when [get].
        /// </summary>
        public void OnGet()
        {
        }

        /// <summary>
        /// Called when [post remove planned meal].
        /// </summary>
        /// <returns>a redirect to the planned meals page.</returns>
        public IActionResult OnPostRemovePlannedMeal()
        {
            int recipeId = int.Parse(Request.Form["RecipeId"][0]!);
            MealCategory mealCategory = (MealCategory)int.Parse(Request.Form["Category"][0]!);
            DateTime date = DateTime.Parse(Request.Form["Date"][0]!);
            this.ViewModel.RemovePlannedMeal(date, mealCategory, recipeId);
            return RedirectToPage("PlannedMeals");
        }
    }
}