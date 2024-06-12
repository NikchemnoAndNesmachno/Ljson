using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Ljson
{
    public abstract class LjsonConvert<T>: BaseLjsonConverter<T> where T: new()
    {
        public override T FromLjson(string ljson)
        {
            var values = GetStringValues(ljson);
            var obj = new T();
            SetValues(obj, values);
            return obj;
        }
    }
}
