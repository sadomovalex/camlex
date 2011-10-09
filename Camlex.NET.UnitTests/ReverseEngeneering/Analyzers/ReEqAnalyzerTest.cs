using System;
using System.Xml.Linq;
using CamlexNET.Impl.Operations.Eq;
using CamlexNET.Impl.ReverseEngeneering.Caml.Analyzers;
using CamlexNET.Interfaces.ReverseEngeneering;
using NUnit.Framework;

namespace CamlexNET.UnitTests.ReverseEngeneering.Analyzers
{
    internal class ReEqAnalyzerTest : ReBinaryExpressionTestBase<ReEqAnalyzer, EqOperation>
    {
        private readonly Func<XElement, IReOperandBuilder, ReEqAnalyzer> 
            CONSTRUCTOR = (el, operandBuilder) => new ReEqAnalyzer(el, operandBuilder);
        private const string OPERATION = "Eq";

        [Test]
        public void test_WHEN_xml_is_null_THEN_expression_is_not_valid()
        {
            BASE_test_WHEN_xml_is_null_THEN_expression_is_not_valid(CONSTRUCTOR);
        }

        [Test]
        public void test_WHEN_neither_field_ref_nor_value_specified_THEN_expression_is_not_valid()
        {
            BASE_test_WHEN_neither_field_ref_nor_value_specified_THEN_expression_is_not_valid(CONSTRUCTOR, OPERATION);
        }

        [Test]
        public void test_WHEN_field_ref_specified_and_value_not_specified_THEN_expression_is_not_valid()
        {
            BASE_test_WHEN_field_ref_specified_and_value_not_specified_THEN_expression_is_not_valid(CONSTRUCTOR, OPERATION);
        }

        [Test]
        public void test_WHEN_field_ref_not_specified_and_value_specified_THEN_expression_is_not_valid()
        {
            BASE_test_WHEN_field_ref_not_specified_and_value_specified_THEN_expression_is_not_valid(CONSTRUCTOR, OPERATION);
        }

        [Test]
        public void test_WHEN_field_ref_and_text_value_specified_THEN_expression_is_valid()
        {
            BASE_test_WHEN_field_ref_and_text_value_specified_THEN_expression_is_valid(CONSTRUCTOR, OPERATION);
        }

        [Test]
        public void test_WHEN_supported_value_type_specified_THEN_expression_is_valid()
        {
            BASE_test_WHEN_supported_value_type_specified_THEN_expression_is_valid(CONSTRUCTOR, OPERATION);
        }

        [Test]
        public void test_WHEN_not_supported_value_type_specified_THEN_expression_is_not_valid()
        {
            BASE_test_WHEN_not_supported_value_type_specified_THEN_expression_is_not_valid(CONSTRUCTOR, OPERATION);
        }

        [Test]
        [ExpectedException(typeof(CamlAnalysisException))]
        public void test_WHEN_expression_is_not_valid_THEN_exception_is_thrown()
        {
            BASE_test_WHEN_expression_is_not_valid_THEN_exception_is_thrown(CONSTRUCTOR);
        }

        [Test]
        public void test_WHEN_expression_is_valid_THEN_operation_is_returned()
        {
            var operation = BASE_test_WHEN_expression_is_valid_THEN_operation_is_returned(CONSTRUCTOR, OPERATION);
            Assert.That(operation.ToExpression().ToString(), Is.EqualTo("(Convert(x.get_Item(\"Title\")) = \"testValue\")"));
        }
    }
}