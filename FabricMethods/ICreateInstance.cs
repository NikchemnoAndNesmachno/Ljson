namespace Ljson.FabricMethods
{
    public interface ICreateInstance<out T>
    {
        T CreateInstance(object parameter);
    }
}