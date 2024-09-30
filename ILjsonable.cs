using System.Collections.Generic;

namespace Ljson
{
    public interface ILjsonable
    {
        void AssignValues(IList<string> values);
        IList<object> ExtractValues();
    }
}
