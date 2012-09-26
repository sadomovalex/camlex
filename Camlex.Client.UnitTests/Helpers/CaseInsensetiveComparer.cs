#region Copyright(c) Alexey Sadomov, Vladimir Timashkov, Stef Heyenrath. All Rights Reserved.
// -----------------------------------------------------------------------------
// Copyright(c) 2010 Alexey Sadomov, Vladimir Timashkov, Stef Heyenrath. All Rights Reserved.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//
//   1. No Trademark License - Microsoft Public License (Ms-PL) does not grant you rights to use
//      authors names, logos, or trademarks.
//   2. If you distribute any portion of the software, you must retain all copyright,
//      patent, trademark, and attribution notices that are present in the software.
//   3. If you distribute any portion of the software in source code form, you may do
//      so only under this license by including a complete copy of Microsoft Public License (Ms-PL)
//      with your distribution. If you distribute any portion of the software in compiled
//      or object code form, you may only do so under a license that complies with
//      Microsoft Public License (Ms-PL).
//   4. The names of the authors may not be used to endorse or promote products
//      derived from this software without specific prior written permission.
//
// The software is licensed "as-is." You bear the risk of using it. The authors
// give no express warranties, guarantees or conditions. You may have additional consumer
// rights under your local laws which this license cannot change. To the extent permitted
// under your local laws, the authors exclude the implied warranties of merchantability,
// fitness for a particular purpose and non-infringement.
// -----------------------------------------------------------------------------
#endregion
using System.Collections.Generic;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace CamlexNET.UnitTests.Helpers
{
	public class CamlComparer : IComparer<string>
	{
		/// <summary>
		/// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other
		/// </summary>
		/// <param name="x">The first object to compare.</param>
		/// <param name="y">The second object to compare.</param>
		/// <returns></returns>
		public int Compare(string x, string y)
		{
			x = this.RemoveSpacesBetweenTags(x.Trim());
			y = this.RemoveSpacesBetweenTags(y.Trim());

			return string.Compare(x, y);
		}

		/// <summary>
		/// Removes the spaces, newlines, returns and tabs between tags.
		/// </summary>
		/// <param name="s">The string.</param>
		/// <returns>formatted string</returns>
		private string RemoveSpacesBetweenTags(string s)
		{
			var regEx = new Regex(">.+?<", RegexOptions.Singleline);

			return regEx.Replace(s, m => string.Format(">{0}<", m.Value.Substring(1, m.Value.Length - 2).Trim(new[] { ' ', '\n', '\r', '\t' })));
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
			Assert.That(s1, Is.EqualTo(s2).Using(new CamlComparer()));
		}

		[Test]
		public void test_THAT_strings_ARE_equal2()
		{
			string s1 = " <eq>1</eq> "; // spaces
			string s2 =
				@"<eq>
					1
				</eq> ";
			Assert.That(s1, Is.EqualTo(s2).Using(new CamlComparer()));
		}

		[Test]
		public void test_THAT_strings_ARE_equal3()
		{
			string s1 = "\t<eq>1</eq>\t"; // tabs
			string s2 =
				@"<eq>
					1
				</eq> ";
			Assert.That(s1, Is.EqualTo(s2).Using(new CamlComparer()));
		}

		[Test]
		public void test_THAT_strings_ARE_equal4()
		{
			string s1 = "\r\n<eq>1</eq>\r\n"; // newline and return
			string s2 =
				@"<eq>
					1
				</eq> ";
			Assert.That(s1, Is.EqualTo(s2).Using(new CamlComparer()));
		}

		[Test]
		public void test_THAT_strings_ARE_equal5()
		{
			string s1 = "\r\n\t <eq>1</eq>\r\n\t "; // mixed
			string s2 =
				@"<eq>
					1
				</eq> ";
			Assert.That(s1, Is.EqualTo(s2).Using(new CamlComparer()));
		}
	}
}