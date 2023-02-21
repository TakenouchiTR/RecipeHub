﻿using Web_Client.Endpoints.Recipes;
using Web_Client.Service.Recipes;
using Moq;
using Shared_Resources.ErrorMessages;
using Web_Client.Service.Users;

namespace WebClientTests.WebClient.Service.Recipes.RecipesServiceTests
{
    public class RemoveRecipeTests
    {
        [Test]
        public void SuccessfullyRemoveRecipe()
        {
            const string sessionKey = "Key";
            const int recipeId = 1;

            var recipesEndpoint = new Mock<IRecipesEndpoints>();
            var usersService = new Mock<UsersService>();

            recipesEndpoint.Setup(mock => mock.RemoveRecipe(sessionKey, recipeId));
            usersService.Setup(mock => mock.RefreshSessionKey());

            var service = new RecipesService(recipesEndpoint.Object, usersService.Object);

            Assert.Multiple(() =>
            {
                Assert.DoesNotThrow(() => service.RemoveRecipe(sessionKey, recipeId));
                recipesEndpoint.Verify(mock => mock.RemoveRecipe(sessionKey, recipeId), Times.Once);
            });
        }

        [Test]
        public void NullSessionKey()
        {
            const string sessionKey = null!;
            const int recipeId = 1;

            const string errorMessage = SessionKeyErrorMessages.SessionKeyCannotBeNull + " (Parameter 'sessionKey')";
            Assert.Multiple(() =>
            {
                var message = Assert.Throws<ArgumentNullException>(() =>
                    new RecipesService().RemoveRecipe(sessionKey!, recipeId))!.Message;
                Assert.That(message, Is.EqualTo(errorMessage));
            });
        }

        [Test]
        public void EmptySessionKey()
        {
            const string sessionKey = "";
            const int recipeId = 1;

            const string errorMessage = SessionKeyErrorMessages.SessionKeyCannotBeEmpty;
            Assert.Multiple(() =>
            {
                var message = Assert.Throws<ArgumentException>(() =>
                    new RecipesService().RemoveRecipe(sessionKey, recipeId))!.Message;
                Assert.That(message, Is.EqualTo(errorMessage));
            });
        }
    }
}