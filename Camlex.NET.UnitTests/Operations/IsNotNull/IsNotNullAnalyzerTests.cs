using System;
using System.Linq.Expressions;
using CamlexNET.Impl.Operands;
using CamlexNET.Impl.Operations.IsNotNull;
using CamlexNET.Impl.Operations.Lt;
using CamlexNET.Interfaces;
using Microsoft.SharePoint;
using NUnit.Framework;
using Rhino.Mocks;

namespace CamlexNET.UnitTests.Operations.IsNotNull
{
    [TestFixture]
    public class IsNotNullAnalyzerTests
    {
        [Test]
        public void test_THAT_isnotnull_expression_IS_valid()
        {
            var operandBuilder = MockRepository.GenerateStub<IOperandBuilder>();
            operandBuilder.Stub(b => b.CreateValueOperandForNativeSyntax(null)).Return(new NullValueOperand()).IgnoreArguments();

            var analyzer = new IsNotNullAnalyzer(null, operandBuilder);
            Expression<Func<SPItem, bool>> expr = x => x["Count"] != null;
            Assert.That(analyzer.IsValid(expr), Is.True);
        }

        [Test]
        public void test_THAT_string_based_expression_IS_not_valid_isnotnull_expression()
        {
            var operandBuilder = MockRepository.GenerateStub<IOperandBuilder>();
            operandBuilder.Stub(b => b.CreateValueOperandForNativeSyntax(null)).Return(new NullValueOperand()).IgnoreArguments();

            var analyzer = new IsNotNullAnalyzer(null, operandBuilder);
            Expression<Func<SPItem, bool>> expr = x => x["Count"] != (DataTypes.Integer)"1";
            Assert.That(analyzer.IsValid(expr), Is.False);
        }

        [Test]
        public void test_THAT_string_based_null_expression_IS_valid_isnotnull_expression()
        {
            var operandBuilder = MockRepository.GenerateStub<IOperandBuilder>();
            operandBuilder.Stub(b => b.CreateValueOperandForNativeSyntax(null)).Return(new NullValueOperand()).IgnoreArguments();

            var analyzer = new IsNotNullAnalyzer(null, operandBuilder);
            Expression<Func<SPItem, bool>> expr = x => x["Count"] != (DataTypes.Integer)null;
            Assert.That(analyzer.IsValid(expr), Is.True);
        }

        [Test]
        public void test_THAT_isnotnull_expression_IS_determined_properly()
        {
            // arrange
            Expression<Func<SPItem, bool>> expr = x => x["Count"] != null;

            var operandBuilder = MockRepository.GenerateStub<IOperandBuilder>();
            operandBuilder.Stub(b => b.CreateFieldRefOperand(expr.Body)).Return(null);
            operandBuilder.Stub(b => b.CreateValueOperandForNativeSyntax(expr.Body)).Return(new NullValueOperand()).IgnoreArguments();

            var analyzer = new IsNotNullAnalyzer(null, operandBuilder);

            // act
            var operation = analyzer.GetOperation(expr);

            //assert
            Assert.That(operation, Is.InstanceOf<IsNotNullOperation>());
        }
    }
}


