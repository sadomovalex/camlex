using System;
using CamlexNET.Impl.Operands;
using CamlexNET.Impl.Operations.Array;
using CamlexNET.Impl.Operations.Eq;
using NUnit.Framework;

namespace CamlexNET.UnitTests.ReverseEngeneering.Operations
{
    [TestFixture]
    public class ArrayOperationTests
    {
        [Test]
        public void test_THAT_array_operation_with_one_operand_IS_converted_to_expression_correctly()
        {
            var op1 = new FieldRefOperand("Status");
            var op = new ArrayOperation(null, op1);
            var expr = op.ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("new [] {x.get_Item(\"Status\")}"));
        }

        [Test]
        public void test_THAT_array_operation_with_two_operands_IS_converted_to_expression_correctly()
        {
            var op1 = new FieldRefOperand("Status");
            var op2 = new FieldRefOperand("Title");
            var op = new ArrayOperation(null, op1, op2);
            var expr = op.ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("new [] {x.get_Item(\"Status\"), x.get_Item(\"Title\")}"));
        }

        [Test]
        public void test_THAT_array_operation_with_two_operands_with_ordering_IS_converted_to_expression_correctly()
        {
            var op1 = new FieldRefOperand("Status");
            var op11 = new FieldRefOperandWithOrdering(op1, new Camlex.Asc());
            var op2 = new FieldRefOperand("Title");
            var op21 = new FieldRefOperandWithOrdering(op2, new Camlex.Desc());
            var op = new ArrayOperation(null, op11, op21);
            var expr = op.ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("new [] {(x.get_Item(\"Status\") As Asc), (x.get_Item(\"Title\") As Desc)}"));
        }
    }
}
