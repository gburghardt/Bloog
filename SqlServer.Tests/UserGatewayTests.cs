using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Bloog.SqlServer.Tests
{
    public class UserGatewayTests
    {
        const string connectionString = @"Data Source=localhost\sqlexpress;Integrated Security=True";
        [Fact]
        public async void CreateNewUser()
        {
            var auditor = new StubAuditor(Guid.NewGuid(), () => DateTime.UtcNow);
            var gateway = new UserGateway(auditor, connectionString);

            await gateway.CreateUserAsync(auditor.UserId, auditor.UserId.ToString());
        }
    }
}
