using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YellowbrickV6
{
    public static class UnitTools
    {
        public static double Nm2M(double nm)
        {
            return nm * 1852;
        }

        public static double M2Nm(double m)
        {
            return m / 1852;
        }
    }
}
