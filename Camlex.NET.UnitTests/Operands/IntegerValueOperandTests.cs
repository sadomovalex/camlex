using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Camlex.NET.Impl.Operands;
using NUnit.Framework;

namespace Camlex.NET.UnitTests.Operands
{
    [TestFixture]
    public class IntegerValueOperandTests
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
    }
}
