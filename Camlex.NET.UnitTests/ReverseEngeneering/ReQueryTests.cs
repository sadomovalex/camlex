#region Copyright(c) Alexey Sadomov, Vladimir Timashkov. All Rights Reserved.
// -----------------------------------------------------------------------------
// Copyright(c) 2010 Alexey Sadomov, Vladimir Timashkov. All Rights Reserved.
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using CamlexNET.Impl.ReverseEngeneering;
using CamlexNET.Interfaces;
using CamlexNET.Interfaces.ReverseEngeneering;
using NUnit.Framework;
using Rhino.Mocks;

namespace CamlexNET.UnitTests.ReverseEngeneering
{
    [TestFixture]
    public class ReQueryTests
    {
        [Test]
        public void test_THAT_1_order_by_IS_translated_sucessfully()
        {
            string xml =
                "<Query>" +
                "  <OrderBy>" +
                "    <FieldRef Name=\"Modified\" Ascending=\"False\" />" +
                "  </OrderBy>" +
                "</Query>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("Query().OrderBy(x => (x.get_Item(\"Modified\") As Desc))"));
        }

        [Test]
        public void test_THAT_2_order_by_ARE_translated_sucessfully()
        {
            string xml =
                "<Query>" +
                "  <OrderBy>" +
                "    <FieldRef Name=\"Title\" />" +
                "    <FieldRef Name=\"Modified\" Ascending=\"False\" />" +
                "  </OrderBy>" +
                "</Query>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("Query().OrderBy(x => new [] {x.get_Item(\"Title\"), (x.get_Item(\"Modified\") As Desc)})"));
        }

        [Test]
        public void test_THAT_multiple_groupby_expression_IS_translated_sucessfully()
        {
            var xml =
                "<Query>" +
                "  <GroupBy Collapse=\"True\" GroupLimit=\"10\">" +
                "    <FieldRef Name=\"field1\" />" +
                "    <FieldRef Name=\"field2\" />" +
                "  </GroupBy>" +
                "</Query>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("Query().GroupBy(x => new [] {x.get_Item(\"field1\"), x.get_Item(\"field2\")}, True, 10)"));
        }

        [Test]
        public void test_THAT_in_expression_IS_translated_successfully()
        {
            var xml =
                "<Query>" +
                "  <Where>" +
                "    <In>" +
                "      <FieldRef Name=\"test\" />" +
                "      <Values>" +
                "        <Value Type=\"Text\">test1</Value>" +
                "        <Value Type=\"Text\">test2</Value>" +
                "      </Values>" +
                "    </In>" +
                "  </Where>" +
                "</Query>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("Query().Where(x => new [] {\"test1\", \"test2\"}.Contains(Convert(x.get_Item(\"test\"))))"));
        }

        [Test]
        public void test_THAT_true_boolean_expression_IS_translated_successfully()
        {
            var xml =
                "<Query>" +
                "  <Where>" +
                "    <Eq>" +
                "      <FieldRef Name=\"foo\" />" +
                "      <Value Type=\"Boolean\">1</Value>" +
                "    </Eq>" +
                "  </Where>" +
                "</Query>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("Query().Where(x => Convert(x.get_Item(\"foo\")))"));
        }

        [Test]
        public void test_THAT_false_boolean_expression_IS_translated_successfully()
        {
            var xml =
                "<Query>" +
                "  <Where>" +
                "    <Eq>" +
                "      <FieldRef Name=\"foo\" />" +
                "      <Value Type=\"Boolean\">0</Value>" +
                "    </Eq>" +
                "  </Where>" +
                "</Query>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("Query().Where(x => Not(Convert(x.get_Item(\"foo\"))))"));
        }

        [Test]
        public void test_THAT_mixed_boolean_expression_IS_translated_successfully()
        {
            var xml =
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
                "</Query>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("Query().Where(x => ((Convert(x.get_Item(\"foo1\")) AndAlso Not(Convert(x.get_Item(\"foo2\")))) OrElse Convert(x.get_Item(\"foo3\"))))"));
        }

        [Test]
        public void test_THAT_expression_with_lookup_id_IS_translated_successfully()
        {
            var xml =
                "<Query>" +
                "  <Where>" +
                "    <Eq>" +
                "      <FieldRef Name=\"Title\" LookupId=\"True\" />" +
                "      <Value Type=\"Lookup\">5</Value>" +
                "    </Eq>" +
                "  </Where>" +
                "</Query>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("Query().Where(x => (x.get_Item(\"Title\") == Convert(Convert(\"5\"))))"));
        }

        [Test]
        public void test_THAT_expression_with_lookup_value_IS_translated_successfully()
        {
            var xml =
                "<Query>" +
                "  <Where>" +
                "    <Eq>" +
                "      <FieldRef Name=\"Title\" />" +
                "      <Value Type=\"Lookup\">5</Value>" +
                "    </Eq>" +
                "  </Where>" +
                "</Query>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("Query().Where(x => (x.get_Item(\"Title\") == Convert(Convert(\"5\"))))"));
        }

        [Test]
        public void test_THAT_expression_with_lookup_multi_id_IS_translated_successfully()
        {
            var xml =
                "<Query>" +
                "  <Where>" +
                "    <Eq>" +
                "      <FieldRef Name=\"Title\" LookupId=\"True\" />" +
                "      <Value Type=\"LookupMulti\">5</Value>" +
                "    </Eq>" +
                "  </Where>" +
                "</Query>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("Query().Where(x => (x.get_Item(\"Title\") == Convert(Convert(\"5\"))))"));
        }

        [Test]
        public void test_THAT_expression_with_lookup_multi_value_IS_translated_successfully()
        {
            var xml =
                "<Query>" +
                "  <Where>" +
                "    <Eq>" +
                "      <FieldRef Name=\"Title\" />" +
                "      <Value Type=\"LookupMulti\">5</Value>" +
                "    </Eq>" +
                "  </Where>" +
                "</Query>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("Query().Where(x => (x.get_Item(\"Title\") == Convert(Convert(\"5\"))))"));
        }

        [Test]
        public void test_THAT_expression_with_number_IS_translated_successfully()
        {
            var xml =
                "<Query>" +
                "  <Where>" +
                "    <Eq>" +
                "      <FieldRef Name=\"foo\" />" +
                "      <Value Type=\"Number\">1.23</Value>" +
                "    </Eq>" +
                "  </Where>" +
                "</Query>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("Query().Where(x => (Convert(x.get_Item(\"foo\")) == 1.23))"));
        }

        [Test]
        public void test_THAT_expression_with_includes_IS_translated_successfully()
        {
            string xml =
                "<Query>" +
                "   <Where>" +
                "       <Includes>" +
                "           <FieldRef Name=\"Foo\" />" +
                "           <Value Type=\"Integer\">1</Value>" +
                "       </Includes>" +
                "   </Where>" +
                "</Query>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("Query().Where(x => Convert(Convert(x.get_Item(\"Foo\"))).Includes(Convert(1)))"));
        }

        [Test]
        public void test_THAT_expression_with_includes_with_lookup_id_IS_translated_successfully()
        {
            string xml =
                "<Query>" +
                "   <Where>" +
                "       <Includes>" +
                "           <FieldRef Name=\"Foo\" LookupId=\"True\" />" +
                "           <Value Type=\"Integer\">1</Value>" +
                "       </Includes>" +
                "   </Where>" +
                "</Query>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("Query().Where(x => Convert(Convert(x.get_Item(\"Foo\"))).Includes(Convert(1), True))"));
        }

        [Test]
        public void test_THAT_expression_with_not_includes_IS_translated_successfully()
        {
            string xml =
                "<Query>" +
                "   <Where>" +
                "       <NotIncludes>" +
                "           <FieldRef Name=\"Foo\" />" +
                "           <Value Type=\"Integer\">1</Value>" +
                "       </NotIncludes>" +
                "   </Where>" +
                "</Query>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("Query().Where(x => Not(Convert(Convert(x.get_Item(\"Foo\"))).Includes(Convert(1))))"));
        }

        [Test]
        public void test_THAT_expression_with_not_includes_with_lookup_id_IS_translated_successfully()
        {
            string xml =
                "<Query>" +
                "   <Where>" +
                "       <NotIncludes>" +
                "           <FieldRef Name=\"Foo\" LookupId=\"True\" />" +
                "           <Value Type=\"Integer\">1</Value>" +
                "       </NotIncludes>" +
                "   </Where>" +
                "</Query>";

            var expr = Camlex.QueryFromString(xml).ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("Query().Where(x => Not(Convert(Convert(x.get_Item(\"Foo\"))).Includes(Convert(1), True)))"));
        }
    }
}
