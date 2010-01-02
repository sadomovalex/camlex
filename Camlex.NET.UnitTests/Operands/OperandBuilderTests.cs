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
        public void test_THAT_field_ref_operand_IS_created_successfully_from_valid_expression()
        {
            var operandBuilder = new OperandBuilder();
            Expression<Func<SPItem, bool>> expr = x => (string) x["Email"] == "test@example.com";
            var operand = operandBuilder.CreateFieldRefOperand(((BinaryExpression)expr.Body).Left);
            Assert.That(operand, Is.InstanceOf<FieldRefOperand>());
            Assert.That(((FieldRefOperand)operand).FieldName, Is.EqualTo("Email"));
        }

        [Test]
        public void test_THAT_text_value_operand_IS_created_successfully_from_valid_expression()
        {
            var operandBuilder = new OperandBuilder();
            Expression<Func<SPItem, bool>> expr = x => (string)x["Email"] == "test@example.com";
            var operand = operandBuilder.CreateValueOperand(((BinaryExpression)expr.Body).Right);
            
            Assert.That(operand, Is.InstanceOf<TextValueOperand>());

            var valueOperand = operand as TextValueOperand;
            Assert.That(valueOperand.Type, Is.EqualTo(DataType.Text));
            Assert.That(valueOperand.Value, Is.EqualTo("test@example.com"));
        }

        [Test]
        public void test_THAT_integer_value_operand_IS_created_successfully_from_valid_expression()
        {
            var operandBuilder = new OperandBuilder();
            Expression<Func<SPItem, bool>> expr = x => (int)x["Foo"] == 1;
            var operand = operandBuilder.CreateValueOperand(((BinaryExpression)expr.Body).Right);

            Assert.That(operand, Is.InstanceOf<IntegerValueOperand>());

            var valueOperand = operand as IntegerValueOperand;
            Assert.That(valueOperand.Type, Is.EqualTo(DataType.Integer));
            Assert.That(valueOperand.Value, Is.EqualTo(1));
        }
    }
}
