﻿using Desktop_Client.Endpoints.Ingredients;
using Desktop_Client.Service.Ingredients;
using Moq;
using Shared_Resources.Model.Ingredients;

namespace DesktopClientTests.DesktopClient.Service.Ingredients.IngredientsServiceTests
{
    public class DeleteIngredientTests
    {
        [Test]
        public void SuccessfullyDeleteIngredient()
        {
            var ingredient = new Ingredient();
            var endpoints = new Mock<IIngredientEndpoints>();
            endpoints.Setup(mock => mock.DeleteIngredient(ingredient)).Returns(true);

            var service = new IngredientsService(endpoints.Object);

            Assert.Multiple(() =>
            {
                var result = service.DeleteIngredient(ingredient);
                Assert.That(result, Is.EqualTo(true));
                endpoints.Verify(mock => mock.DeleteIngredient(ingredient), Times.Once);
            });
        }
    }
}