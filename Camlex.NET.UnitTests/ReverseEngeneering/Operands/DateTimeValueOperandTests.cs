using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CamlexNET.Impl.Operands;
using NUnit.Framework;

namespace CamlexNET.UnitTests.ReverseEngeneering.Operands
{
    [TestFixture]
    public class DateTimeValueOperandTests
    {
        [Test]
        public void test_THAT_operand_with_native_value_IS_conveted_to_expression_correctly()
        {
            var dt = new DateTime(2011, 4, 12, 22, 00, 00, 00);
            var op = new DateTimeValueOperand(dt, false);
            var expr = op.ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo(dt.ToString()));
        }

        [Test]
        public void test_THAT_operand_with_native_string_value_IS_conveted_to_expression_correctly()
        {
            var dt = new DateTime(2011, 4, 12, 22, 00, 00, 00);
            var op = new DateTimeValueOperand(dt.ToString(), false);
            var expr = op.ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo(dt.ToString()));
        }

        [Test]
        public void test_THAT_operand_with_now_IS_conveted_to_expression_correctly()
        {
            var op = new DateTimeValueOperand(Camlex.Now, false);
            var expr = op.ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("Convert(Convert(\"Now\"))"));
        }

        [Test]
        public void test_THAT_operand_with_today_IS_conveted_to_expression_correctly()
        {
            var op = new DateTimeValueOperand(Camlex.Today, false);
            var expr = op.ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("Convert(Convert(\"Today\"))"));
        }

        [Test]
        public void test_THAT_operand_with_week_IS_conveted_to_expression_correctly()
        {
            var op = new DateTimeValueOperand(Camlex.Week, false);
            var expr = op.ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("Convert(Convert(\"Week\"))"));
        }

        [Test]
        public void test_THAT_operand_with_month_IS_conveted_to_expression_correctly()
        {
            var op = new DateTimeValueOperand(Camlex.Month, false);
            var expr = op.ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("Convert(Convert(\"Month\"))"));
        }

        [Test]
        public void test_THAT_operand_with_year_IS_conveted_to_expression_correctly()
        {
            var op = new DateTimeValueOperand(Camlex.Year, false);
            var expr = op.ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("Convert(Convert(\"Year\"))"));
        }

        [Test]
        public void test_THAT_native_operand_with_includetimevalue_IS_conveted_to_expression_correctly()
        {
            var dt = new DateTime(2011, 4, 16, 22, 00, 00, 00);
            var op = new DateTimeValueOperand(dt.ToString(), true);
            var expr = op.ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo(dt + ".IncludeTimeValue()"));
        }
    }
}
