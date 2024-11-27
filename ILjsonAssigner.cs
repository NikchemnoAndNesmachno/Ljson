namespace Ljson
{
    public interface ILjsonAssigner<in TInput>
    {
        string ToLjson(TInput obj);
        void FromLjson(TInput obj, string ljson);
    }
}