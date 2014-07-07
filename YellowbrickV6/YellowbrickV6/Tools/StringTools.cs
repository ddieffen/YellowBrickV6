﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YellowbrickV6
{
    internal static class StringTools
    {
        internal static string Lenght(this string str, int maxChar)
        {
            if (str.Length > maxChar)
                return str.Substring(0, maxChar);
            else
            {
                string completed = str;
                for (int i = str.Length; i < maxChar; i++)
                    completed += " ";
                return completed;
            }
        }
    }
}
