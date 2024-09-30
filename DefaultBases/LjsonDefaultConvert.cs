using System.Collections;
using System.Collections.Generic;
using Ljson.FabricMethods;

namespace Ljson.DefaultBases
{
    public abstract class LjsonDefaultConvert<T>: BaseLjsonConverter<T, T> where T: new()
    {
        public LjsonDefaultConvert()
        {
            CreateStrategy = new ObjectCreateInstance<T>();
        }
        public abstract override IList<object> ExtractValues(T obj);

        public abstract override void AssignValues(T obj, IList<string> values);
    }
}