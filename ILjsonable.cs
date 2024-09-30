using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Ljson
{
    public interface ILjsonable
    {
        void AssignValues(IList<string> values);
        IList ExtractValues();
    }
}
