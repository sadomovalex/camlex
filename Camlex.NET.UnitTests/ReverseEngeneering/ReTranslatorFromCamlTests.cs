using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CamlexNET.Impl.ReverseEngeneering.Caml;
using CamlexNET.Impl.ReverseEngeneering.Caml.Analyzers;
using CamlexNET.Impl.ReverseEngeneering.Caml.Factories;
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
            var t = new ReTranslatorFromCaml(new ReEqAnalyzer(XmlHelper.Get(xml), b), null, null, null);
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
            var t = new ReTranslatorFromCaml(null, new ReArrayAnalyzer(XmlHelper.Get(xml), b), null, null);
            var expr = t.TranslateOrderBy();
            Assert.That(expr.ToString(), Is.EqualTo("x => new [] {(x.get_Item(\"Modified\") As Desc)}"));
        }

        [Test]
        public void test_THAT_group_by_IS_translated_correctly()
        {
            string xml =
                "  <GroupBy>" +
                "    <FieldRef Name=\"field1\" />" +
                "  </GroupBy>";

            var b = new ReOperandBuilderFromCaml();
            var t = new ReTranslatorFromCaml(null, null, new ReArrayAnalyzer(XmlHelper.Get(xml), b), null);
            var expr = t.TranslateGroupBy();
            Assert.That(expr.ToString(), Is.EqualTo("x => new [] {x.get_Item(\"field1\")}"));
        }

        [Test]
        public void test_THAT_view_fields_ARE_translated_correctly()
        {
            string xml =
                "<ViewFields>" +
                    "<FieldRef Name=\"Title\" />" +
                "</ViewFields>";

            var b = new ReOperandBuilderFromCaml();
            var t = new ReTranslatorFromCaml(null, null, null, new ReArrayAnalyzer(XmlHelper.Get(xml), b));
            var expr = t.TranslateViewFields();
            Assert.That(expr.ToString(), Is.EqualTo("x => new [] {x.get_Item(\"Title\")}"));
        }
    }
}
