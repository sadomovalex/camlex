using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Camlex.NET.Impl.Operands;
using NUnit.Framework;

namespace Camlex.NET.UnitTests.Operands
{
    [TestFixture]
    public class BooleanValueOperandTests
    {
        [Test]
        public void test_THAT_boolean_value_IS_rendered_to_caml_properly()
        {
            var operand = new BooleanValueOperand(true);
            string caml = operand.ToCaml().ToString();
            Assert.That(caml, Is.EqualTo("<Value Type=\"Boolean\">True</Value>"));
        }

        [Test]
        public void test_THAT_boolean_value_IS_successfully_created_from_valid_string1()
        {
            var operand = new BooleanValueOperand("True");
            Assert.That(operand.Value, Is.True);
        }

        [Test]
        public void test_THAT_boolean_value_IS_successfully_created_from_valid_string2()
        {
            var operand = new BooleanValueOperand("False");
            Assert.That(operand.Value, Is.False);
        }

        [Test]
        [ExpectedException(typeof(InvalidValueForOperandTypeException))]
        public void test_WHEN_string_is_not_valid_boolean_THEN_exception_is_thrown()
        {
            var operand = new IntegerValueOperand("asd");
            Assert.That(operand.Value, Is.EqualTo(false));
        }
    }
}
