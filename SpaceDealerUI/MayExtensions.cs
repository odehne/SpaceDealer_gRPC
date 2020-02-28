﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace SpaceDealerUI
{
    public static class MyExtensions
    {
        public static string ToDecimalString(this double value)
        {
            return value.ToString("0.##", CultureInfo.InvariantCulture);
        }

        public static string Tabyfy(this string str)
        {
            if (str.Length < 8)
            {
                return str + "\t\t";
            }
            return str + "\t";
        }
    }
}
