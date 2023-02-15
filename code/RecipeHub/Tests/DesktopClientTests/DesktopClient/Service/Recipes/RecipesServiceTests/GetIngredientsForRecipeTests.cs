﻿using Desktop_Client.Endpoints.Recipes;
using Desktop_Client.Service.Recipes;
using Moq;
using Shared_Resources.ErrorMessages;
using Shared_Resources.Model.Ingredients;
using Shared_Resources.Model.Recipes;

namespace DesktopClientTests.DesktopClient.Service.Recipes.RecipesServiceTests
{
    public class GetIngredientsForRecipeTests
    {
        [Test]
        public void SuccessfullyGetRecipeSteps()
        {
            var ingredients = new Ingredient[] {
                new ("name", 0, MeasurementType.Volume),
            };
            const string sessionKey = "Key";
            const int recipeId = 0;

            var recipesEndpoint = new Mock<IRecipesEndpoints>();
            recipesEndpoint.Setup(mock => mock.GetIngredientsForRecipe(sessionKey, recipeId)).Returns(ingredients);

            var service = new RecipesService(recipesEndpoint.Object);
            var result = service.GetIngredientsForRecipe(sessionKey, recipeId);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.EquivalentTo(ingredients));
                recipesEndpoint.Verify(mock => mock.GetIngredientsForRecipe(sessionKey, recipeId), Times.Once);
            });
        }

        [Test]
        public void NullSessionKey()
        {
            var errorMessage = SessionKeyErrorMessages.SessionKeyCannotBeNull + " (Parameter 'sessionKey')";
            Assert.Multiple(() =>
            {
                var message = Assert.Throws<ArgumentNullException>(
                    () => new RecipesService().GetIngredientsForRecipe(null!, 0))!.Message;
                Assert.That(message, Is.EqualTo(errorMessage));
            });
        }

        [Test]
        public void EmptySessionKey()
        {
            var errorMessage = SessionKeyErrorMessages.SessionKeyCannotBeEmpty;
            Assert.Multiple(() =>
            {
                var message = Assert.Throws<ArgumentException>(
                    () => new RecipesService().GetIngredientsForRecipe("", 0))!.Message;
                Assert.That(message, Is.EqualTo(errorMessage));
            });
        }
    }
}