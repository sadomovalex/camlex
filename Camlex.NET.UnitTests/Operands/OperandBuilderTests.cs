using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Camlex.NET.Impl.Operands;
using Camlex.NET.Interfaces;
using Microsoft.SharePoint;
using NUnit.Framework;

namespace Camlex.NET.UnitTests.Operands
{
    [TestFixture]
    public class OperandBuilderTests
    {
        [Test]
        public void test_THAT_field_ref_operand_IS_created_successfully()
        {
            var operandBuilder = new OperandBuilder();
            Expression<Func<SPItem, bool>> expr = x => (string) x["Email"] == "test@example.com";
            var operand = operandBuilder.CreateFieldRefOperand(((BinaryExpression)expr.Body).Left);
            Assert.That(operand, Is.InstanceOf<FieldRefOperand>());
            Assert.That(((FieldRefOperand)operand).FieldName, Is.EqualTo("Email"));
        }

        [Test]
        public void test_WHEN_value_is_text_THEN_text_operand_is_created()
        {
            var operandBuilder = new OperandBuilder();
            Expression<Func<SPItem, bool>> expr = x => (string)x["Email"] == "test@example.com";
            var operand = operandBuilder.CreateValueOperand(((BinaryExpression)expr.Body).Right);
            
            Assert.That(operand, Is.InstanceOf<TextValueOperand>());

            var valueOperand = operand as TextValueOperand;
            Assert.That(valueOperand.Type, Is.EqualTo(typeof(DataTypes.Text)));
            Assert.That(valueOperand.Value, Is.EqualTo("test@example.com"));
        }

        [Test]
        public void test_WHEN_value_is_integer_THEN_integer_operand_is_created()
        {
            var operandBuilder = new OperandBuilder();
            Expression<Func<SPItem, bool>> expr = x => (int)x["Foo"] == 1;
            var operand = operandBuilder.CreateValueOperand(((BinaryExpression)expr.Body).Right);

            Assert.That(operand, Is.InstanceOf<IntegerValueOperand>());

            var valueOperand = operand as IntegerValueOperand;
            Assert.That(valueOperand.Type, Is.EqualTo(typeof(DataTypes.Integer)));
            Assert.That(valueOperand.Value, Is.EqualTo(1));
        }

        [Test]
        public void test_WHEN_value_is_null_THEN_nullvalue_operand_is_created()
        {
            var operandBuilder = new OperandBuilder();
            Expression<Func<SPItem, bool>> expr = x => x["Foo"] == null;
            var operand = operandBuilder.CreateValueOperand(((BinaryExpression)expr.Body).Right);

            Assert.That(operand, Is.InstanceOf<NullValueOperand>());
        }

        [Test]
        public void test_WHEN_variable_is_null_THEN_nullvalue_operand_is_created()
        {
            object o = null;
            Expression<Func<SPItem, bool>> expr = x => x["Foo"] == o;

            var operandBuilder = new OperandBuilder();
            var operand = operandBuilder.CreateValueOperand(((BinaryExpression)expr.Body).Right);

            Assert.That(operand, Is.InstanceOf<NullValueOperand>());
        }

        [Test]
        public void test_WHEN_method_call_result_is_null_THEN_nullvalue_operand_is_created()
        {
            Expression<Func<SPItem, bool>> expr = x => x["Foo"] == val1();

            var operandBuilder = new OperandBuilder();
            var operand = operandBuilder.CreateValueOperand(((BinaryExpression)expr.Body).Right);

            Assert.That(operand, Is.InstanceOf<NullValueOperand>());
        }

        object val1()
        {
            return null;
        }

        [Test]
        public void test_WHEN_nested_method_call_result_is_null_THEN_nullvalue_operand_is_created()
        {
            Func<object> f = () => null;
            Expression<Func<SPItem, bool>> expr = x => x["Foo"] == f();

            var operandBuilder = new OperandBuilder();
            var operand = operandBuilder.CreateValueOperand(((BinaryExpression)expr.Body).Right);

            Assert.That(operand, Is.InstanceOf<NullValueOperand>());
        }
    }
}
