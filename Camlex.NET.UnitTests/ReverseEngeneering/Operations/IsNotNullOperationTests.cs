using System;
using CamlexNET.Impl.Operands;
using CamlexNET.Impl.Operations.Eq;
using CamlexNET.Impl.Operations.Geq;
using CamlexNET.Impl.Operations.Gt;
using CamlexNET.Impl.Operations.IsNotNull;
using NUnit.Framework;

namespace CamlexNET.UnitTests.ReverseEngeneering.Operations
{
    [TestFixture]
    public class IsNotNullOperationTests
    {
        [Test]
        public void test_THAT_is_not_null_operation_IS_converted_to_expression_correctly()
        {
            var op1 = new FieldRefOperand("Count");
            var op = new IsNotNullOperation(null, op1);
            var expr = op.ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("(x.get_Item(\"Count\") != null)"));
        }
    }
}
