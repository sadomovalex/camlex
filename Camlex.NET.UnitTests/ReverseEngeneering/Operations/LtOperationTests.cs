using System;
using CamlexNET.Impl.Operands;
using CamlexNET.Impl.Operations.Eq;
using CamlexNET.Impl.Operations.Geq;
using CamlexNET.Impl.Operations.Gt;
using CamlexNET.Impl.Operations.Lt;
using NUnit.Framework;

namespace CamlexNET.UnitTests.ReverseEngeneering.Operations
{
    [TestFixture]
    public class LtOperationTests
    {
        [Test]
        public void test_THAT_lt_operation_IS_converted_to_expression_correctly()
        {
            var op1 = new FieldRefOperand("Count");
            var op2 = new IntegerValueOperand(1);
            var op = new LtOperation(null, op1, op2);
            var expr = op.ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("(Convert(x.get_Item(\"Count\")) < 1)"));
        }
    }
}
