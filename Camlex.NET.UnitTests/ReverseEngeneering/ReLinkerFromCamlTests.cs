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
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Xml.Linq;
using CamlexNET.Impl.ReverseEngeneering.Caml;
using CamlexNET.Interfaces.ReverseEngeneering;
using Microsoft.SharePoint;
using NUnit.Framework;

namespace CamlexNET.UnitTests.ReverseEngeneering
{
    [TestFixture]
    public class ReLinkerFromCamlTests
    {
        [Test]
        public void test_WHEN_where_is_specified_THEN_expressions_are_linked_correctly()
        {
            var l = new ReLinkerFromCaml(null, null, null, null, null);
            var g = new GroupByParams();
            var expr = l.Link((Expression<Func<SPListItem, bool>>)(x => (int)x["foo"] == 1), null, null, null, null, g);
            Assert.That(expr.ToString(), Is.EqualTo("Query().Where(x => (Convert(x.get_Item(\"foo\")) = 1))"));
        }

        [Test]
        public void test_WHEN_order_by_is_specified_THEN_expressions_are_linked_correctly()
        {
            var orderBy =
                "  <OrderBy>" +
                "    <FieldRef Name=\"Title\" Ascending=\"True\" />" +
                "  </OrderBy>";

            var l = new ReLinkerFromCaml(null, XmlHelper.Get(orderBy), null, null, null);
            var g = new GroupByParams();
            var expr = l.Link(null, (Expression<Func<SPListItem, object>>)(x => x["Title"] as Camlex.Asc), null, null, null, g);
            Assert.That(expr.ToString(), Is.EqualTo("Query().OrderBy(x => (x.get_Item(\"Title\") As Asc))"));
        }

        [Test]
        public void test_WHEN_order_by_is_specified_but_is_empty_THEN_expression_is_empty()
        {
            var orderBy =
                "  <OrderBy>" +
                "  </OrderBy>";

            var l = new ReLinkerFromCaml(null, XmlHelper.Get(orderBy), null, null, null);
            var g = new GroupByParams();
            var expr = l.Link(null, (Expression<Func<SPListItem, object>>)(x => x["Title"] as Camlex.Asc), null, null, null, g);
            Assert.That(expr.ToString(), Is.EqualTo("Query()"));
        }

        [Test]
        public void test_WHEN_several_order_by_are_specified_THEN_expressions_are_linked_correctly()
        {
            var orderBy =
                "  <OrderBy>" +
                "    <FieldRef Name=\"Title\" />" +
                "    <FieldRef Name=\"Date\" Ascending=\"False\" />" +
                "  </OrderBy>";

            var l = new ReLinkerFromCaml(null, XmlHelper.Get(orderBy), null, null, null);
            var g = new GroupByParams();
            var expr = l.Link(null, (Expression<Func<SPListItem, object[]>>)(x => new[] { x["Title"], x["Date"] as Camlex.Desc }), null, null, null, g);
            Assert.That(expr.ToString(), Is.EqualTo("Query().OrderBy(x => new [] {x.get_Item(\"Title\"), (x.get_Item(\"Date\") As Desc)})"));
        }

        [Test]
        public void test_WHEN_where_and_order_by_are_specified_THEN_expressions_are_linked_correctly()
        {
            var orderBy =
                "  <OrderBy>" +
                "    <FieldRef Name=\"Title\" Ascending=\"True\" />" +
                "  </OrderBy>";

            var l = new ReLinkerFromCaml(null, XmlHelper.Get(orderBy), null, null, null);
            var g = new GroupByParams();
            var expr = l.Link((Expression<Func<SPListItem, bool>>)(x => (int)x["foo"] == 1), (Expression<Func<SPListItem, object>>)(x => x["Title"] as Camlex.Asc), null, null, null, g);
            Assert.That(expr.ToString(), Is.EqualTo("Query().Where(x => (Convert(x.get_Item(\"foo\")) = 1)).OrderBy(x => (x.get_Item(\"Title\") As Asc))"));
        }

        [Test]
        public void test_WHEN_group_by_is_specified_THEN_expressions_are_linked_correctly()
        {
            var groupBy =
                "  <GroupBy>" +
                "    <FieldRef Name=\"field1\" />" +
                "  </GroupBy>";

            var l = new ReLinkerFromCaml(null, null, XmlHelper.Get(groupBy), null, null);
            var g = new GroupByParams();
            var expr = l.Link(null, null, (Expression<Func<SPListItem, object>>)(x => x["field1"]), null, null, g);
            Assert.That(expr.ToString(), Is.EqualTo("Query().GroupBy(x => x.get_Item(\"field1\"))"));
        }

