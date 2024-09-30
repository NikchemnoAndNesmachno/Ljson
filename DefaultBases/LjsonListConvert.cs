using System;
using System.Collections;
using System.Collections.Generic;
using Ljson.FabricMethods;

namespace Ljson.DefaultBases
{
    public abstract class LjsonListConvert<T>: BaseLjsonConverter<IList<T>, IList<T>>
    {
        public LjsonListConvert()
        {
            CreateStrategy = new ArrayCreateInstance<T>();
        }
        public abstract override IList ExtractValues(IList<T> obj);

        public abstract override void AssignValues(IList<T> obj, IList<string> values);
    }
}