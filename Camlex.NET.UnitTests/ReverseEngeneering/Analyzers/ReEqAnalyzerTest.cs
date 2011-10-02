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
    public class ReEqAnalyzerTest
    {
        class SupportedValueType
        {
            public Type SupportedType { get; set; }
            public string ExampleValue { get; set; }
        }

        List<SupportedValueType> supportedTypesWithExamples = new List<SupportedValueType>
        {
            new SupportedValueType { SupportedType = typeof(DataTypes.Boolean), ExampleValue = bool.TrueString },
            new SupportedValueType { SupportedType = typeof(DataTypes.Boolean), ExampleValue = 1.ToString() },
            new SupportedValueType { SupportedType = typeof(DataTypes.DateTime), ExampleValue = DateTime.Now.ToString() },
            new SupportedValueType { SupportedType = typeof(DataTypes.DateTime), ExampleValue = Camlex.Now },
            new SupportedValueType { SupportedType = typeof(DataTypes.Guid), ExampleValue = Guid.NewGuid().ToString() },
            new SupportedValueType { SupportedType = typeof(DataTypes.Integer), ExampleValue = 12345.ToString() },
            new SupportedValueType { SupportedType = typeof(DataTypes.Lookup), ExampleValue = "lookup" },
            new SupportedValueType { SupportedType = typeof(DataTypes.Text), ExampleValue = "text" }
        };

        [Test]
        public void test_WHEN_xml_is_null_THEN_expression_is_not_valid()
        {
            var analyzer = new ReEqAnalyzer(null, null);
            Assert.IsFalse(analyzer.IsValid());
        }

        [Test]
        public void test_WHEN_neither_field_ref_nor_value_specified_THEN_expression_is_not_valid()
        {
            var xml =
                "  <Eq>" +
                "  </Eq>";

            var analyzer = new ReEqAnalyzer(XmlHelper.Get(xml), null);
            Assert.IsFalse(analyzer.IsValid());
        }

        [Test]
        public void test_WHEN_field_ref_specified_and_value_not_specified_THEN_expression_is_not_valid()
        {
            var xml =
                "<Eq>" +
                "    <FieldRef Name=\"Title\" />" +
                "</Eq>";

            var analyzer = new ReEqAnalyzer(XmlHelper.Get(xml), null);
            Assert.IsFalse(analyzer.IsValid());
        }

        [Test]
        public void test_WHEN_field_ref_not_specified_and_value_specified_THEN_expression_is_not_valid()
        {
            var xml =
                "<Eq>" +
                "    <Value Type=\"Text\">testValue</Value>" +
                "</Eq>";

            var analyzer = new ReEqAnalyzer(XmlHelper.Get(xml), null);
            Assert.IsFalse(analyzer.IsValid());
        }

        [Test]
        public void test_WHEN_field_ref_and_text_value_specified_THEN_expression_is_valid()
        {
            var xml =
                "<Eq>" +
                "    <FieldRef Name=\"Title\" />" +
                "    <Value Type=\"Text\">testValue</Value>" +
                "</Eq>";

            var analyzer = new ReEqAnalyzer(XmlHelper.Get(xml), null);
            Assert.IsTrue(analyzer.IsValid());
        }

        [Test]
        public void test_WHEN_supported_value_type_specified_THEN_expression_is_valid()
        {
            typeof(DataTypes).GetMembers()
                .Where(x => x.MemberType == MemberTypes.NestedType).Select(x => x.Name).ToList().ForEach(x =>
            {
                var foundItem = supportedTypesWithExamples
                    .Where(y => y.SupportedType.Name == x).FirstOrDefault();
                if (foundItem == null) return;

                var xml = string.Format(
                    "<Eq>" +
                    "    <FieldRef Name=\"Title\" />" +
                    "    <Value Type=\"{0}\">{1}</Value>" +
                    "</Eq>",
                    foundItem.SupportedType.Name, foundItem.ExampleValue);

                var analyzer = new ReEqAnalyzer(XmlHelper.Get(xml), null);
                Assert.IsTrue(analyzer.IsValid());
            });
        }

        [Test]
        public void test_WHEN_not_supported_value_type_specified_THEN_expression_is_not_valid()
        {
            typeof(DataTypes).GetMembers()
                .Where(x => x.MemberType == MemberTypes.NestedType).Select(x => x.Name).ToList().ForEach(x =>
                {
                    var foundItem = supportedTypesWithExamples
                        .Where(y => y.SupportedType.Name == x).FirstOrDefault();
                    if (foundItem != null) return;

                    var xml = string.Format(
                        "<Eq>" +
                        "    <FieldRef Name=\"Title\" />" +
                        "    <Value Type=\"{0}\">testValue</Value>" +
                        "</Eq>", x);

                    var analyzer = new ReEqAnalyzer(XmlHelper.Get(xml), null);
                    Assert.IsFalse(analyzer.IsValid());
                });
        }

        [Test]
        [ExpectedException(typeof(CamlAnalysisException))]
        public void test_WHEN_expression_is_not_valid_THEN_exception_is_thrown()
        {
            var analyzer = new ReEqAnalyzer(null, null);
            analyzer.GetOperation();
        }

        [Test]
        public void test_WHEN_expression_is_valid_THEN_operation_is_returned()
        {
            var xml =
                "<Eq>" +
                "    <FieldRef Name=\"Title\" />" +
                "    <Value Type=\"Text\">testValue</Value>" +
                "</Eq>";

            var b = MockRepository.GenerateStub<IReOperandBuilder>();
            b.Stub(c => c.CreateFieldRefOperand(null)).Return(new FieldRefOperand("Title")).IgnoreArguments();
            b.Stub(c => c.CreateValueOperand(null, null)).Return(new TextValueOperand("testValue")).IgnoreArguments();

            var analyzer = new ReEqAnalyzer(XmlHelper.Get(xml), b);
            var operation = analyzer.GetOperation();
            Assert.IsInstanceOf<EqOperation>(operation);
            var eqOperation = (EqOperation)operation;
            Assert.That(eqOperation.ToExpression().ToString(), Is.EqualTo("(Convert(x.get_Item(\"Title\")) = \"testValue\")"));
        }
    }
}