using System;
using System.Collections.Generic;
using System.Text;

namespace Libraries.Custominterface
{
    public interface IValueRange<T>
    {
        T Start { get; set; }

        T End { get; set; }
    }
}
