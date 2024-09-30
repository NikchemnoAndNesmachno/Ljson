using System.Collections;
using System.Collections.Generic;

namespace Ljson.ConvertStringsStrategy
{
    public interface IConvertStringStrategy
    {
        IList<string> LjsonToList(string ljson);
        string ListToLjson(IList<object> values);
    }
}