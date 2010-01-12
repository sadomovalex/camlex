using System;
using System.Linq.Expressions;
using CamlexNET.Impl.Operations.Geq;
using CamlexNET.Interfaces;
using Microsoft.SharePoint;
using NUnit.Framework;
using Rhino.Mocks;

namespace CamlexNET.UnitTests.Operations.Geq
{
    [TestFixture]
    public class GeqAnalyzerTests
    {
        [Test]
        public void test_THAT_geq_expression_IS_valid()
        {
            var analyzer = new GeqAnalyzer(null, null);
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
            operandBuilder.Stub(b => b.CreateValueOperandForNativeSyntax(expr.Body)).Return(null);

            var analyzer = new GeqAnalyzer(null, operandBuilder);

            // act
            var operation = analyzer.GetOperation(expr);

            //assert
            Assert.That(operation, Is.InstanceOf<GeqOperation>());
        }
    }
}


