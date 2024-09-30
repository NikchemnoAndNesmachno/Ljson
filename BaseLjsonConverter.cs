using System.Collections;
using System.Collections.Generic;
using Ljson.ConvertStringsStrategy;
using Ljson.FabricMethods;

namespace Ljson
{
    public abstract class BaseLjsonConverter<TInput, TOutput> : ILjsonConverter<TInput, TOutput>
    {
        public IConvertStringStrategy ConvertStrategy { get; set; }
        public ICreateInstance<TOutput> CreateStrategy { get; set; }
        public abstract IList<object> ExtractValues(TInput obj);
        public abstract void AssignValues(TOutput obj, IList<string> values);
        public string ToLjson(TInput obj) => ConvertStrategy.ListToLjson(ExtractValues(obj));

        public virtual TOutput CreateInstance(IList<string> values) => 
            CreateStrategy.CreateInstance(values.Count);

        public virtual TOutput FromLjson(string ljson)
        {
            var values = ConvertStrategy.LjsonToList(ljson);
            var t = CreateInstance(values);
            AssignValues(t, values);
            return t;
        }
        
        
    }
}