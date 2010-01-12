using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CamlexNET
{
    public static class ExtensionMethods
    {
        /// <summary>Marker method toindicate that DateTime value should have IncludeTimeValue attribute</summary>
        /// <param name="dateTime">DateTime value</param>
        /// <returns>Not modified DateTime value</returns>
        public static DateTime IncludeTimeValue(this DateTime dateTime) { return dateTime; }
    }
}
