using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YellowbrickV6
{
    internal static class UnitTools
    {
        internal static double Nm2M(double nm)
        {
            return nm * 1852;
        }

        internal static double M2Nm(double m)
        {
            return m / 1852;
        }
    }
}
