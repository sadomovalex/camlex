using System;
using System.Linq.Expressions;
using CamlexNET.Impl.Operations.Contains;
using CamlexNET.Interfaces;
using Microsoft.SharePoint;
using NUnit.Framework;
using Rhino.Mocks;

namespace CamlexNET.UnitTests.Operations.Contains
{
    [TestFixture]
    public class ContainsAnalyzerTests
    {
        [Test]
        public void test_THAT_contains_expression_with_string_type_IS_valid()
        {
            var analyzer = new ContainsAnalyzer(null, null);
            Expression<Func<SPItem, bool>> expr = x => ((string)x["Count"]).Contains("foo");
            Assert.That(analyzer.IsValid(expr), Is.True);
        }

        [Test]
        public void test_THAT_contains_expression_with_custom_text_type_IS_valid()
        {
            var analyzer = new ContainsAnalyzer(null, null);
            Expression<Func<SPItem, bool>> expr = x => ((DataTypes.Text)x["Count"]).Contains("foo");
            Assert.That(analyzer.IsValid(expr), Is.True);
        }

        [Test]
        public void test_THAT_contains_expression_with_custom_note_type_IS_valid()
        {
            var analyzer = new ContainsAnalyzer(null, null);
            Expression<Func<SPItem, bool>> expr = x => ((DataTypes.Note)x["Count"]).Contains("foo");
            Assert.That(analyzer.IsValid(expr), Is.True);
        }

        [Test]
        public void test_THAT_contains_expression_with_string_type_and_variable_IS_valid()
        {
            var analyzer = new ContainsAnalyzer(null, null);
            var stringVar = "Blah-blah-blah";
            Expression<Func<SPItem, bool>> expr = x => ((string)x["Count"]).Contains(stringVar);
            Assert.That(analyzer.IsValid(expr), Is.True);
        }

        [Test]
        public void test_THAT_contains_expression_with_custom_text_type_and_variable_IS_valid()
        {
            var analyzer = new ContainsAnalyzer(null, null);
            var stringVar = "Blah-blah-blah";
            Expression<Func<SPItem, bool>> expr = x => ((DataTypes.Text)x["Count"]).Contains(stringVar);
            Assert.That(analyzer.IsValid(expr), Is.True);
        }

        [Test]
        public void test_THAT_contains_expression_with_custom_note_type_and_variable_IS_valid()
        {
            var analyzer = new ContainsAnalyzer(null, null);
            var stringVar = "Blah-blah-blah";
            Expression<Func<SPItem, bool>> expr = x => ((DataTypes.Note)x["Count"]).Contains(stringVar);
            Assert.That(analyzer.IsValid(expr), Is.True);
        }

        [Test]
        public void test_THAT_contains_expression_IS_determined_properly()
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
        public void test_THAT_contains_expression_with_string_type_IS_determined_Properly()
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
        public void test_THAT_contains_expression_with_custom_note_type_IS_determined_properly()
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
