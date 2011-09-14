using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CamlexNET.Impl.Operands;
using CamlexNET.Impl.Operations.Array;
using CamlexNET.Impl.ReverseEngeneering.Caml.Analyzers;
using CamlexNET.Interfaces;
using CamlexNET.Interfaces.ReverseEngeneering;
using NUnit.Framework;
using Rhino.Mocks;

namespace CamlexNET.UnitTests.ReverseEngeneering.Analyzers
{
    [TestFixture]
    public class ReArrayAnalyzerTests
    {
        [Test]
        public void test_WHEN_xml_is_null_THEN_extression_is_not_valid()
        {
            var analyzer = new ReArrayAnalyzer(null, null);
            Assert.IsFalse(analyzer.IsValid());
        }

        [Test]
        public void test_WHEN_no_field_refs_specified_THEN_extression_is_not_valid()
        {
            var xml =
                "  <OrderBy>" +
                "  </OrderBy>";

            var analyzer = new ReArrayAnalyzer(XmlHelper.Get(xml), null);
            Assert.IsFalse(analyzer.IsValid());
        }

        [Test]
        public void test_WHEN_1_field_ref_specified_THEN_extression_is_valid()
        {
            var xml =
                "  <OrderBy>" +
                "    <FieldRef Name=\"Title\" />" +
                "  </OrderBy>";

            var analyzer = new ReArrayAnalyzer(XmlHelper.Get(xml), null);
            Assert.IsTrue(analyzer.IsValid());
        }

        [Test]
        public void test_WHEN_2_field_refs_specified_THEN_extression_is_valid()
        {
            var xml =
                "  <OrderBy>" +
                "    <FieldRef Name=\"Title\" />" +
                "    <FieldRef Name=\"Status\" />" +
                "  </OrderBy>";

            var analyzer = new ReArrayAnalyzer(XmlHelper.Get(xml), null);
            Assert.IsTrue(analyzer.IsValid());
        }

        [Test]
        [ExpectedException(typeof(CamlAnalysisException))]
        public void test_WHEN_expression_is_not_valid_THEN_exception_is_thrown()
        {
            var analyzer = new ReArrayAnalyzer(null, null);
            analyzer.GetOperation();
        }

        [Test]
        public void test_WHEN_expression_is_valid_THEN_operation_is_returned()
        {
            var xml =
                "  <OrderBy>" +
                "    <FieldRef Name=\"Title\" />" +
                "    <FieldRef Name=\"Status\" Ascending=\"True\" />" +
                "  </OrderBy>";

            var b = MockRepository.GenerateStub<IReOperandBuilder>();
            b.Stub(c => c.CreateFieldRefOperand(null)).Return(new FieldRefOperand("1")).IgnoreArguments();
            b.Stub(c => c.CreateFieldRefOperandWithOrdering(null, null)).Return(new FieldRefOperandWithOrdering(new FieldRefOperand("2"), new Camlex.Asc())).IgnoreArguments();

            var analyzer = new ReArrayAnalyzer(XmlHelper.Get(xml), b);
            var operation = analyzer.GetOperation();
            Assert.IsInstanceOf<ArrayOperation>(operation);
            var arrayOperation = (ArrayOperation)operation;
            Assert.That(arrayOperation.ToExpression().ToString(), Is.EqualTo("new [] {x.get_Item(\"1\"), (x.get_Item(\"2\") As Asc)}"));
        }
    }
}
