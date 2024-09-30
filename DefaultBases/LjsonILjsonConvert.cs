using System.Collections;
using System.Collections.Generic;

namespace Ljson.DefaultBases
{
    public class LjsonILjsonConvert<T> :BaseLjsonConverter<T, T> where T: ILjsonable
    {
        public override IList<object> ExtractValues(T obj) => obj.ExtractValues();

        public override void AssignValues(T obj, IList<string> values) => obj.AssignValues(values);
    }
}