        [Test]
        public void test_WHEN_group_by_with_collapse_is_specified_THEN_expressions_are_linked_correctly()
        {
            var groupBy =
                "  <GroupBy Collapse=\"True\">" +
                "    <FieldRef Name=\"field1\" />" +
                "  </GroupBy>";

            var l = new ReLinkerFromCaml(null, null, XmlHelper.Get(groupBy), null, null);
            var g = new GroupByParams { HasCollapse = true, Collapse = true};
            var expr = l.Link(null, null, (Expression<Func<SPListItem, object>>)(x => x["field1"]), null, null, g);
            Assert.That(expr.ToString(), Is.EqualTo("Query().GroupBy(x => x.get_Item(\"field1\"), True)"));
        }

        [Test]
        public void test_WHEN_group_by_with_group_limit_is_specified_THEN_expressions_are_linked_correctly()
        {
            var groupBy =
                "  <GroupBy GroupLimit=\"1\">" +
                "    <FieldRef Name=\"field1\" />" +
                "  </GroupBy>";

            var l = new ReLinkerFromCaml(null, null, XmlHelper.Get(groupBy), null, null);
            var g = new GroupByParams {HasGroupLimit = true, GroupLimit = 1 }; ;
            var expr = l.Link(null, null, (Expression<Func<SPListItem, object>>)(x => x["field1"]), null, null, g);
            Assert.That(expr.ToString(), Is.EqualTo("Query().GroupBy(x => x.get_Item(\"field1\"), 1)"));
        }

        [Test]
        public void test_WHEN_group_by_with_collapse_and_group_limit_is_specified_THEN_expressions_are_linked_correctly()
        {
            var groupBy =
                "  <GroupBy Collapse=\"True\" GroupLimit=\"1\">" +
                "    <FieldRef Name=\"field1\" />" +
                "  </GroupBy>";

            var l = new ReLinkerFromCaml(null, null, XmlHelper.Get(groupBy), null, null);
            var g = new GroupByParams{HasCollapse = true, Collapse = true, HasGroupLimit = true, GroupLimit = 1};
            var expr = l.Link(null, null, (Expression<Func<SPListItem, object>>)(x => x["field1"]), null, null, g);
            Assert.That(expr.ToString(), Is.EqualTo("Query().GroupBy(x => x.get_Item(\"field1\"), True, 1)"));
        }

        [Test]
        public void test_WHEN_group_by_is_specified_with_several_fied_refs_THEN_expressions_are_linked_correctly()
        {
            var groupBy =
                "  <GroupBy>" +
                "    <FieldRef Name=\"field1\" />" +
                "    <FieldRef Name=\"field2\" />" +
                "  </GroupBy>";

            var l = new ReLinkerFromCaml(null, null, XmlHelper.Get(groupBy), null, null);
            var g = new GroupByParams();
            var expr = l.Link(null, null, (Expression<Func<SPListItem, object[]>>)(x => new[] { x["field1"], x["field2"] }), null, null, g);
            Assert.That(expr.ToString(), Is.EqualTo("Query().GroupBy(x => new [] {x.get_Item(\"field1\"), x.get_Item(\"field2\")}, null, null)"));
        }

        [Test]
        public void test_WHEN_group_by_with_collapse_is_specified_with_several_fied_refs_THEN_expressions_are_linked_correctly()
        {
            var groupBy =
                "  <GroupBy Collapse=\"True\">" +
                "    <FieldRef Name=\"field1\" />" +
                "    <FieldRef Name=\"field2\" />" +
                "  </GroupBy>";

            var l = new ReLinkerFromCaml(null, null, XmlHelper.Get(groupBy), null, null);
            var g = new GroupByParams { HasCollapse = true, Collapse = true};
            var expr = l.Link(null, null, (Expression<Func<SPListItem, object[]>>)(x => new[] { x["field1"], x["field2"] }), null, null, g);
            Assert.That(expr.ToString(), Is.EqualTo("Query().GroupBy(x => new [] {x.get_Item(\"field1\"), x.get_Item(\"field2\")}, True, null)"));
        }

