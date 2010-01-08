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
        public void test_THAT_integer_value_IS_successfully_created_from_valid_string()
        {
            var operand = new IntegerValueOperand("1");
            Assert.That(operand.Value, Is.EqualTo(1));
        }

        [Test]
        [ExpectedException(typeof(InvalidValueForOperandTypeException))]
        public void test_WHEN_string_is_not_valid_integer_THEN_exception_is_thrown()
        {
            var operand = new IntegerValueOperand("asd");
            Assert.That(operand.Value, Is.EqualTo(1));
        }

        [Test]
        public void test_THAT_text_value_IS_rendered_to_caml_properly()
        {
            var operand = new TextValueOperand("foo");
            string caml = operand.ToCaml().ToString();
            Assert.That(caml, Is.EqualTo("<Value Type=\"Text\">foo</Value>"));
        }

        [Test]
        [ExpectedException(typeof(NullValueOperandCannotBeTranslatedToCamlException))]
        public void test_WHEN_nullvalue_operand_is_rendered_to_caml_THEN_exception_is_thrown()
        {
            var operand = new NullValueOperand();
            operand.ToCaml();
        }

        [Test]
        public void test_THAT_generic_string_based_value_IS_rendered_to_caml_properly()
        {
            var operand = new GenericStringBasedValueOperand(typeof(DataTypes.User), "John Smith");
            string caml = operand.ToCaml().ToString();
            Assert.That(caml, Is.EqualTo("<Value Type=\"User\">John Smith</Value>"));
        }
    }
}


