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
using NUnit.Framework;

namespace CamlexNET.UnitTests.ReverseEngeneering
{
	[TestFixture]
	public class ReQueryTests
	{
		[Test]
		public void test_THAT_1_order_by_IS_translated_sucessfully()
		{
			const string xml =
				"<View>" +
				"    <Query>" +
				"        <OrderBy>" +
				"            <FieldRef Name=\"Modified\" Ascending=\"False\" />" +
				"        </OrderBy>" +
				"    </Query>" +
				"</View>";

			var expr = Camlex.QueryFromString(xml).ToExpression();
			Assert.That(expr.ToString(), Is.EqualTo("Query().OrderBy(x => (x.get_Item(\"Modified\") As Desc))"));
		}

		[Test]
		public void test_THAT_2_order_by_ARE_translated_sucessfully()
		{
			const string xml =
				"<View>" +
				"	<Query>" +
				"		<OrderBy>" +
				"			<FieldRef Name=\"Title\" />" +
				"			<FieldRef Name=\"Modified\" Ascending=\"False\" />" +
				"		</OrderBy>" +
				"	</Query>" +
				"</View>";

			var expr = Camlex.QueryFromString(xml).ToExpression();
			Assert.That(expr.ToString(), Is.EqualTo("Query().OrderBy(x => new [] {x.get_Item(\"Title\"), (x.get_Item(\"Modified\") As Desc)})"));
		}

		[Test]
		public void test_THAT_multiple_groupby_expression_IS_translated_sucessfully()
		{
			const string xml =
				"<View>" +
				"    <Query>" +
				"        <GroupBy Collapse=\"True\" GroupLimit=\"10\">" +
				"            <FieldRef Name=\"field1\" />" +
				"			<FieldRef Name=\"field2\" />" +
				"		</GroupBy>" +
				"	</Query>" +
				"</View>";

			var expr = Camlex.QueryFromString(xml).ToExpression();
			Assert.That(expr.ToString(), Is.EqualTo("Query().GroupBy(x => new [] {x.get_Item(\"field1\"), x.get_Item(\"field2\")}, True, 10)"));
		}

