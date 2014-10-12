using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using CamlexNET.Impl.Operations.BeginsWith;
using CamlexNET.Impl.Operations.Constant;
using CamlexNET.Interfaces;
using Microsoft.SharePoint.Client;
using NUnit.Framework;
using Rhino.Mocks;

namespace CamlexNET.UnitTests.Operations.Constant
{
    [TestFixture]
    public class ConstantAnalyzerTests
    {
        [Test]
        public void test_THAT_constant_expression_IS_valid()
        {
            var analyzer = new ConstantAnalyzer(null, null, "foo");
            int c = 10;
            Expression<Func<int>> expr = () => c;
            Assert.That(analyzer.IsValid(expr), Is.True);
        }

        [Test]
        [ExpectedException(typeof(NonSupportedExpressionException))]
        public void test_WHEN_operation_is_not_valid_THEN_exception_is_thrown()
        {
            var analyzer = new ConstantAnalyzer(null, null, "foo");
            Expression<Func<ListItem, bool>> expr = x => true;
            analyzer.GetOperation(expr);
        }

        [Test]
        public void test_THAT_constant_expression_IS_determined_properly()
        {
            var operandBuilder = MockRepository.GenerateStub<IOperandBuilder>();
            var operationResultBuilder = MockRepository.GenerateStub<IOperationResultBuilder>();
            var analyzer = new ConstantAnalyzer(operationResultBuilder, operandBuilder, "foo");
            int c = 10;
            Expression<Func<int>> expr = () => c;

            var operation = analyzer.GetOperation(expr);
            Assert.IsInstanceOf<ConstantOperation>(operation);
        }
    }
}
