using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CamlexNET.Impl.Operands;
using CamlexNET.Impl.Operations.Eq;
using CamlexNET.UnitTests.Helpers;
using NUnit.Framework;

namespace CamlexNET.UnitTests.ReverseEngeneering
{
    [TestFixture]
    public class EqOperationTests
    {
        [Test]
        public void test_THAT_eq_operation_IS_converted_to_expression_correctly()
        {
            var op1 = new FieldRefOperand("Status");
            var op2 = new BooleanValueOperand(true);
            var op = new EqOperation(null, op1, op2);
            var expr = op.ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("(Convert(x.get_Item(\"Status\")) = True)"));
        }
    }
}
