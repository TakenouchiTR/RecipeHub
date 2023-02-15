﻿using Desktop_Client.Endpoints.Ingredients;
using Desktop_Client.Service.Ingredients;
using Moq;
using Shared_Resources.Model.Ingredients;

namespace DesktopClientTests.DesktopClient.Service.Ingredients.IngredientsServiceTests
{
    public class AddIngredientTests
    {
        [Test]
        public void SuccessfullyUpdateIngredient()
        {
            var ingredient = new Ingredient();
            var endpoints = new Mock<IIngredientEndpoints>();
            endpoints.Setup(mock => mock.UpdateIngredient(ingredient)).Returns(true);

            var service = new IngredientsService(endpoints.Object);

            Assert.Multiple(() =>
            {
                var result = service.UpdateIngredient(ingredient);
                Assert.That(result, Is.EqualTo(true));
                endpoints.Verify(mock => mock.UpdateIngredient(ingredient), Times.Once);
            });
        }
    }
}