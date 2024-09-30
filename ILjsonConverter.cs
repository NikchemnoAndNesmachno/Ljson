using System.Collections;
using System.Collections.Generic;

namespace Ljson
{
    public interface ILjsonConverter<in TInput, out TOutput>
    {
        string ToLjson(TInput obj);
        TOutput FromLjson(string ljson);
    }
}