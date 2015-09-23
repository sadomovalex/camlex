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
using CamlexNET.Impl.ReverseEngeneering.Caml;
using CamlexNET.Impl.ReverseEngeneering.Caml.Analyzers;
using CamlexNET.Impl.ReverseEngeneering.Caml.Factories;
using CamlexNET.Interfaces.ReverseEngeneering;
using NUnit.Framework;

namespace CamlexNET.UnitTests.ReverseEngeneering
{
	[TestFixture]
	public class ReTranslatorFromCamlTests
	{
		[Test]
		public void test_THAT_where_IS_translated_correctly()
		{
			const string xml =
				"<Eq>" +
				"    <FieldRef Name=\"Title\" />" +
				"    <Value Type=\"Text\">testValue</Value>" +
				"</Eq>";

			var b = new ReOperandBuilderFromCaml();
            var t = new ReTranslatorFromCaml(new ReEqAnalyzer(XmlHelper.Get(xml), b), null, null, null, null, null);
			var expr = t.TranslateWhere();
			Assert.That(expr.ToString(), Is.EqualTo("x => (Convert(x.get_Item(\"Title\")) = \"testValue\")"));
		}

		[Test]
		public void test_THAT_rowlimit_by_IS_translated_correctly()
		{
			const string xml = "<RowLimit>10</RowLimit>";

			var b = new ReOperandBuilderFromCaml();
            var t = new ReTranslatorFromCaml(null, null, null, null, new ReConstantAnalyzer(XmlHelper.Get(xml), b, Tags.RowLimit, typeof(int)));
			var expr = t.TranslateRowLimit();
			Assert.That(expr.ToString(), Is.EqualTo("10"));
		}

		[Test]
		public void test_THAT_order_by_IS_translated_correctly()
		{
			const string xml =
				"<OrderBy>" +
				"    <FieldRef Name=\"Modified\" Ascending=\"False\" />" +
				"</OrderBy>";

			var b = new ReOperandBuilderFromCaml();
            var t = new ReTranslatorFromCaml(null, new ReArrayAnalyzer(XmlHelper.Get(xml), b), null, null, null, null);
			var expr = t.TranslateOrderBy();
			Assert.That(expr.ToString(), Is.EqualTo("x => (x.get_Item(\"Modified\") As Desc)"));
		}

		[Test]
		public void test_THAT_group_by_IS_translated_correctly()
		{
			const string xml =
				"<GroupBy>" +
				"    <FieldRef Name=\"field1\" />" +
				"</GroupBy>";

			var b = new ReOperandBuilderFromCaml();
            var t = new ReTranslatorFromCaml(null, null, new ReArrayAnalyzer(XmlHelper.Get(xml), b), null, null, null);
			var g = new GroupByParams();
			var expr = t.TranslateGroupBy(out g);
			Assert.That(expr.ToString(), Is.EqualTo("x => x.get_Item(\"field1\")"));
			Assert.IsFalse(g.HasCollapse);
			Assert.IsFalse(g.HasGroupLimit);
		}

		[Test]
		public void test_THAT_view_fields_ARE_translated_correctly()
		{
			const string xml =
				"<ViewFields>" +
					"<FieldRef Name=\"Title\" />" +
				"</ViewFields>";

			var b = new ReOperandBuilderFromCaml();
            var t = new ReTranslatorFromCaml(null, null, null, new ReArrayAnalyzer(XmlHelper.Get(xml), b), null, null);
			var expr = t.TranslateViewFields();
			Assert.That(expr.ToString(), Is.EqualTo("x => x.get_Item(\"Title\")"));
        }

        [Test]
        public void test_THAT_1_join_ARE_translated_correctly()
        {
            string xml =
                  "<Joins>" +
                    "<Join Type=\"LEFT\" ListAlias=\"Customers\">" +
                      "<Eq>" +
                        "<FieldRef Name=\"CustomerName\" RefType=\"Id\" />" +
                        "<FieldRef List=\"Customers\" Name=\"Id\" />" +
                      "</Eq>" +
                    "</Join>" +
                  "</Joins>";

            var b = new ReOperandBuilderFromCaml();
            var t = new ReTranslatorFromCaml(null, null, null, null, new ReJoinAnalyzer(XmlHelper.Get(xml), b), null);
            var expr = t.TranslateJoins();
            Assert.That(expr[0].Key.ToString(), Is.EqualTo("x => x.get_Item(\"CustomerName\").ForeignList(\"Customers\")"));
            Assert.That(expr[0].Value, Is.EqualTo(JoinType.Left));
        }

        [Test]
        public void test_THAT_2_joins_ARE_translated_correctly()
        {
            string xml =
                  "<Joins>" +
                    "<Join Type=\"LEFT\" ListAlias=\"Customers\">" +
                      "<Eq>" +
                        "<FieldRef Name=\"CustomerName\" RefType=\"Id\" />" +
                        "<FieldRef List=\"Customers\" Name=\"Id\" />" +
                      "</Eq>" +
                    "</Join>" +
                    "<Join Type=\"LEFT\" ListAlias=\"CustomerCities\">" +
                      "<Eq>" +
                        "<FieldRef List=\"Customers\" Name=\"CityName\" RefType=\"Id\" />" +
                        "<FieldRef List=\"CustomerCities\" Name=\"Id\" />" +
                      "</Eq>" +
                    "</Join>" +
                  "</Joins>";

            var b = new ReOperandBuilderFromCaml();
            var t = new ReTranslatorFromCaml(null, null, null, null, new ReJoinAnalyzer(XmlHelper.Get(xml), b), null);
            var expr = t.TranslateJoins();
            Assert.That(expr.Count, Is.EqualTo(2));
            Assert.That(expr[0].Key.ToString(), Is.EqualTo("x => x.get_Item(\"CustomerName\").ForeignList(\"Customers\")"));
            Assert.That(expr[0].Value, Is.EqualTo(JoinType.Left));
            Assert.That(expr[1].Key.ToString(), Is.EqualTo("x => x.get_Item(\"CityName\").PrimaryList(\"Customers\").ForeignList(\"CustomerCities\")"));
            Assert.That(expr[1].Value, Is.EqualTo(JoinType.Left));
		}
	}
}
