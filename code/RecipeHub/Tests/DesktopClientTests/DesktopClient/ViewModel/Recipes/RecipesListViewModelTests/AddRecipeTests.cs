﻿using Desktop_Client.Service.Recipes;
using Desktop_Client.ViewModel.Recipes;
using Moq;

namespace DesktopClientTests.DesktopClient.ViewModel.Recipes.RecipesListViewModelTests
{
    public class AddRecipeTests
    {
        [Test]
        public void SuccessfullyAddRecipe()
        {
            const string sessionKey = "Key";
            const string name = "name";
            const string description = "description";
            const bool isPublic = true;

            var service = new Mock<IRecipesService>();
            service.Setup(mock => mock.AddRecipe(sessionKey, name, description, isPublic));

            var viewmodel = new RecipesListViewModel(service.Object);

            Assert.Multiple(() =>
            {
                Assert.DoesNotThrow(() => viewmodel.AddRecipe(sessionKey, name, description, isPublic));
                service.Verify(mock => mock.AddRecipe(sessionKey, name, description, isPublic), Times.Once);
            });
        }
    }
}
