using CamlexNET.Impl.Factories;
using CamlexNET.Impl.Operands;
using CamlexNET.Impl.Operations.BeginsWith;
using NUnit.Framework;

namespace CamlexNET.UnitTests.ReverseEngeneering.Operations
{
    [TestFixture]
    public class ConstantOperationTests
    {
        [Test]
        public void test_THAT_constant_operation_IS_converted_to_expression_properly()
        {
            var operationResultBuilder = new OperationResultBuilder();
            var operand = new ConstantOperand(10, "foo");
            var o = new ConstantOperation(operationResultBuilder, operand);
            Assert.That(o.ToExpression().ToString(), Is.EqualTo("10"));
        }
    }
}
