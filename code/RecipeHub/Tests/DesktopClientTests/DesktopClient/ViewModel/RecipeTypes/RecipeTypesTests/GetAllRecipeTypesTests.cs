﻿using Desktop_Client.Endpoints.RecipeTypes;
using Desktop_Client.Service.RecipeTypes;
using Desktop_Client.ViewModel.RecipeTypes;
using Moq;

namespace DesktopClientTests.DesktopClient.ViewModel.RecipeTypes.RecipeTypesTests
{
    public class GetAllRecipeTypesTests
    {
        [Test]
        public void ShouldGetAllRecipeTypes()
        {
            var types = new[]
            {
                "type1", "type2", "type3"
            };
            var endpoints = new Mock<IRecipeTypesService>();
            var service = new RecipeTypesViewModel(endpoints.Object);

            endpoints.Setup(mock => mock.GetAllRecipeTypes()).Returns(types);

            var actual = service.GetAllRecipeTypes();

            Assert.That(actual, Is.EqualTo(types));
        }
    }
}
