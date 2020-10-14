using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Bloog.SqlServer.Tests")]

namespace Bloog.SqlServer
{
    public class PropertyChange
    {
        internal object OldValue { get; }
        internal object NewValue { get; }

        internal PropertyChange(object oldValue, object newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}