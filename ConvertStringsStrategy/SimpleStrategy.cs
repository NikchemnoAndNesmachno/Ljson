using System.Collections;
using System.Collections.Generic;

namespace Ljson.ConvertStringsStrategy
{
    public class SimpleStrategy: IConvertStringStrategy
    {
        public char Char { get; set; }
        public IList<string> LjsonToList(string ljson) => ljson.Split(Char);
        public string ListToLjson(IList values) => string.Join(Char.ToString(), values);
    }
}