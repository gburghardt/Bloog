using System;

namespace Bloog.SqlServer
{
    public interface IAuditor
    {
        /// <summary>
        /// Returns the current user Id used to set created/updated by info
        /// </summary>
        int UserId { get; }

        /// <summary>
        /// Returns the current date and time in UTC
        /// </summary>
        /// <returns></returns>
        DateTime Now();
    }
}
