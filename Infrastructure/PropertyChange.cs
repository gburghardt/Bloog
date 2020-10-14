using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Bloog.SqlServer.Tests")]

namespace Bloog.Infrastructure
{
    public class PropertyChange
    {
        public object OldValue { get; }
        public object NewValue { get; }

        internal PropertyChange(object oldValue, object newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}