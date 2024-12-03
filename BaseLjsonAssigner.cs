using System.Collections.Generic;
using Ljson.ConvertStringsStrategy;

namespace Ljson
{
    public abstract class BaseLjsonAssigner<TInput>: ILjsonAssigner<TInput>
    {
        public IConvertStringStrategy ConvertStrategy { get; set; } = new SimpleStrategy();
        public abstract IList<object> ExtractValues(TInput obj);
        public abstract void AssignValues(TInput obj, IList<string> values);
        
        public virtual string ToLjson(TInput obj) => 
            ConvertStrategy.ListToLjson(ExtractValues(obj));

        public virtual void FromLjson(TInput obj, string ljson)
        {
            var values = ConvertStrategy.LjsonToList(ljson);
            AssignValues(obj, values);
        }
    }
}