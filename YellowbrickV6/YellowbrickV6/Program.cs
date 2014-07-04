using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace YellowbrickV6
{
    class Program
    {
        static void Main(string[] args)
        {
            Random rnd = new Random();
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("http://yb.tl/BIN/shtp2014/AllPositions?r=" + rnd.NextDouble());
            HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
            Stream responseStream = webResponse.GetResponseStream();
            byte[] buffer = ByteTools.GetStreamBytes(responseStream);

            short confByte = buffer[0];

            var doAltitude = ((confByte & 0x01) == 0x01);
            var doDTF = ((confByte & 0x02) == 0x02);

            var startOff = buffer.getUInt32(1);

            var cursor = 5;

            while (cursor < buffer.Length)
            {
                //read first 2 bytes - this is the team id
                var teamId = buffer.getUInt16(cursor);
                cursor += 2;

                var teamMoments = new List<Moment>();

                var numberOfPositions = buffer.getUInt16(cursor);
                cursor += 2;

                Moment lastMoment = new Moment();

                for (var i = 0; i < numberOfPositions; i++)
                {
                    var firstByte = buffer.getUInt8(cursor);
                    
                    Moment moment = new Moment();
                    
                    if ((firstByte & 0x80) == 0x80)
                    {// relative mode
                        var atDelta = buffer.getUInt16(cursor);
                        cursor += 2;
                        var latDelta = buffer.getInt16(cursor);
                        cursor += 2;
                        var lonDelta = buffer.getInt16(cursor);
                        cursor += 2;

                        if (doAltitude)
                        {
                            moment.alt = buffer.getInt16(cursor);
                            cursor += 2;
                        }

                        if (doDTF)
                        {
                            // expecting a 2-byte short, in metres
                            var dtfDelta = buffer.getInt16(cursor);
                            cursor += 2;
                            moment.dtf = lastMoment.dtf + dtfDelta;
                        }

                        atDelta = atDelta & 0x7fff;

                        moment.lat = lastMoment.lat + ((double)latDelta/100000);
                        moment.lon = lastMoment.lon + ((double)lonDelta/100000);
                        moment.at = lastMoment.at - atDelta;

                    }
                    else
                    {//absolute mode
                        var at = buffer.getUInt32(cursor);
                        cursor += 4;
                        var lat = buffer.getInt32(cursor);
                        cursor += 4;
                        var lon = buffer.getInt32(cursor);
                        cursor += 4;

                        if (doAltitude)
                        {
                            moment.alt = buffer.getInt16(cursor);
                            cursor += 2;
                        }

                        if (doDTF)
                        {
                            // expecting a 4-byte int, in metres
                            var dtf = buffer.getInt32(cursor);
                            cursor += 4;
                            moment.dtf = dtf;
                        }

                        moment.lat = (double)lat / 100000;
                        moment.lon = (double)lon / 100000;
                        moment.at = startOff + at;
                    }

                    teamMoments.Add(moment);
                    lastMoment = moment;
                }
            }

        }
    }
}
