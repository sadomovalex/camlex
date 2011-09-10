using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Xml.Linq;
using CamlexNET.Impl.ReverseEngeneering.Caml;
using Microsoft.SharePoint;
using NUnit.Framework;

namespace CamlexNET.UnitTests.ReverseEngeneering
{
    public static class XmlHelper
    {
        public static XElement Get(string xml)
        {
            using (var tr = new StringReader(xml))
            {
                var doc = XDocument.Load(tr);
                return doc.Descendants().First();
            }
        }
    }

    [TestFixture]
    public class ReLinkerFromCamlTests
    {
        [Test]
        public void test_WHEN_where_is_specified_THEN_expressions_are_linked_correctly()
        {
            var l = new ReLinkerFromCaml(null, null, null, null);
            var expr = l.Link((Expression<Func<SPListItem, bool>>)(x => (int) x["foo"] == 1), null, null, null);
            Assert.That(expr.ToString(), Is.EqualTo("Query().Where(x => (Convert(x.get_Item(\"foo\")) = 1))"));
        }

        [Test]
        public void test_WHEN_order_by_is_specified_THEN_expressions_are_linked_correctly()
        {
            var orderBy =
                "  <OrderBy>" +
                "    <FieldRef Name=\"Title\" Ascending=\"True\" />" +
                "  </OrderBy>";

            var l = new ReLinkerFromCaml(null, XmlHelper.Get(orderBy), null, null);
            var expr = l.Link(null, (Expression<Func<SPListItem, object>>)(x => x["Title"] as Camlex.Asc), null, null);
            Assert.That(expr.ToString(), Is.EqualTo("Query().OrderBy(x => (x.get_Item(\"Title\") As Asc))"));
        }

        [Test]
        public void test_WHEN_order_by_is_specified_but_is_empty_THEN_expression_is_empty()
        {
            var orderBy =
                "  <OrderBy>" +
                "  </OrderBy>";

            var l = new ReLinkerFromCaml(null, XmlHelper.Get(orderBy), null, null);
            var expr = l.Link(null, (Expression<Func<SPListItem, object>>)(x => x["Title"] as Camlex.Asc), null, null);
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

            var l = new ReLinkerFromCaml(null, XmlHelper.Get(orderBy), null, null);
            var expr = l.Link(null, (Expression<Func<SPListItem, object[]>>)(x => new[] { x["Title"], x["Date"] as Camlex.Desc}), null, null);
            Assert.That(expr.ToString(), Is.EqualTo("Query().OrderBy(x => new [] {x.get_Item(\"Title\"), (x.get_Item(\"Date\") As Desc)})"));
        }

        [Test]
        public void test_WHEN_where_and_order_by_are_specified_THEN_expressions_are_linked_correctly()
        {
            var orderBy =
                "  <OrderBy>" +
                "    <FieldRef Name=\"Title\" Ascending=\"True\" />" +
                "  </OrderBy>";

            var l = new ReLinkerFromCaml(null, XmlHelper.Get(orderBy), null, null);
            var expr = l.Link((Expression<Func<SPListItem, bool>>)(x => (int)x["foo"] == 1), (Expression<Func<SPListItem, object>>)(x => x["Title"] as Camlex.Asc), null, null);
            Assert.That(expr.ToString(), Is.EqualTo("Query().Where(x => (Convert(x.get_Item(\"foo\")) = 1)).OrderBy(x => (x.get_Item(\"Title\") As Asc))"));
        }

        [Test]
        public void test_WHEN_group_by_is_specified_THEN_expressions_are_linked_correctly()
        {
            var groupBy =
                "  <GroupBy>" +
                "    <FieldRef Name=\"field1\" />" +
                "  </GroupBy>";

            var l = new ReLinkerFromCaml(null, null, XmlHelper.Get(groupBy), null);
            var expr = l.Link(null, null, (Expression<Func<SPListItem, object>>)(x => x["field1"]), null);
            Assert.That(expr.ToString(), Is.EqualTo("Query().GroupBy(x => x.get_Item(\"field1\"))"));
        }

        [Test]
        public void test_WHEN_group_by_with_collapse_is_specified_THEN_expressions_are_linked_correctly()
        {
            var groupBy =
                "  <GroupBy Collapse=\"True\">" +
                "    <FieldRef Name=\"field1\" />" +
                "  </GroupBy>";

            var l = new ReLinkerFromCaml(null, null, XmlHelper.Get(groupBy), null);
            var expr = l.Link(null, null, (Expression<Func<SPListItem, object>>)(x => x["field1"]), null);
            Assert.That(expr.ToString(), Is.EqualTo("Query().GroupBy(x => x.get_Item(\"field1\"), Convert(True))"));
        }

        [Test]
        public void test_WHEN_group_by_with_group_limit_is_specified_THEN_expressions_are_linked_correctly()
        {
            var groupBy =
                "  <GroupBy GroupLimit=\"1\">" +
                "    <FieldRef Name=\"field1\" />" +
                "  </GroupBy>";

            var l = new ReLinkerFromCaml(null, null, XmlHelper.Get(groupBy), null);
            var expr = l.Link(null, null, (Expression<Func<SPListItem, object>>)(x => x["field1"]), null);
            Assert.That(expr.ToString(), Is.EqualTo("Query().GroupBy(x => x.get_Item(\"field1\"), Convert(1))"));
        }

