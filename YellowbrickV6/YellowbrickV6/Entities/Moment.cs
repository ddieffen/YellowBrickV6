using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YellowbrickV6.Entities
{
    [Serializable]
    public class Moment
    {
        /// <summary>
        /// Latitude positive to north
        /// </summary>
        public double lat { get; set; }
        /// <summary>
        /// Longiture positive to ease
        /// </summary>
        public double lon { get; set; }
        /// <summary>
        /// Unix Time at which the measurement was taken
        /// </summary>
        public uint at { get; set; }
        /// <summary>
        /// Altitude in meters
        /// </summary>
        public int alt { get; set; }
        /// <summary>
        /// Distance to finish in meters
        /// </summary>
        public int dtf { get; set; }
        /// <summary>
        /// Distance in nautical miles calculated from previoulsy known moment
        /// </summary>
        public double dist { get; set; }
        /// <summary>
        /// Speed in knots calculated from previously known moment
        /// </summary>
        public double spd { get; set; }
        /// <summary>
        /// Heading in degrees calculated from previously known moment
        /// </summary>
        public double heading { get; set; }

        public override string ToString()
        {
            return at.ToString();
        }
    }
}
