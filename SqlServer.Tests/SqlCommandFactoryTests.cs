using Bloog.Infrastructure;
using System;
using System.Collections.Generic;
using Xunit;

namespace Bloog.SqlServer.Tests
{
    public class SqlCommandFactoryTests
    {
        [Fact]
        public void UpdateStatementTest()
        {
            var expectedSql = @"UPDATE [dbo].[Blog] SET [Name] = @Name, [UpdatedBy] = @UpdatedBy, [UpdatedOn] = @UpdatedOn WHERE [Id] = @Id";
            var expectedUserId = 3;
            var expectedCreateDate = DateTime.Now;
            var auditor = new StubAuditor(expectedUserId, () => expectedCreateDate);
            var factory = new SqlCommandFactory(auditor);
            var changes = new Dictionary<string, PropertyChange>()
            {
                { "Name", new PropertyChange("A", "B") }
            };
            var command = factory.CreateUpdateStatement(changes, "[dbo].[Blog]", "Id", 4);

            Assert.Equal(expectedSql, command.CommandText);
            Assert.Equal(expectedUserId, command.Parameters["UpdatedBy"].Value);
            Assert.Equal(expectedCreateDate, command.Parameters["UpdatedOn"].Value);
            Assert.Equal("B", command.Parameters["Name"].Value);
        }

        [Fact]
        public void UpdateStatementWithNoChanges()
        {
            var factory = new SqlCommandFactory(new StubAuditor());
            var changes = new Dictionary<string, PropertyChange>();

            Assert.Throws<InvalidOperationException>(() => factory.CreateUpdateStatement(changes, "[dbo].[Blog]", "Id", 4));
        }
    }
}
