using System;
using CamlexNET.Impl.Operands;
using CamlexNET.Impl.Operations.Eq;
using CamlexNET.Impl.Operations.Neq;
using NUnit.Framework;

namespace CamlexNET.UnitTests.ReverseEngeneering.Operations
{
    [TestFixture]
    public class NeqOperationTests
    {
        [Test]
        public void test_THAT_neq_operation_with_bool_IS_converted_to_expression_correctly()
        {
            var op1 = new FieldRefOperand("Status");
            var op2 = new BooleanValueOperand(true);
            var op = new NeqOperation(null, op1, op2);
            var expr = op.ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("(Convert(x.get_Item(\"Status\")) != True)"));
        }
    }
}
