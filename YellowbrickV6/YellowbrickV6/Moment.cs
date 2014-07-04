using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YellowbrickV6
{
    internal class Moment
    {
        /// <summary>
        /// Latitude positive to north
        /// </summary>
        public double lat;
        /// <summary>
        /// Longiture positive to ease
        /// </summary>
        public double lon;
        /// <summary>
        /// Unix Time at which the measurement was taken
        /// </summary>
        public uint at;
        /// <summary>
        /// Altitude in meters
        /// </summary>
        public int alt;
        /// <summary>
        /// Distance to finish in meters
        /// </summary>
        public int dtf;
    }
}
