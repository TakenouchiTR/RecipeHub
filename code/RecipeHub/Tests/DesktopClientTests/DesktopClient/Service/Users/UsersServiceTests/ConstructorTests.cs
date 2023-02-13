﻿using Desktop_Client.Service.Users;
using Shared_Resources.ErrorMessages;

namespace DesktopClientTests.DesktopClient.Service.Users.UsersServiceTests
{
    public class ConstructorTests
    {
        [Test]
        public void ShouldNotAllowNullUsersEndpoints()
        {
            var message = Assert.Throws<ArgumentException>(() =>
            {
                _ = new UsersService(null!);
            })?.Message;
            Assert.That(message, Is.EqualTo(UsersServiceErrorMessages.UsersEndpointsCannotBeNull));
        }
    }
}
