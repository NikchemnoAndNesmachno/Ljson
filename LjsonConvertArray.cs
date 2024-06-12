using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Ljson
{
    public abstract class LjsonConvertArray<T> : BaseLjsonConverter<T[]>
    {
        public override T[] FromLjson(string ljson)
        {
            var values = GetStringValues(ljson);
            var obj = new T[values.Length];
            SetValues(obj, values);
            return obj;
        }
    }
}

