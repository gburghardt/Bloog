using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Bloog.SqlServer.Tests")]

namespace Bloog.SqlServer
{
    internal class PropertyChange
    {
        public object OldValue { get; }
        public object NewValue { get; }

        public PropertyChange(object oldValue, object newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}