        [Test]
        public void test_WHEN_group_by_with_group_limit_is_specified_with_several_fied_refs_THEN_expressions_are_linked_correctly()
        {
            var groupBy =
                "  <GroupBy GroupLimit=\"1\">" +
                "    <FieldRef Name=\"field1\" />" +
                "    <FieldRef Name=\"field2\" />" +
                "  </GroupBy>";

            var l = new ReLinkerFromCaml(null, null, XmlHelper.Get(groupBy), null, null);
            var g = new GroupByParams {HasGroupLimit = true, GroupLimit = 1 };
            var expr = l.Link(null, null, (Expression<Func<SPListItem, object[]>>)(x => new[] { x["field1"], x["field2"] }), null, null, g);
            Assert.That(expr.ToString(), Is.EqualTo("Query().GroupBy(x => new [] {x.get_Item(\"field1\"), x.get_Item(\"field2\")}, null, 1)"));
        }

        [Test]
        public void test_WHEN_group_by_with_collapse_and_group_limit_is_specified_with_several_fied_refs_THEN_expressions_are_linked_correctly()
        {
            var groupBy =
                "  <GroupBy Collapse=\"True\" GroupLimit=\"1\">" +
                "    <FieldRef Name=\"field1\" />" +
                "    <FieldRef Name=\"field2\" />" +
                "  </GroupBy>";

            var l = new ReLinkerFromCaml(null, null, XmlHelper.Get(groupBy), null, null);
            var g = new GroupByParams { HasCollapse = true, Collapse = true, HasGroupLimit = true, GroupLimit = 1 };
            var expr = l.Link(null, null, (Expression<Func<SPListItem, object[]>>)(x => new[] { x["field1"], x["field2"] }), null, null, g);
            Assert.That(expr.ToString(), Is.EqualTo("Query().GroupBy(x => new [] {x.get_Item(\"field1\"), x.get_Item(\"field2\")}, True, 1)"));
        }

        [Test]
        public void test_WHEN_view_field_is_specified_THEN_expressions_are_linked_correctly()
        {
            string viewFields =
                "<ViewFields>" +
                    "<FieldRef Name=\"field1\" />" +
                "</ViewFields>";

            var l = new ReLinkerFromCaml(null, null, null, XmlHelper.Get(viewFields), null);
            var g = new GroupByParams();
            var expr = l.Link(null, null, null, (Expression<Func<SPListItem, object>>)(x => x["field1"]), null, g);
            Assert.That(expr.ToString(), Is.EqualTo("Query().ViewFields(x => x.get_Item(\"field1\"), True)"));
        }

        [Test]
        public void test_WHEN_view_several_fields_are_specified_THEN_expressions_are_linked_correctly()
        {
            string viewFields =
                "<ViewFields>" +
                    "<FieldRef Name=\"field1\" />" +
                    "<FieldRef Name=\"field2\" />" +
                "</ViewFields>";

            var l = new ReLinkerFromCaml(null, null, null, XmlHelper.Get(viewFields), null);
            var g = new GroupByParams();
            var expr = l.Link(null, null, null, (Expression<Func<SPListItem, object[]>>)(x => new[] { x["field1"], x["field2"] }), null, g);
            Assert.That(expr.ToString(), Is.EqualTo("Query().ViewFields(x => new [] {x.get_Item(\"field1\"), x.get_Item(\"field2\")}, True)"));
        }

        [Test]
        [ExpectedException(typeof(OnlyOnePartOfQueryShouldBeNotNullException))]
        public void test_WHEN_fluent_part_and_view_fields_are_specified_THEN_exception_is_thrown()
        {
            var l = new ReLinkerFromCaml(null, null, null, null, null);
            var g = new GroupByParams();
            l.Link((Expression<Func<SPListItem, bool>>)(x => (int)x["foo"] == 1), null, null, (Expression<Func<SPListItem, object[]>>)(x => new[] { x["field1"], x["field2"] }), null, g);
        }

        [Test]
        public void test_WHEN_1_join_is_specified_THEN_expressions_are_linked_correctly()
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

            var l = new ReLinkerFromCaml(null, null, null, null, XmlHelper.Get(xml));
            var g = new GroupByParams();

            Expression<Func<SPListItem, object>> ex = x => x["CustomerName"].ForeignList("Customers");
            var expr = l.Link(null, null, null, null, (new[]{Expression.Lambda(ex)}).ToList(), g);
            Assert.That(expr.ToString(), Is.EqualTo("Query().Joins().Left(x => (x.get_Item(\"CustomerName\").ForeignList(\"Customers\")))"));
        }
    }
}
