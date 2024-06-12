using System;
using System.Collections.Generic;
using System.Text;

namespace Ljson
{
    public interface ILjsonable
    {
        void SetValues(string[] values);
        string[] GetValues();
    }
}
