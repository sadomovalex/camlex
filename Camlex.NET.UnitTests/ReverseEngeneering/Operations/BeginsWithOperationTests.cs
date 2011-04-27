using System;
using CamlexNET.Impl.Operands;
using CamlexNET.Impl.Operations.Array;
using CamlexNET.Impl.Operations.BeginsWith;
using CamlexNET.Impl.Operations.Eq;
using NUnit.Framework;

namespace CamlexNET.UnitTests.ReverseEngeneering.Operations
{
    [TestFixture]
    public class BeginsWithOperationTests
    {
        [Test]
        public void test_THAT_begin_with_operation_IS_converted_to_expression_correctly()
        {
            var op1 = new FieldRefOperand("Title");
            var op2 = new TextValueOperand("foo");
            var op = new BeginsWithOperation(null, op1, op2);
            var expr = op.ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("Convert(x.get_Item(\"Title\")).StartsWith(\"foo\")"));
        }
    }
}
