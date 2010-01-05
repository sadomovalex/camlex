using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Camlex.NET.Impl.Operations.Array;
using Camlex.NET.Interfaces;
using Microsoft.SharePoint;
using NUnit.Framework;
using Rhino.Mocks;

namespace Camlex.NET.UnitTests.Array
{
    [TestFixture]
    public class ArrayAnalyzerTest
    {
        [Test]
        public void test_THAT_array_expression_IS_valid()
        {
            var analyzer = new ArrayAnalyzer(null);
            Expression<Func<SPItem, object[]>> expr = (x => new [] { x["field1"], x["field2"] as Camlex.Asc, x["field3"] as Camlex.Desc });
            Assert.That(analyzer.IsValid(expr), Is.True);
        }

        [Test]
        public void test_THAT_array_expression_IS_determined_properly()
        {
            // arrange
            Expression<Func<SPItem, object[]>> expr = (x => new[] { x["field1"], x["field2"] as Camlex.Asc, x["field3"] as Camlex.Desc });
            var expressions = ((NewArrayExpression)expr.Body).Expressions;

            var operandBuilder = MockRepository.GenerateStub<IOperandBuilder>();
            operandBuilder.Stub(b => b.CreateFieldRefOperandWithOrdering(expressions[0], new Camlex.Asc())).Return(null);
            operandBuilder.Stub(b => b.CreateFieldRefOperandWithOrdering(((UnaryExpression)expressions[1]).Operand, new Camlex.Desc())).Return(null);
            operandBuilder.Stub(b => b.CreateFieldRefOperandWithOrdering(((UnaryExpression)expressions[2]).Operand, new Camlex.Desc())).Return(null);
            var analyzer = new ArrayAnalyzer(operandBuilder);

            // act
            var operation = analyzer.GetOperation(expr);

            //assert
            Assert.That(operation, Is.InstanceOf<ArrayOperation>());
        }
    }
}
