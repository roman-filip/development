using System;

namespace DecodeNullObjectRefactor
{
    public class DecodeNullObject
    {
        public TObject DecodeNullObjectOldImplementation<TObject>(object innerValue)
        {
            if (innerValue == typeof(NullObject))
            {
                return default(TObject);
            }
            return (TObject)innerValue;
        }

        public TObject DecodeNullObjectNewImplementation<TObject>(object innerValue)
        {
            if (innerValue is Type && (Type)innerValue == typeof(NullObject))
            {
                return default(TObject);
            }
            return (TObject)innerValue;
        }
    }
}
