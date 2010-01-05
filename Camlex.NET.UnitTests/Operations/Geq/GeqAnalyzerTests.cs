using System;
using System.Linq.Expressions;
using Camlex.NET.Impl.Eq;
using Camlex.NET.Impl.Geq;
using Camlex.NET.Interfaces;
using Microsoft.SharePoint;
using NUnit.Framework;
using Rhino.Mocks;

namespace Camlex.NET.UnitTests.Geq
{
    [TestFixture]
    public class GeqAnalyzerTests
    {
        [Test]
        public void test_THAT_geq_expression_IS_valid()
        {
            var analyzer = new GeqAnalyzer(null);
            Expression<Func<SPItem, bool>> expr = x => (int) x["Count"] >= 1;
            Assert.That(analyzer.IsValid(expr), Is.True);
        }

        [Test]
        public void test_THAT_geq_expression_IS_determined_properly()
        {
            // arrange
            Expression<Func<SPItem, bool>> expr = x => (int)x["Count"] >= 1;

            var operandBuilder = MockRepository.GenerateStub<IOperandBuilder>();
            operandBuilder.Stub(b => b.CreateFieldRefOperand(expr.Body)).Return(null);
            operandBuilder.Stub(b => b.CreateValueOperand(expr.Body)).Return(null);

            var analyzer = new GeqAnalyzer(operandBuilder);

            // act
            var operation = analyzer.GetOperation(expr);

            //assert
            Assert.That(operation, Is.InstanceOf<GeqOperation>());
        }
    }
}


