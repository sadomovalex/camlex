using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace CamlexNET.Impl.Helpers
{
    internal class ConvertHelper
    {
        public static string ConvertToString(XElement[] elements)
        {
            var sb = new StringBuilder();
            Array.ForEach(elements, e => sb.Append(e.ToString()));
            return sb.ToString();
        }
    }
}
