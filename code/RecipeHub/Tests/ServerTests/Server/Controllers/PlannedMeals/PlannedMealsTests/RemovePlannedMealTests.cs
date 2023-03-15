﻿using System.Net;
using Moq;
using Server.Controllers.PlannedMeals;
using Server.Controllers.ResponseModels;
using Server.Data.Settings;
using Server.Service.PlannedMeals;
using Shared_Resources.Model.PlannedMeals;

namespace ServerTests.Server.Controllers.PlannedMeals.PlannedMealsTests
{
    public class RemovePlannedMealTests
    {
        [Test]
        public void ShouldRemovePlannedMeal()
        {
            var sessionKey = "key";
            var date = new DateTime(2023, 03, 03);
            var category = MealCategory.Breakfast;
            var recipeId = 1;

            var expected = new BaseResponseModel(HttpStatusCode.OK, ServerSettings.DefaultSuccessfulConnectionMessage);

            var service = new Mock<IPlannedMealsService>();
            var controller = new PlannedMealsController(service.Object);

            service.Setup(mock => mock.RemovePlannedMeal(sessionKey, date, category, recipeId));

            var response = controller.RemovePlannedMeal(sessionKey, date, category, recipeId);

            Assert.That(response.Code, Is.EqualTo(expected.Code));
            Assert.That(response.Message, Is.EqualTo(expected.Message));
            service.Verify(mock => mock.RemovePlannedMeal(sessionKey, date, category, recipeId), Times.Once);
        }

        [Test]
        public void ShouldNotRemovePlannedMealIfNoSession()
        {
            var sessionKey = "key";
            var date = new DateTime(2023, 03, 03);
            var category = MealCategory.Breakfast;
            var recipeId = 1;

            var expected = new BaseResponseModel(HttpStatusCode.Unauthorized, "message");

            var service = new Mock<IPlannedMealsService>();
            var controller = new PlannedMealsController(service.Object);

            service.Setup(mock => mock.RemovePlannedMeal(sessionKey, date, category, recipeId)).Throws(() => new UnauthorizedAccessException("message"));

            var response = controller.RemovePlannedMeal(sessionKey, date, category, recipeId);

            Assert.That(response.Code, Is.EqualTo(expected.Code));
            Assert.That(response.Message, Is.EqualTo(expected.Message));
            service.Verify(mock => mock.RemovePlannedMeal(sessionKey, date, category, recipeId), Times.Once);
        }

        [Test]
        public void ShouldNotRemovePlannedMealIfServerError()
        {
            var sessionKey = "key";
            var date = new DateTime(2023, 03, 03);
            var category = MealCategory.Breakfast;
            var recipeId = 1;

            var expected = new BaseResponseModel(HttpStatusCode.InternalServerError, "message");

            var service = new Mock<IPlannedMealsService>();
            var controller = new PlannedMealsController(service.Object);

            service.Setup(mock => mock.RemovePlannedMeal(sessionKey, date, category, recipeId)).Throws(() => new ArgumentException("message"));

            var response = controller.RemovePlannedMeal(sessionKey, date, category, recipeId);

            Assert.That(response.Code, Is.EqualTo(expected.Code));
            Assert.That(response.Message, Is.EqualTo(expected.Message));
            service.Verify(mock => mock.RemovePlannedMeal(sessionKey, date, category, recipeId), Times.Once);
        }
    }
}
