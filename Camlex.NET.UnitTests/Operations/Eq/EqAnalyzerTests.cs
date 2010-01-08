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
            var analyzer = new EqAnalyzer(null, null);
            Expression<Func<SPItem, bool>> expr = x => (string) x["Title"] == "testValue";
            Assert.That(analyzer.IsValid(expr), Is.True);
        }

        [Test]
        public void test_THAT_string_based_eq_expression_IS_valid()
        {
            var analyzer = new EqAnalyzer(null, null);
            Expression<Func<SPItem, bool>> expr = x => x["Title"] == (DataTypes.Text)"testValue";
            Assert.That(analyzer.IsValid(expr), Is.True);
        }

        [Test]
        public void test_THAT_string_based_eq_expression_IS_not_valid()
        {
            var analyzer = new EqAnalyzer(null, null);
            Expression<Func<SPItem, bool>> expr = x => x["Title"] == (DataTypes.Text)new BaseFieldType();
            Assert.That(analyzer.IsValid(expr), Is.False);
        }

        [Test]
        public void test_THAT_string_based_eq_expression_with_variable_IS_not_valid()
        {
            var analyzer = new EqAnalyzer(null, null);
            var foo = new DataTypes.Text();
            Expression<Func<SPItem, bool>> expr = x => x["Title"] == foo;
            Assert.That(analyzer.IsValid(expr), Is.False);
        }

        [Test]
        public void test_THAT_eq_expression_IS_determined_properly()
        {
            // arrange
            Expression<Func<SPItem, bool>> expr = x => (string)x["Title"] == "testValue";

            var operandBuilder = MockRepository.GenerateStub<IOperandBuilder>();
            operandBuilder.Stub(b => b.CreateFieldRefOperand(expr.Body)).Return(null);
            operandBuilder.Stub(b => b.CreateValueOperandForNativeSyntax(expr.Body)).Return(null);

            var analyzer = new EqAnalyzer(null, operandBuilder);

            // act
            var operation = analyzer.GetOperation(expr);

            //assert
            Assert.That(operation, Is.InstanceOf<EqOperation>());
        }

        [Test]
        public void test_THAT_string_based_eq_expression_IS_determined_properly()
        {
            // arrange
            Expression<Func<SPItem, bool>> expr = x => x["Title"] == (DataTypes.Text)"testValue";

            var operandBuilder = MockRepository.GenerateStub<IOperandBuilder>();
            operandBuilder.Stub(b => b.CreateFieldRefOperand(expr.Body)).Return(null);
            operandBuilder.Stub(b => b.CreateValueOperandForNativeSyntax(expr.Body)).Return(null);

            var analyzer = new EqAnalyzer(null, operandBuilder);

            // act
            var operation = analyzer.GetOperation(expr);

            //assert
            Assert.That(operation, Is.InstanceOf<EqOperation>());
        }
    }
}


