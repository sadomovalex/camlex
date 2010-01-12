using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace CamlexNET.UnitTests.Helpers
{
    public class CamlComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            x = this.removeSpacesBetweenTags(x.Trim());
            y = this.removeSpacesBetweenTags(y.Trim());
            return string.Compare(x, y);
        }

        private string removeSpacesBetweenTags(string s)
        {
            var re = new Regex(">.+?<", RegexOptions.Singleline);
            string result = re.Replace(s, m => string.Format(">{0}<", m.Value.Substring(1, m.Value.Length - 2).Trim(new [] {' ', '\n', '\r'})));
            return result;
        }
    }

    [TestFixture]
    public class CamlComparerTests
    {
        [Test]
        public void test_THAT_strings_ARE_equal1()
        {
            string s1 = "<eq>1</eq>";
            string s2 =
                @"<eq>
                    1
                </eq>";
            Assert.That(new CamlComparer().Compare(s1, s2), Is.EqualTo(0));
        }

        [Test]
        public void test_THAT_strings_ARE_equal2()
        {
            string s1 = " <eq>1</eq>";
            string s2 =
                @"<eq>
                    1
                </eq> ";
            Assert.That(new CamlComparer().Compare(s1, s2), Is.EqualTo(0));
        }
    }
}
