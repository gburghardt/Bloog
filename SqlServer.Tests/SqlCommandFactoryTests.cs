using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Bloog.SqlServer.Tests
{
    public class SqlCommandFactoryTests
    {
        [Fact]
        public void UpdateStatementTest()
        {
            var factory = new SqlCommandFactory();
            var changes = new Dictionary<string, PropertyChange>()
            {
                { "Name", new PropertyChange("A", "B") }
            };
            var command = factory.CreateUpdateStatement(changes, "[dbo].[Blog]", "Id", 4);
            var expectedSql = @"UPDATE [dbo].[Blog] SET [Name] = @Name WHERE [Id] = @Id";

            Assert.Equal(expectedSql, command.CommandText);
        }
    }
}
