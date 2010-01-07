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
            operandBuilder.Stub(b => b.CreateValueOperandForNativeSyntax(null)).Return(new NullValueOperand()).IgnoreArguments();

            var analyzer = new IsNullAnalyzer(null, operandBuilder);
            Expression<Func<SPItem, bool>> expr = x => x["Count"] == null;
            Assert.That(analyzer.IsValid(expr), Is.True);
        }

        [Test]
        public void test_THAT_string_based_expression_IS_not_valid_isnull_expression()
        {
            var operandBuilder = MockRepository.GenerateStub<IOperandBuilder>();
            operandBuilder.Stub(b => b.CreateValueOperandForNativeSyntax(null)).Return(new NullValueOperand()).IgnoreArguments();

            var analyzer = new IsNullAnalyzer(null, operandBuilder);
            Expression<Func<SPItem, bool>> expr = x => x["Count"] == (DataTypes.Integer)"1";
            Assert.That(analyzer.IsValid(expr), Is.False);
        }

        [Test]
        public void test_THAT_string_based_null_expression_IS_valid_isnull_expression()
        {
            var operandBuilder = MockRepository.GenerateStub<IOperandBuilder>();
            operandBuilder.Stub(b => b.CreateValueOperandForNativeSyntax(null)).Return(new NullValueOperand()).IgnoreArguments();

            var analyzer = new IsNullAnalyzer(null, operandBuilder);
            Expression<Func<SPItem, bool>> expr = x => x["Count"] == (DataTypes.Integer)null;
            Assert.That(analyzer.IsValid(expr), Is.True);
        }

        [Test]
        public void test_THAT_isnull_expression_IS_determined_properly()
        {
            // arrange
            Expression<Func<SPItem, bool>> expr = x => x["Count"] == null;

            var operandBuilder = MockRepository.GenerateStub<IOperandBuilder>();
            operandBuilder.Stub(b => b.CreateFieldRefOperand(expr.Body)).Return(null);
            operandBuilder.Stub(b => b.CreateValueOperandForNativeSyntax(expr.Body)).Return(new NullValueOperand()).IgnoreArguments();

            var analyzer = new IsNullAnalyzer(null, operandBuilder);

            // act
            var operation = analyzer.GetOperation(expr);

            //assert
            Assert.That(operation, Is.InstanceOf<IsNullOperation>());
        }
    }
}


