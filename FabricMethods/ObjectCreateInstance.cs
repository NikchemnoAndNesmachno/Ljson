namespace Ljson.FabricMethods
{
    public class ObjectCreateInstance<T> : ICreateInstance<T> where T : new()
    {
        public T CreateInstance(object parameter) => new T();
    }
}