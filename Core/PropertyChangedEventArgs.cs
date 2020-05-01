using System;

namespace Bloog
{
    public class PropertyChangedEventArgs<TKey, TValue> : EventArgs
    {
        public TKey Key { get; }
        public TValue OldValue { get; }
        public TValue NewValue { get; }

        public PropertyChangedEventArgs(TKey key, TValue oldValue, TValue newValue)
        {
            Key = key;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}