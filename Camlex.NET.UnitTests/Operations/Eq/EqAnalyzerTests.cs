using System;
using System.Linq.Expressions;
using Camlex.NET.Impl.Operations.Eq;
using Camlex.NET.Interfaces;
using Microsoft.SharePoint;
using NUnit.Framework;
using Rhino.Mocks;

namespace Camlex.NET.UnitTests.Operations.Eq
{
    [TestFixture]
    public class EqAnalyzerTests
    {
        [Test]
        public void test_THAT_eq_expression_IS_valid()
        {
            var analyzer = new EqAnalyzer(null);
            Expression<Func<SPItem, bool>> expr = x => (string) x["Title"] == "testValue";
            Assert.That(analyzer.IsValid(expr), Is.True);
        }

        [Test]
        public void test_THAT_eq_expression_IS_determined_properly()
        {
            // arrange
            Expression<Func<SPItem, bool>> expr = x => (string)x["Title"] == "testValue";

            var operandBuilder = MockRepository.GenerateStub<IOperandBuilder>();
            operandBuilder.Stub(b => b.CreateFieldRefOperand(expr.Body)).Return(null);
            operandBuilder.Stub(b => b.CreateValueOperand(expr.Body)).Return(null);

            var analyzer = new EqAnalyzer(operandBuilder);

            // act
            var operation = analyzer.GetOperation(expr);

            //assert
            Assert.That(operation, Is.InstanceOf<EqOperation>());
        }
    }
}


