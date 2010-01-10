using System;
using System.Linq.Expressions;
using Camlex.NET.Impl.Operations.Contains;
using Camlex.NET.Interfaces;
using Microsoft.SharePoint;
using NUnit.Framework;
using Rhino.Mocks;

namespace Camlex.NET.UnitTests.Operations.Contains
{
    [TestFixture]
    public class ContainsAnalyzerTests
    {
        [Test]
        public void TestThatContainsExpressionWithStringTypeIsValid()
        {
            var analyzer = new ContainsAnalyzer(null, null);
            Expression<Func<SPItem, bool>> expr = x => ((string)x["Count"]).Contains("foo");
            Assert.That(analyzer.IsValid(expr), Is.True);
        }

        [Test]
        public void TestThatContainsExpressionWithCustomTextTypeIsValid()
        {
            var analyzer = new ContainsAnalyzer(null, null);
            Expression<Func<SPItem, bool>> expr = x => ((DataTypes.Text)x["Count"]).Contains("foo");
            Assert.That(analyzer.IsValid(expr), Is.True);
        }

        [Test]
        public void TestThatContainsExpressionWithCustomNoteTypeIsValid()
        {
            var analyzer = new ContainsAnalyzer(null, null);
            Expression<Func<SPItem, bool>> expr = x => ((DataTypes.Note)x["Count"]).Contains("foo");
            Assert.That(analyzer.IsValid(expr), Is.True);
        }

        [Test]
        public void TestThatContainsExpressionWithStringTypeAndVariableIsValid()
        {
            var analyzer = new ContainsAnalyzer(null, null);
            var stringVar = "Blah-blah-blah";
            Expression<Func<SPItem, bool>> expr = x => ((string)x["Count"]).Contains(stringVar);
            Assert.That(analyzer.IsValid(expr), Is.True);
        }

        [Test]
        public void TestThatContainsExpressionWithCustomTextTypeAndVariableIsValid()
        {
            var analyzer = new ContainsAnalyzer(null, null);
            var stringVar = "Blah-blah-blah";
            Expression<Func<SPItem, bool>> expr = x => ((DataTypes.Text)x["Count"]).Contains(stringVar);
            Assert.That(analyzer.IsValid(expr), Is.True);
        }

        [Test]
        public void TestThatContainsExpressionWithCustomNoteTypeAndVariableIsValid()
        {
            var analyzer = new ContainsAnalyzer(null, null);
            var stringVar = "Blah-blah-blah";
            Expression<Func<SPItem, bool>> expr = x => ((DataTypes.Note)x["Count"]).Contains(stringVar);
            Assert.That(analyzer.IsValid(expr), Is.True);
        }

        [Test]
        public void TestThatContainsExpressionIsDeterminedProperly()
        {
            // arrange
            Expression<Func<SPItem, bool>> expr = x => ((string)x["Count"]).Contains("foo");
            var operandBuilder = MockRepository.GenerateStub<IOperandBuilder>();
            operandBuilder.Stub(b => b.CreateFieldRefOperand(((MethodCallExpression)expr.Body).Object)).Return(null);
            operandBuilder.Stub(b => b.CreateValueOperandForNativeSyntax(((MethodCallExpression)expr.Body).Arguments[0])).Return(null);
            var analyzer = new ContainsAnalyzer(null, operandBuilder);

            // act
            var operation = analyzer.GetOperation(expr);

            //assert
            Assert.That(operation, Is.InstanceOf<ContainsOperation>());
        }

        [Test]
        public void TestThatContainsExpressionWithStringTypeIsDeterminedProperly()
        {
            // arrange
            Expression<Func<SPItem, bool>> expr = x => ((string)x["Count"]).Contains("foo");
            var operandBuilder = MockRepository.GenerateStub<IOperandBuilder>();
            operandBuilder.Stub(b => b.CreateFieldRefOperand(((MethodCallExpression)expr.Body).Object)).Return(null);
            operandBuilder.Stub(b => b.CreateValueOperandForNativeSyntax(((MethodCallExpression)expr.Body).Arguments[0])).Return(null);
            var analyzer = new ContainsAnalyzer(null, operandBuilder);

            // act
            var operation = analyzer.GetOperation(expr);

            //assert
            Assert.That(operation, Is.InstanceOf<ContainsOperation>());
        }

        [Test]
        public void TestThatContainsExpressionWithCustomNoteTypeIsDeterminedProperly()
        {
            // arrange
            Expression<Func<SPItem, bool>> expr = x => ((DataTypes.Note)x["Count"]).Contains("foo");
            var operandBuilder = MockRepository.GenerateStub<IOperandBuilder>();
            operandBuilder.Stub(b => b.CreateFieldRefOperand(((MethodCallExpression)expr.Body).Object)).Return(null);
            operandBuilder.Stub(b => b.CreateValueOperandForNativeSyntax(((MethodCallExpression)expr.Body).Arguments[0])).Return(null);
            var analyzer = new ContainsAnalyzer(null, operandBuilder);

            // act
            var operation = analyzer.GetOperation(expr);

            //assert
            Assert.That(operation, Is.InstanceOf<ContainsOperation>());
        }
    }
}
