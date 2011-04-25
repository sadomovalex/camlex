﻿using System;
using CamlexNET.Impl.Operands;
using CamlexNET.Impl.Operations.Eq;
using NUnit.Framework;

namespace CamlexNET.UnitTests.ReverseEngeneering.Operations
{
    [TestFixture]
    public class EqOperationTests
    {
        [Test]
        public void test_THAT_eq_operation_with_bool_IS_converted_to_expression_correctly()
        {
            var op1 = new FieldRefOperand("Status");
            var op2 = new BooleanValueOperand(true);
            var op = new EqOperation(null, op1, op2);
            var expr = op.ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("(Convert(x.get_Item(\"Status\")) = True)"));
        }

        [Test]
        public void test_THAT_eq_operation_with_native_datetime_IS_converted_to_expression_correctly()
        {
            var op1 = new FieldRefOperand("Modified");

            var dt = new DateTime(2011, 4, 25, 19, 7, 00, 00);
            var op2 = new DateTimeValueOperand(dt, false);
            var op = new EqOperation(null, op1, op2);
            var expr = op.ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo(string.Format("(Convert(x.get_Item(\"Modified\")) = {0})", dt)));
        }

        [Test]
        public void test_THAT_eq_operation_with_native_datetime_includetime_IS_converted_to_expression_correctly()
        {
            var op1 = new FieldRefOperand("Modified");

            var dt = new DateTime(2011, 4, 25, 19, 7, 00, 00);
            var op2 = new DateTimeValueOperand(dt, true);
            var op = new EqOperation(null, op1, op2);
            var expr = op.ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo(string.Format("(Convert(x.get_Item(\"Modified\")) = {0}.IncludeTimeValue())", dt)));
        }

        [Test]
        public void test_THAT_eq_operation_with_string_based_datetime_IS_converted_to_expression_correctly()
        {
            var op1 = new FieldRefOperand("Modified");

            var op2 = new DateTimeValueOperand(Camlex.Now, false);
            var op = new EqOperation(null, op1, op2);
            var expr = op.ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("(x.get_Item(\"Modified\") = Convert(Convert(\"Now\")))"));
        }

        [Test]
        public void test_THAT_eq_operation_with_string_based_datetime_includetime_IS_converted_to_expression_correctly()
        {
            var op1 = new FieldRefOperand("Modified");

            var op2 = new DateTimeValueOperand(Camlex.Now, true);
            var op = new EqOperation(null, op1, op2);
            var expr = op.ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("(x.get_Item(\"Modified\") = Convert(Convert(\"Now\")).IncludeTimeValue())"));
        }

        [Test]
        public void test_THAT_eq_operation_with_generic_string_based_operand_IS_converted_to_expression_correctly()
        {
            var op1 = new FieldRefOperand("Title");
            var op2 = new GenericStringBasedValueOperand(typeof(DataTypes.Text), "foo");
            var op = new EqOperation(null, op1, op2);
            var expr = op.ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("(x.get_Item(\"Title\") = Convert(Convert(\"foo\")))"));
        }

        [Test]
        public void test_THAT_eq_operation_with_lookup_id_operand_IS_converted_to_expression_correctly()
        {
            var op1 = new FieldRefOperand("Title");
            var op2 = new LookupIdValueOperand("1");
            var op = new EqOperation(null, op1, op2);
            var expr = op.ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("(x.get_Item(\"Title\") = Convert(Convert(\"1\")))"));
        }

        [Test]
        public void test_THAT_eq_operation_with_lookup_value_operand_IS_converted_to_expression_correctly()
        {
            var op1 = new FieldRefOperand("Title");
            var op2 = new LookupValueValueOperand("foo");
            var op = new EqOperation(null, op1, op2);
            var expr = op.ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("(x.get_Item(\"Title\") = Convert(Convert(\"foo\")))"));
        }
    }
}
