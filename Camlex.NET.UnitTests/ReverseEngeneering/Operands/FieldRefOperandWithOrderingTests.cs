using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CamlexNET.Impl.Operands;
using NUnit.Framework;

namespace CamlexNET.UnitTests.ReverseEngeneering.Operands
{
    [TestFixture]
    public class FieldRefOperandWithOrderingTests
    {
        [Test]
        public void test_THAT_field_ref_operand_with_asc_IS_converted_to_expression_correctly()
        {
            var op = new FieldRefOperand("Title");
            var opOrder = new FieldRefOperandWithOrdering(op, new Camlex.Asc());
            var expr = opOrder.ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("(x.get_Item(\"Title\") As Asc)"));
            // problem 1 - Asc is not accessible. Only Camlex.Asc can be used
            // problem 2 - As instead of as is used. It won't be compiled
        }

        [Test]
        public void test_THAT_field_ref_operand_with_desc_IS_converted_to_expression_correctly()
        {
            var op = new FieldRefOperand("Title");
            var opOrder = new FieldRefOperandWithOrdering(op, new Camlex.Desc());
            var expr = opOrder.ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("(x.get_Item(\"Title\") As Desc)"));
        }

        [Test]
        public void test_THAT_field_ref_operand_with_none_IS_converted_to_expression_correctly()
        {
            var op = new FieldRefOperand("Title");
            var opOrder = new FieldRefOperandWithOrdering(op, new Camlex.None());
            var expr = opOrder.ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("x.get_Item(\"Title\")"));
        }
    }
}