        [Test]
        public void test_WHEN_group_by_with_collapse_and_group_limit_is_specified_THEN_expressions_are_linked_correctly()
        {
            var groupBy =
                "  <GroupBy Collapse=\"True\" GroupLimit=\"1\">" +
                "    <FieldRef Name=\"field1\" />" +
                "  </GroupBy>";

            var l = new ReLinkerFromCaml(null, null, XmlHelper.Get(groupBy), null);
            var expr = l.Link(null, null, (Expression<Func<SPListItem, object>>)(x => x["field1"]), null);
            Assert.That(expr.ToString(), Is.EqualTo("Query().GroupBy(x => x.get_Item(\"field1\"), Convert(True), Convert(1))"));
        }

        [Test]
        public void test_WHEN_group_by_is_specified_with_several_fied_refs_THEN_expressions_are_linked_correctly()
        {
            var groupBy =
                "  <GroupBy>" +
                "    <FieldRef Name=\"field1\" />" +
                "    <FieldRef Name=\"field2\" />" +
                "  </GroupBy>";

            var l = new ReLinkerFromCaml(null, null, XmlHelper.Get(groupBy), null);
            var expr = l.Link(null, null, (Expression<Func<SPListItem, object[]>>)(x => new[] { x["field1"], x["field2"] }), null);
            Assert.That(expr.ToString(), Is.EqualTo("Query().GroupBy(x => new [] {x.get_Item(\"field1\"), x.get_Item(\"field2\")}, Convert(null), Convert(null))"));
        }

        [Test]
        public void test_WHEN_group_by_with_collapse_is_specified_with_several_fied_refs_THEN_expressions_are_linked_correctly()
        {
            var groupBy =
                "  <GroupBy Collapse=\"True\">" +
                "    <FieldRef Name=\"field1\" />" +
                "    <FieldRef Name=\"field2\" />" +
                "  </GroupBy>";

            var l = new ReLinkerFromCaml(null, null, XmlHelper.Get(groupBy), null);
            var expr = l.Link(null, null, (Expression<Func<SPListItem, object[]>>)(x => new[] { x["field1"], x["field2"] }), null);
            Assert.That(expr.ToString(), Is.EqualTo("Query().GroupBy(x => new [] {x.get_Item(\"field1\"), x.get_Item(\"field2\")}, Convert(True), Convert(null))"));
        }

        [Test]
        public void test_WHEN_group_by_with_group_limit_is_specified_with_several_fied_refs_THEN_expressions_are_linked_correctly()
        {
            var groupBy =
                "  <GroupBy GroupLimit=\"1\">" +
                "    <FieldRef Name=\"field1\" />" +
                "    <FieldRef Name=\"field2\" />" +
                "  </GroupBy>";

            var l = new ReLinkerFromCaml(null, null, XmlHelper.Get(groupBy), null);
            var expr = l.Link(null, null, (Expression<Func<SPListItem, object[]>>)(x => new[] { x["field1"], x["field2"] }), null);
            Assert.That(expr.ToString(), Is.EqualTo("Query().GroupBy(x => new [] {x.get_Item(\"field1\"), x.get_Item(\"field2\")}, Convert(null), Convert(1))"));
        }

        [Test]
        public void test_WHEN_group_by_with_collapse_and_group_limit_is_specified_with_several_fied_refs_THEN_expressions_are_linked_correctly()
        {
            var groupBy =
                "  <GroupBy Collapse=\"True\" GroupLimit=\"1\">" +
                "    <FieldRef Name=\"field1\" />" +
                "    <FieldRef Name=\"field2\" />" +
                "  </GroupBy>";

            var l = new ReLinkerFromCaml(null, null, XmlHelper.Get(groupBy), null);
            var expr = l.Link(null, null, (Expression<Func<SPListItem, object[]>>)(x => new[] { x["field1"], x["field2"] }), null);
            Assert.That(expr.ToString(), Is.EqualTo("Query().GroupBy(x => new [] {x.get_Item(\"field1\"), x.get_Item(\"field2\")}, Convert(True), Convert(1))"));
        }

        [Test]
        public void test_WHEN_view_field_is_specified_THEN_expressions_are_linked_correctly()
        {
            string viewFields =
                "<ViewFields>" +
                    "<FieldRef Name=\"field1\" />" +
                "</ViewFields>";

            var l = new ReLinkerFromCaml(null, null, null, XmlHelper.Get(viewFields));
            var expr = l.Link(null, null, null, (Expression<Func<SPListItem, object>>)(x => x["field1"]));
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

            var l = new ReLinkerFromCaml(null, null, null, XmlHelper.Get(viewFields));
            var expr = l.Link(null, null, null, (Expression<Func<SPListItem, object[]>>)(x => new[] { x["field1"], x["field2"] }));
            Assert.That(expr.ToString(), Is.EqualTo("Query().ViewFields(x => new [] {x.get_Item(\"field1\"), x.get_Item(\"field2\")}, True)"));
        }
    }
}
