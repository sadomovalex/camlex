using CamlexNET.Impl.Operands;
using NUnit.Framework;

namespace CamlexNET.UnitTests.ReverseEngeneering.Operands
{
    [TestFixture]
    public class ConstantOperandTests
    {
        [Test]
        public void test_THAT_constant_operand_IS_converted_to_expression_properly()
        {
            var o = new ConstantOperand(10, "foo");
            Assert.That(o.ToExpression().ToString(), Is.EqualTo("10"));
        }
    }
}
