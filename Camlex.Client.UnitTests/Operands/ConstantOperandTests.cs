using CamlexNET.Impl.Operands;
using NUnit.Framework;

namespace CamlexNET.UnitTests.Operands
{
    [TestFixture]
    public class ConstantOperandTests
    {
        [Test]
        public void test_THAT_constant_operand_IS_rendered_to_caml_properly()
        {
            var o = new ConstantOperand(10, "foo");
            Assert.That(o.ToCaml().ToString(), Is.EqualTo("<foo>10</foo>"));
        }
    }
}
