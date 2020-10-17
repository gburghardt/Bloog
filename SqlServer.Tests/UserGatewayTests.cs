using System;
using Xunit;

namespace Bloog.SqlServer.Tests
{
    public class UserGatewayTests : IDisposable
    {
        const string connectionString = @"Data Source=localhost\sqlexpress;Integrated Security=True";

        private Guid expectedUserId;

        private void BeforeTest()
        {
            expectedUserId = Guid.NewGuid();
        }

        [Fact]
        public async void CreateNewUser()
        {
            BeforeTest();

            var expectedUsername = expectedUserId.ToString();
            var auditor = new StubAuditor(expectedUserId, () => DateTime.UtcNow);
            var gateway = new UserGateway(auditor, connectionString);

            await gateway.CreateUserAsync(expectedUserId, expectedUsername);

            var user = await gateway.FindUserAsync(auditor.UserId);

            Assert.Equal(expectedUserId, user.Id);
            Assert.Equal(expectedUsername, user.Username);
        }

        public async void Dispose()
        {
            var auditor = new StubAuditor(expectedUserId, () => DateTime.UtcNow);
            var gateway = new UserGateway(auditor, connectionString);

            await gateway.DeleteUserAsync(expectedUserId);
        }
    }
}
