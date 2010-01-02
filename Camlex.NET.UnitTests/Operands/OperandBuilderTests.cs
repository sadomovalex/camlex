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
    }
}
