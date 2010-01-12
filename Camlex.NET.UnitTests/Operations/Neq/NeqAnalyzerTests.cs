using System;
using System.Linq.Expressions;
using CamlexNET.Impl.Operations.Eq;
using CamlexNET.Impl.Operations.Neq;
using CamlexNET.Interfaces;
using Microsoft.SharePoint;
using NUnit.Framework;
using Rhino.Mocks;

namespace CamlexNET.UnitTests.Operations.Neq
{
    [TestFixture]
    public class NeqAnalyzerTests
    {
        [Test]
        public void test_THAT_neq_expression_IS_valid()
        {
            var analyzer = new NeqAnalyzer(null, null);
            Expression<Func<SPItem, bool>> expr = x => (string) x["Title"] != "testValue";
            Assert.That(analyzer.IsValid(expr), Is.True);
        }

        [Test]
        public void test_THAT_neq_expression_IS_determined_properly()
        {
            // arrange
            Expression<Func<SPItem, bool>> expr = x => (string)x["Title"] != "testValue";

            var operandBuilder = MockRepository.GenerateStub<IOperandBuilder>();
            operandBuilder.Stub(b => b.CreateFieldRefOperand(expr.Body)).Return(null);
            operandBuilder.Stub(b => b.CreateValueOperandForNativeSyntax(expr.Body)).Return(null);

            var analyzer = new NeqAnalyzer(null, operandBuilder);

            // act
            var operation = analyzer.GetOperation(expr);

            //assert
            Assert.That(operation, Is.InstanceOf<NeqOperation>());
        }
    }
}


