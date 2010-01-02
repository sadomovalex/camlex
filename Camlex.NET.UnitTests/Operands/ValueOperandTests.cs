using Camlex.NET.Impl.Operands;
using NUnit.Framework;

namespace Camlex.NET.UnitTests.Operands
{
    [TestFixture]
    public class ValueOperandTests
    {
        [Test]
        public void test_THAT_integer_value_IS_rendered_to_caml_properly()
        {
            var operand = new IntegerValueOperand(1);
            string caml = operand.ToCaml().ToString();
            Assert.That(caml, Is.EqualTo("<Value Type=\"Integer\">1</Value>"));
        }

        [Test]
        public void test_THAT_text_value_IS_rendered_to_caml_properly()
        {
            var operand = new TextValueOperand("foo");
            string caml = operand.ToCaml().ToString();
            Assert.That(caml, Is.EqualTo("<Value Type=\"Text\">foo</Value>"));
        }
    }
}


