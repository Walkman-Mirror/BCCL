using System;
using System.Globalization;

namespace BCCL.Utility
{
    public static class Format
    {
        public static bool IsNumeric(this string val, NumberStyles numberStyle)
        {
            Int32 result;
            return Int32.TryParse(val, numberStyle,
                                  CultureInfo.CurrentCulture, out result);
        }
    }
}