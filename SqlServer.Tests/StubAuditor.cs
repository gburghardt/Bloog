using Bloog.Infrastructure;
using System;

namespace Bloog.SqlServer.Tests
{
    class StubAuditor : IAuditor
    {
        public int UserId { get; }

        private readonly Func<DateTime> currentDateTime;

        public StubAuditor() : this(1)
        {
        }

        public StubAuditor(int userId)
        {
            UserId = userId;
            currentDateTime = () => DateTime.UtcNow;
        }

        public StubAuditor(int userId, Func<DateTime> currentDateTime)
        {
            UserId = userId;
            this.currentDateTime = currentDateTime;
        }

        public DateTime Now()
        {
            return currentDateTime();
        }
    }
}
