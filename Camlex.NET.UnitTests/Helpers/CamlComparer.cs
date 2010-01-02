using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Camlex.NET.UnitTests.Helpers
{
    public class CamlComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            x = this.removeSymbolsBetweenTags(x);
            y = this.removeSymbolsBetweenTags(y);
            return string.Compare(x, y);
        }

        private string removeSymbolsBetweenTags(string s)
        {
            var re = new Regex("<.+?>");
            var sb = new StringBuilder();
            foreach (Match matche in re.Matches(s))
            {
                sb.Append(matche.Value);
            }
            return sb.ToString();
        }
    }
}
