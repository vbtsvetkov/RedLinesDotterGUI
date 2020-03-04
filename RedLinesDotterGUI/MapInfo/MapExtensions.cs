using System;

namespace RedLinesDotterGUI.MapInfo
{
    internal static class MapExtensions
    {
        internal static int ToInt(this string value)
        {
            return Convert.ToInt32(value);
        }

        internal static double ToDouble(this string value)
        {
            return Convert.ToDouble(value, System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
