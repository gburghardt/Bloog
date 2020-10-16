using Bloog.Infrastructure;
using System;

namespace Bloog.SqlServer.Tests
{
    class StubAuditor : IAuditor
    {
        public Guid UserId { get; }

        private readonly Func<DateTime> currentDateTime;

        public StubAuditor() : this(Guid.NewGuid())
        {
        }

        public StubAuditor(Guid userId)
        {
            UserId = userId;
            currentDateTime = () => DateTime.UtcNow;
        }

        public StubAuditor(Guid userId, Func<DateTime> currentDateTime)
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
