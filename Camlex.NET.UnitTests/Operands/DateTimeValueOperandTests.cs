using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CamlexNET;
using CamlexNET.Impl.Operands;
using NUnit.Framework;

namespace Camlex.NET.UnitTests.Operands
{
    [TestFixture]
    public class DateTimeValueOperandTests
    {
        [Test]
        public void test_THAT_datetime_value_with_includetimevalue_IS_rendered_to_caml_properly()
        {
            var dateTime = DateTime.Now;
            var operand = new DateTimeValueOperand(dateTime, true);
            var caml = operand.ToCaml().ToString();
            Assert.That(caml, Is.EqualTo("<Value Type=\"DateTime\" IncludeTimeValue=\"True\">" + dateTime.ToString("s") + "</Value>"));
        }

        [Test]
        public void test_THAT_datetime_value_without_includetimevalue_IS_rendered_to_caml_properly()
        {
            var dateTime = DateTime.Now;
            var operand = new DateTimeValueOperand(dateTime, false);
            var caml = operand.ToCaml().ToString();
            Assert.That(caml, Is.EqualTo("<Value Type=\"DateTime\">" + dateTime.ToString("s") + "</Value>"));
        }

        [Test]
        public void test_THAT_datetime_value_IS_successfully_created_from_valid_string()
        {
            var operand = new DateTimeValueOperand("02.01.2010 03:04:05", true);
            Assert.That(operand.Value, Is.EqualTo(new DateTime(2010, 1, 2, 3, 4, 5)));
        }

        [Test]
        [ExpectedException(typeof(InvalidValueForOperandTypeException))]
        public void test_WHEN_datetime_value_is_not_valid_datetime_THEN_exception_is_thrown()
        {
            var operand = new DateTimeValueOperand("abc", true);
        }
    }
}
