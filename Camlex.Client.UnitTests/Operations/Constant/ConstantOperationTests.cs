using CamlexNET.Impl.Factories;
using CamlexNET.Impl.Operands;
using CamlexNET.Impl.Operations.BeginsWith;
using NUnit.Framework;

namespace CamlexNET.UnitTests.Operations.Constant
{
    [TestFixture]
    public class ConstantOperationTests
    {
        [Test]
        public void test_THAT_constant_operation_IS_rendered_to_caml_properly()
        {
            var operationResultBuilder = new OperationResultBuilder();
            var operand = new ConstantOperand(10, "foo");
            var o = new ConstantOperation(operationResultBuilder, operand);
            Assert.That(o.ToResult().ToString(), Is.EqualTo("<foo>10</foo>"));
        }
    }
}
