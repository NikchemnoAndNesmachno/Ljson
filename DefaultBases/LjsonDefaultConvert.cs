using System.Collections;
using System.Collections.Generic;
using Ljson.ConvertStringsStrategy;

namespace Ljson.DefaultBases
{
    public abstract class LjsonDefaultConvert<T>: BaseLjsonAssigner<T>
    {
        public LjsonDefaultConvert()
        {
            ConvertStrategy = new SimpleStrategy();
        }
        public abstract override IList<object> ExtractValues(T obj);

        public abstract override void AssignValues(T obj, IList<string> values);
    }
}