        [Test]
        public void test_THAT_query_with_all_elements_IS_translated_sucessfully()
        {
            const string xml =
                "<View>" +
                "    <Query>" +
				"       <Where>" +
				"           <Eq>" +
				"               <FieldRef Name=\"Title\" />" +
				"               <Value Type=\"Text\">testValue</Value>" +
				"           </Eq>" +
				"       </Where>" +
                "		<OrderBy>" +
                "			<FieldRef Name=\"Title\" />" +
                "			<FieldRef Name=\"Modified\" Ascending=\"False\" />" +
                "		</OrderBy>" +
                "        <GroupBy Collapse=\"True\" GroupLimit=\"10\">" +
                "            <FieldRef Name=\"field1\" />" +
                "			<FieldRef Name=\"field2\" />" +
                "		</GroupBy>" +
                "       <ViewFields>" +
                "           <FieldRef Name=\"Modified\" />" +
                "           <FieldRef Name=\"Title\" />" +
                "       </ViewFields>" +
                "	</Query>" +
				"	<RowLimit>10</RowLimit>" +
                "</View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("Query().Where(x => (Convert(x.get_Item(\"Title\")) == \"testValue\")).OrderBy(x => new [] {x.get_Item(\"Title\"), (x.get_Item(\"Modified\") As Desc)}).GroupBy(x => new [] {x.get_Item(\"field1\"), x.get_Item(\"field2\")}, True, 10).ViewFields(x => new [] {x.get_Item(\"Modified\"), x.get_Item(\"Title\")}).Take(10)"));
        }
        
        [Test]
        public void test_THAT_in_expression_IS_translated_sucessfully()
        {
            var xml =
                "<View>" +
                "  <Query>" +
                "    <Where>" +
                "      <In>" +
                "        <FieldRef Name=\"test\" />" +
                "        <Values>" +
                "          <Value Type=\"Text\">test1</Value>" +
                "          <Value Type=\"Text\">test2</Value>" +
                "        </Values>" +
                "      </In>" +
                "    </Where>" +
                "  </Query>" +
                "</View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("Query().Where(x => new [] {\"test1\", \"test2\"}.Contains(Convert(x.get_Item(\"test\"))))"));
        }

        [Test]
        public void test_THAT_true_boolean_expression_IS_translated_successfully()
        {
            var xml =
                "<View>" +
                "<Query>" +
                "  <Where>" +
                "    <Eq>" +
                "      <FieldRef Name=\"foo\" />" +
                "      <Value Type=\"Boolean\">1</Value>" +
                "    </Eq>" +
                "  </Where>" +
                "</Query>" +
                "</View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("Query().Where(x => Convert(x.get_Item(\"foo\")))"));
        }

        [Test]
        public void test_THAT_false_boolean_expression_IS_translated_successfully()
        {
            var xml =
                "<View>" +
                "<Query>" +
                "  <Where>" +
                "    <Eq>" +
                "      <FieldRef Name=\"foo\" />" +
                "      <Value Type=\"Boolean\">0</Value>" +
                "    </Eq>" +
                "  </Where>" +
                "</Query>" +
                "</View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("Query().Where(x => Not(Convert(x.get_Item(\"foo\"))))"));
        }

        [Test]
        public void test_THAT_mixed_boolean_expression_IS_translated_successfully()
        {
            var xml =
                "<View>" +
                "<Query>" +
                "   <Where>" +
                "       <Or>" +
                "           <And>" +
                "               <Eq>" +
                "                   <FieldRef Name=\"foo1\" />" +
                "                   <Value Type=\"Boolean\">1</Value>" +
                "               </Eq>" +
                "               <Eq>" +
                "                   <FieldRef Name=\"foo2\" />" +
                "                   <Value Type=\"Boolean\">0</Value>" +
                "               </Eq>" +
                "           </And>" +
                "           <Eq>" +
                "               <FieldRef Name=\"foo3\" />" +
                "               <Value Type=\"Boolean\">1</Value>" +
                "           </Eq>" +
                "       </Or>" +
                "   </Where>" +
                "</Query>" +
                "</View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("Query().Where(x => ((Convert(x.get_Item(\"foo1\")) AndAlso Not(Convert(x.get_Item(\"foo2\")))) OrElse Convert(x.get_Item(\"foo3\"))))"));
        }

        [Test]
        public void test_THAT_expression_with_lookup_id_IS_translated_successfully()
        {
            var xml =
                "<View>" +
                "<Query>" +
                "  <Where>" +
                "    <Eq>" +
                "      <FieldRef Name=\"Title\" LookupId=\"True\" />" +
                "      <Value Type=\"Lookup\">5</Value>" +
                "    </Eq>" +
                "  </Where>" +
                "</Query>" +
                "</View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("Query().Where(x => (x.get_Item(\"Title\") == Convert(Convert(\"5\"))))"));
        }

        [Test]
        public void test_THAT_expression_with_lookup_value_IS_translated_successfully()
        {
            var xml =
                "<View>" +
                "<Query>" +
                "  <Where>" +
                "    <Eq>" +
                "      <FieldRef Name=\"Title\" />" +
                "      <Value Type=\"Lookup\">5</Value>" +
                "    </Eq>" +
                "  </Where>" +
                "</Query>" +
                "</View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("Query().Where(x => (x.get_Item(\"Title\") == Convert(Convert(\"5\"))))"));
        }

        [Test]
        public void test_THAT_expression_with_lookup_multi_id_IS_translated_successfully()
        {
            var xml =
                "<View>" +
                "<Query>" +
                "  <Where>" +
                "    <Eq>" +
                "      <FieldRef Name=\"Title\" LookupId=\"True\" />" +
                "      <Value Type=\"LookupMulti\">5</Value>" +
                "    </Eq>" +
                "  </Where>" +
                "</Query>" +
                "</View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("Query().Where(x => (x.get_Item(\"Title\") == Convert(Convert(\"5\"))))"));
        }

        [Test]
        public void test_THAT_expression_with_lookup_multi_value_IS_translated_successfully()
        {
            var xml =
                "<View>" +
                "<Query>" +
                "  <Where>" +
                "    <Eq>" +
                "      <FieldRef Name=\"Title\" />" +
                "      <Value Type=\"LookupMulti\">5</Value>" +
                "    </Eq>" +
                "  </Where>" +
                "</Query>" +
                "</View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("Query().Where(x => (x.get_Item(\"Title\") == Convert(Convert(\"5\"))))"));
        }

        [Test]
        public void test_THAT_expression_with_number_IS_translated_successfully()
        {
            var xml =
                "<View>" +
                "<Query>" +
                "  <Where>" +
                "    <Eq>" +
                "      <FieldRef Name=\"foo\" />" +
                "      <Value Type=\"Number\">1.23</Value>" +
                "    </Eq>" +
                "  </Where>" +
                "</Query>" +
                "</View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("Query().Where(x => (Convert(x.get_Item(\"foo\")) == 1.23))"));
        }

        [Test]
        public void test_THAT_expression_with_includes_IS_translated_successfully()
        {
            string xml =
                "<View>" +
                "<Query>" +
                "   <Where>" +
                "       <Includes>" +
                "           <FieldRef Name=\"Foo\" />" +
                "           <Value Type=\"Integer\">1</Value>" +
                "       </Includes>" +
                "   </Where>" +
                "</Query>"+
                "</View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("Query().Where(x => Convert(Convert(x.get_Item(\"Foo\"))).Includes(Convert(1)))"));
        }

        [Test]
        public void test_THAT_expression_with_includes_with_lookup_id_IS_translated_successfully()
        {
            string xml =
                "<View>" +
                "<Query>" +
                "   <Where>" +
                "       <Includes>" +
                "           <FieldRef Name=\"Foo\" LookupId=\"True\" />" +
                "           <Value Type=\"Integer\">1</Value>" +
                "       </Includes>" +
                "   </Where>" +
                "</Query>" +
                "</View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("Query().Where(x => Convert(Convert(x.get_Item(\"Foo\"))).Includes(Convert(1), True))"));
        }

        [Test]
        public void test_THAT_expression_with_not_includes_IS_translated_successfully()
        {
            string xml =
                "<View>" +
                "<Query>" +
                "   <Where>" +
                "       <NotIncludes>" +
                "           <FieldRef Name=\"Foo\" />" +
                "           <Value Type=\"Integer\">1</Value>" +
                "       </NotIncludes>" +
                "   </Where>" +
                "</Query>" +
                "</View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("Query().Where(x => Not(Convert(Convert(x.get_Item(\"Foo\"))).Includes(Convert(1))))"));
        }

        [Test]
        public void test_THAT_expression_with_not_includes_with_lookup_id_IS_translated_successfully()
        {
            string xml =
                "<View>" +
                "<Query>" +
                "   <Where>" +
                "       <NotIncludes>" +
                "           <FieldRef Name=\"Foo\" LookupId=\"True\" />" +
                "           <Value Type=\"Integer\">1</Value>" +
                "       </NotIncludes>" +
                "   </Where>" +
                "</Query>" +
                "</View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("Query().Where(x => Not(Convert(Convert(x.get_Item(\"Foo\"))).Includes(Convert(1), True)))"));
        }

        [Test]
        public void test_THAT_datetime_with_includetimevalue_IS_translated_sucessfully()
        {
            string xml =
                "<View>" +
                "<Query>" +
                "  <Where>" +
                "    <Gt>" +
                "        <FieldRef Name=\"Created\" />" +
                "        <Value Type=\"DateTime\" IncludeTimeValue=\"True\">2021-05-18T17:31:18Z</Value>" +
                "    </Gt>" +
                "  </Where>" +
                "</Query>" +
                "</View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("Query().Where(x => (Convert(x.get_Item(\"Created\")) > 5/18/2021 5:31:18 PM.IncludeTimeValue()))"));
        }

        [Test]
        public void test_THAT_datetime_with_today_IS_translated_sucessfully()
        {
            string xml =
                "<View>" +
                "<Query>" +
                "  <Where>" +
                "    <Gt>" +
                "        <FieldRef Name=\"Created\" />" +
                "        <Value Type=\"DateTime\"><Today /></Value>" +
                "    </Gt>" +
                "  </Where>" +
                "</Query>" +
                "</View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("Query().Where(x => (x.get_Item(\"Created\") > Convert(Convert(Camlex.Today))))"));
        }

        [Test]
        public void test_THAT_datetime_with_today_and_offsetdays_IS_translated_sucessfully()
        {
            string xml =
                "<View>" +
                "<Query>" +
                "  <Where>" +
                "    <Gt>" +
                "        <FieldRef Name=\"Created\" />" +
                "        <Value Type=\"DateTime\"><Today OffsetDays=\"5\"/></Value>" +
                "    </Gt>" +
                "  </Where>" +
                "</Query>" +
                "</View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("Query().Where(x => (x.get_Item(\"Created\") > Convert(Convert(Camlex.Today)).OffsetDays(5)))"));
        }

        [Test]
        public void test_THAT_datetime_with_includetimevalue_and_storagetz_IS_translated_sucessfully()
        {
            string xml =
                "<View>" +
                "<Query>" +
                "  <Where>" +
                "    <Gt>" +
                "        <FieldRef Name=\"Created\" />" +
                "        <Value Type=\"DateTime\" IncludeTimeValue=\"True\" StorageTZ=\"True\">2021-05-18T17:31:18Z</Value>" +
                "    </Gt>" +
                "  </Where>" +
                "</Query>" +
                "</View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("Query().Where(x => (Convert(x.get_Item(\"Created\")) > 5/18/2021 5:31:18 PM.IncludeTimeValue(True)))"));
        }

        [Test]
        public void test_THAT_expression_with_string_greater_IS_translated_successfully()
        {
            var xml =
                "<View>" +
                "<Query>" +
                "  <Where>" +
                "    <Gt>" +
                "      <FieldRef Name=\"Foo\" />" +
                "      <Value Type=\"Text\">Test</Value>" +
                "    </Gt>" +
                "  </Where>" +
                "</Query>" +
                "</View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("Query().Where(x => (x.get_Item(\"Foo\") > Convert(Convert(\"Test\"))))"));
        }

        [Test]
        public void test_THAT_expression_with_string_greaterORequals_IS_translated_successfully()
        {
            var xml =
                "<View>" +
                "<Query>" +
                "  <Where>" +
                "    <Geq>" +
                "      <FieldRef Name=\"Foo\" />" +
                "      <Value Type=\"Text\">Test</Value>" +
                "    </Geq>" +
                "  </Where>" +
                "</Query>" +
                "</View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("Query().Where(x => (x.get_Item(\"Foo\") >= Convert(Convert(\"Test\"))))"));
        }

        [Test]
        public void test_THAT_expression_with_string_less_IS_translated_successfully()
        {
            var xml =
                "<View>" +
                "<Query>" +
                "  <Where>" +
                "    <Lt>" +
                "      <FieldRef Name=\"Foo\" />" +
                "      <Value Type=\"Text\">Test</Value>" +
                "    </Lt>" +
                "  </Where>" +
                "</Query>" +
                "</View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("Query().Where(x => (x.get_Item(\"Foo\") < Convert(Convert(\"Test\"))))"));
        }

        [Test]
        public void test_THAT_expression_with_string_lessORequals_IS_translated_successfully()
        {
            var xml =
                "<View>" +
                "<Query>" +
                "  <Where>" +
                "    <Leq>" +
                "      <FieldRef Name=\"Foo\" />" +
                "      <Value Type=\"Text\">Test</Value>" +
                "    </Leq>" +
                "  </Where>" +
                "</Query>" +
                "</View>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("Query().Where(x => (x.get_Item(\"Foo\") <= Convert(Convert(\"Test\"))))"));
        }
    }
}
