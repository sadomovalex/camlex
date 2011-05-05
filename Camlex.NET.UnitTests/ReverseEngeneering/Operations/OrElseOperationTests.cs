using System;
using CamlexNET.Impl.Operands;
using CamlexNET.Impl.Operations.AndAlso;
using CamlexNET.Impl.Operations.Eq;
using CamlexNET.Impl.Operations.OrElse;
using NUnit.Framework;

namespace CamlexNET.UnitTests.ReverseEngeneering.Operations
{
    [TestFixture]
    public class OrElseOperationTests
    {
        [Test]
        public void test_THAT_or_else_operation_IS_converted_to_expression_correctly()
        {
            var op11 = new FieldRefOperand("Status");
            var op12 = new BooleanValueOperand(true);
            var op1 = new EqOperation(null, op11, op12);

            var op21 = new FieldRefOperand("Status");
            var op22 = new BooleanValueOperand(false);
            var op2 = new EqOperation(null, op21, op22);

            var op = new OrElseOperation(null, op1, op2);

            var expr = op.ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("((Convert(x.get_Item(\"Status\")) = True) || (Convert(x.get_Item(\"Status\")) = False))"));
        }
    }
}
