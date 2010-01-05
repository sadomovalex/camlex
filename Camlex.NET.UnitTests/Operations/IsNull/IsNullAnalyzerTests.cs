using System;
using System.Linq.Expressions;
using Camlex.NET.Impl.Operands;
using Camlex.NET.Impl.Operations.IsNotNull;
using Camlex.NET.Impl.Operations.IsNull;
using Camlex.NET.Interfaces;
using Microsoft.SharePoint;
using NUnit.Framework;
using Rhino.Mocks;

namespace Camlex.NET.UnitTests.Operations.IsNull
{
    [TestFixture]
    public class IsNullAnalyzerTests
    {
        [Test]
        public void test_THAT_isnull_expression_IS_valid()
        {
            var operandBuilder = MockRepository.GenerateStub<IOperandBuilder>();
            operandBuilder.Stub(b => b.CreateValueOperand(null)).Return(new NullValueOperand()).IgnoreArguments();

            var analyzer = new IsNullAnalyzer(null, operandBuilder);
            Expression<Func<SPItem, bool>> expr = x => x["Count"] == null;
            Assert.That(analyzer.IsValid(expr), Is.True);
        }

        [Test]
        public void test_THAT_isnull_expression_IS_determined_properly()
        {
            // arrange
            Expression<Func<SPItem, bool>> expr = x => x["Count"] == null;

            var operandBuilder = MockRepository.GenerateStub<IOperandBuilder>();
            operandBuilder.Stub(b => b.CreateFieldRefOperand(expr.Body)).Return(null);
            operandBuilder.Stub(b => b.CreateValueOperand(expr.Body)).Return(new NullValueOperand()).IgnoreArguments();

            var analyzer = new IsNullAnalyzer(null, operandBuilder);

            // act
            var operation = analyzer.GetOperation(expr);

            //assert
            Assert.That(operation, Is.InstanceOf<IsNullOperation>());
        }
    }
}


