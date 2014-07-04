using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace YellowbrickV6
{
    internal static class ByteTools
    {
        internal static uint getUInt8(this byte[] array, int position, bool little = false)
        {
           return (uint)array[position];
        }

        internal static uint getUInt16(this byte[] array, int position, bool little = false)
        {
            if (!little)
                return (uint)(array[position] << 8 | array[position + 1]);
            else
                return (uint)(array[position + 1] << 8 | array[position]);
        }

        internal static uint getUInt32(this byte[] array, int position, bool little = false)
        {
            if (!little)
                return (uint)(array[position] << 24 | array[position + 1] << 16 | array[position + 2] << 8 | array[position + 3]);
            else
                return (uint)(array[position + 3] << 24 | array[position + 2] << 16 | array[position + 1] << 8 | array[position]);
        }

        internal static int getInt8(this byte[] array, int position, bool little = false)
        {
            return -(int)(~(array[position])+1);
        }

        internal static int getInt16(this byte[] array, int position, bool little = false)
        {
            if (!little)
                return -(int)(~(array[position] << 8 | array[position + 1])+1);
            else
                return -(int)(~(array[position + 1]  << 8 | array[position])+1);
        }

        internal static int getInt32(this byte[] array, int position, bool little = false)
        {
             if (!little)//need to work on this Javascrip uses 2's completement signed integers
                 return -(int)(~((array[position] << 24 | array[position + 1] << 16 | array[position + 2] << 8 | array[position + 3])) + 1);
            else
                return -(int)(~((array[position + 3] << 24 | array[position + 2] << 16 | array[position + 1] << 8 | array[position] & 0xFF)) +1);
        }

        //floats are 32-bit IEEE

        internal static byte[] GetStreamBytes(Stream stream)
        {
            try
            {
                stream.Position = 0;
            }
            catch
            {
            }

            byte[] readBuffer = new byte[1024];
            List<byte> outputBytes = new List<byte>();

            int offset = 0;

            while (true)
            {
                int bytesRead = stream.Read(readBuffer, 0, readBuffer.Length);

                if (bytesRead == 0)
                {
                    break;
                }
                else if (bytesRead == readBuffer.Length)
                {
                    outputBytes.AddRange(readBuffer);
                }
                else
                {
                    byte[] tempBuf = new byte[bytesRead];

                    Array.Copy(readBuffer, tempBuf, bytesRead);

                    outputBytes.AddRange(tempBuf);

                    break;
                }

                offset += bytesRead;
            }

            return outputBytes.ToArray();
        }

    }
}
