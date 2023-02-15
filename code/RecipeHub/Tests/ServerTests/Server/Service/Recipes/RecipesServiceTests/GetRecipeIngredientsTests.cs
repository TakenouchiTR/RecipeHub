﻿using Moq;
using Server.DAL.Recipes;
using Server.DAL.Users;
using Server.Service.Recipes;
using Shared_Resources.Model.Ingredients;
using Shared_Resources.Model.Recipes;

namespace ServerTests.Server.Service.Recipes.RecipesServiceTests
{
    public class GetRecipeIngredientsTests
    {
        [Test]
        public void RetrieveValidIngredientList()
        {
            const string sessionKey = "Key";
            const int recipeId = 0;
            const int userId = 0;
            var ingredients = new Ingredient[] {
                new ("name", 5, MeasurementType.Volume)
            };

            var recipesDal = new Mock<IRecipesDal>();
            var usersDal = new Mock<IUsersDal>();
            recipesDal.Setup(mock => mock.GetIngredientsForRecipe(recipeId)).Returns(ingredients);
            recipesDal.Setup(mock => mock.UserCanSeeRecipe(userId, recipeId)).Returns(true);
            usersDal.Setup(mock => mock.GetIdForSessionKey(sessionKey)).Returns(userId);

            var service = new RecipesService(recipesDal.Object, usersDal.Object);
            var result = service.GetRecipeIngredients(sessionKey, recipeId);

            Assert.Multiple(() =>
            {
                recipesDal.Verify(mock => mock.GetIngredientsForRecipe(recipeId), Times.Once);
                recipesDal.Verify(mock => mock.UserCanSeeRecipe(userId, recipeId), Times.Once);
                usersDal.Verify(mock => mock.GetIdForSessionKey(sessionKey), Times.Once);
                Assert.That(result, Is.EqualTo(ingredients));
            });
            
        }

        [Test]
        public void NullSessionKey()
        {
            Assert.Throws<ArgumentNullException>(() => new RecipesService().GetRecipeIngredients(null!, 0));
        }

        [Test]
        public void EmptySessionKey()
        {
            Assert.Throws<ArgumentException>(() => new RecipesService().GetRecipeIngredients("", 0));
        }

        [Test]
        public void SessionKeyIsUnused()
        {
            var recipesDal = new Mock<IRecipesDal>();
            var usersDal = new Mock<IUsersDal>();
            usersDal.Setup(mock => mock.GetIdForSessionKey("0")).Returns((int?)null);

            var service = new RecipesService(recipesDal.Object, usersDal.Object);
            Assert.Throws<ArgumentException>(() => service.GetRecipeIngredients("0", 0));
        }

        [Test]
        public void UserCannotSeeRecipe()
        {
            const string sessionKey = "Key";
            const int recipeId = 0;
            const int userId = 0;

            var recipesDal = new Mock<IRecipesDal>();
            var usersDal = new Mock<IUsersDal>();
            recipesDal.Setup(mock => mock.UserCanSeeRecipe(userId, recipeId)).Returns(false);
            usersDal.Setup(mock => mock.GetIdForSessionKey(sessionKey)).Returns(userId);

            var service = new RecipesService(recipesDal.Object, usersDal.Object);

            Assert.Multiple(() =>
            {
                Assert.Throws <ArgumentException>(() => service.GetRecipeIngredients(sessionKey, recipeId));
                recipesDal.Verify(mock => mock.UserCanSeeRecipe(userId, recipeId), Times.Once);
                usersDal.Verify(mock => mock.GetIdForSessionKey(sessionKey), Times.Once);
            });
        }
    }
}