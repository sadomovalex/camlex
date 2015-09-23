using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            string xml =
                "       <Eq>" +
                "           <FieldRef Name=\"Title\" />" +
                "           <Value Type=\"Text\">testValue</Value>" +
                "       </Eq>";

            var b = new ReOperandBuilderFromCaml();
            var t = new ReTranslatorFromCaml(new ReEqAnalyzer(XmlHelper.Get(xml), b), null, null, null, null, null);
            var expr = t.TranslateWhere();
            Assert.That(expr.ToString(), Is.EqualTo("x => (Convert(x.get_Item(\"Title\")) = \"testValue\")"));
        }

        [Test]
        public void test_THAT_order_by_IS_translated_correctly()
        {
            string xml =
                "  <OrderBy>" +
                "    <FieldRef Name=\"Modified\" Ascending=\"False\" />" +
                "  </OrderBy>";

            var b = new ReOperandBuilderFromCaml();
            var t = new ReTranslatorFromCaml(null, new ReArrayAnalyzer(XmlHelper.Get(xml), b), null, null, null, null);
            var expr = t.TranslateOrderBy();
            Assert.That(expr.ToString(), Is.EqualTo("x => (x.get_Item(\"Modified\") As Desc)"));
        }

        [Test]
        public void test_THAT_group_by_IS_translated_correctly()
        {
            string xml =
                "  <GroupBy>" +
                "    <FieldRef Name=\"field1\" />" +
                "  </GroupBy>";

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
            string xml =
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
