using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using CamlexNET.Impl.Operands;
using CamlexNET.Impl.Operations.Array;
using CamlexNET.Impl.Operations.Eq;
using CamlexNET.Impl.ReverseEngeneering.Caml.Analyzers;
using CamlexNET.Interfaces;
using CamlexNET.Interfaces.ReverseEngeneering;
using NUnit.Framework;
using Rhino.Mocks;

namespace CamlexNET.UnitTests.ReverseEngeneering.Analyzers
{
    public class ReIsNullAnalyzerTest
    {
        [Test]
        public void test_WHEN_xml_is_null_THEN_expression_is_not_valid()
        {
            var analyzer = new ReIsNullAnalyzer(null, null);
            Assert.IsFalse(analyzer.IsValid());
        }

        [Test]
        public void test_WHEN_field_ref_not_specified_THEN_expression_is_not_valid()
        {
            var xml =
                "<IsNull>" +
                "</IsNull>";

            var analyzer = new ReIsNullAnalyzer(XmlHelper.Get(xml), null);
            Assert.IsFalse(analyzer.IsValid());
        }

        [Test]
        public void test_WHEN_field_ref_specified_THEN_expression_is_valid()
        {
            var xml =
                "<IsNull>" +
                "    <FieldRef Name=\"Title\" />" +
                "</IsNull>";

            var analyzer = new ReIsNullAnalyzer(XmlHelper.Get(xml), null);
            Assert.IsTrue(analyzer.IsValid());
        }

        [Test]
        public void test_WHEN_field_ref_specified_THEN_is_not_null_expression_is_valid()
        {
            var xml =
                "<IsNotNull>" +
                "    <FieldRef Name=\"Title\" />" +
                "</IsNotNull>";

            var analyzer = new ReIsNotNullAnalyzer(XmlHelper.Get(xml), null);
            Assert.IsTrue(analyzer.IsValid());
        }
    }
}