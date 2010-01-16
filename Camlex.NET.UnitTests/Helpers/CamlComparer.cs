#region Copyright(c) Alexey Sadomov, Vladimir Timashkov. All Rights Reserved.
// -----------------------------------------------------------------------------
// Copyright(c) 2007 Alexey Sadomov, Vladimir Timashkov. All Rights Reserved.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//
//   1.  No Trademark License - Microsoft Public License (Ms-PL) does not grant you rights to use
//       authors names, logos, or trademarks.
//   2.  If you distribute any portion of the software, you must retain all copyright,
//       patent, trademark, and attribution notices that are present in the software.
//   3.  If you distribute any portion of the software in source code form, you may do
//       so only under this license by including a complete copy of Microsoft Public License (Ms-PL)
//       with your distribution. If you distribute any portion of the software in compiled
//       or object code form, you may only do so under a license that complies with
//       Microsoft Public License (Ms-PL).
//
// The software is licensed "as-is." You bear the risk of using it. The authors
// give no express warranties, guarantees or conditions. You may have additional consumer
// rights under your local laws which this license cannot change. To the extent permitted
// under your local laws, the authors exclude the implied warranties of merchantability,
// fitness for a particular purpose and non-infringement.
// -----------------------------------------------------------------------------
#endregion
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
