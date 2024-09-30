using System.Collections.Generic;

namespace Ljson.FabricMethods
{
    public class ArrayCreateInstance<T>: ICreateInstance<IList<T>>
    {
        public IList<T> CreateInstance(object parameter) => new T[(int)parameter];
    }
}