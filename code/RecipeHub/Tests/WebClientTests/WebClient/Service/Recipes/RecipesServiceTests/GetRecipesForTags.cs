﻿using Moq;
using Shared_Resources.Data.UserData;
using Shared_Resources.ErrorMessages;
using Shared_Resources.Model.Recipes;
using Web_Client.Endpoints.Recipes;
using Web_Client.Service.Recipes;
using Web_Client.Service.Users;

namespace WebClientTests.WebClient.Service.Recipes.RecipesServiceTests
{
    public class GetRecipesForTags
    {
        [Test]
        public void ShouldNotAllowNullSessionKey()
        {
            var service = new RecipesService();
            Session.Key = null;
            var message = Assert.Throws<ArgumentException>(() =>
            {
                _ = service.GetRecipesForTags(new[] { "tag1" });
            })?.Message;
            Assert.That(message, Is.EqualTo(SessionKeyErrorMessages.SessionKeyCannotBeNull));
        }

        [Test]
        public void ShouldNotAllowEmptySessionKey()
        {
            var service = new RecipesService();
            Session.Key = "   ";
            var message = Assert.Throws<ArgumentException>(() =>
            {
                _ = service.GetRecipesForTags(new[] { "tag1" });
            })?.Message;
            Assert.That(message, Is.EqualTo(SessionKeyErrorMessages.SessionKeyCannotBeEmpty));
        }

        [Test]
        public void ShouldNotAllowNullTags()
        {
            var service = new RecipesService();
            Session.Key = "Key";
            var message = Assert.Throws<ArgumentException>(() =>
            {
                _ = service.GetRecipesForTags(null!);
            })?.Message;
            Assert.That(message, Is.EqualTo(RecipesServiceErrorMessages.RecipeTagsCannotBeNull));
        }

        [Test]
        public void ShouldGetRecipesForTags()
        {
            var recipes = new[]
            {
                new Recipe(1, "author", "name1", "desc1", true),
                new Recipe(2, "author", "name2", "desc2", true),
                new Recipe(3, "author", "name3", "desc3", true)
            };

            Session.Key = "Key";
            var tags = new[] { "tag1", "tag2", "tag3" };

            var endpoints = new Mock<IRecipesEndpoints>();
            var usersService = new Mock<IUsersService>();
            var service = new RecipesService(endpoints.Object, usersService.Object);

            usersService.Setup(mock => mock.RefreshSessionKey());
            endpoints.Setup(mock => mock.GetRecipesForTags("Key", tags)).Returns(recipes);

            var returnedRecipes = service.GetRecipesForTags(tags);
            Assert.That(returnedRecipes, Is.EqualTo(recipes));


        }
    }
}
