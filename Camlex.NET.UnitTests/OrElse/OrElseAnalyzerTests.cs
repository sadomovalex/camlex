using System;
using System.Linq.Expressions;
using Camlex.NET.Impl.AndAlso;
using Camlex.NET.Impl.Factories;
using Camlex.NET.Impl.OrElse;
using Camlex.NET.Interfaces;
using Microsoft.SharePoint;
using NUnit.Framework;
using Rhino.Mocks;

namespace Camlex.NET.UnitTests.OrElse
{
    [TestFixture]
    public class OrElseAnalyzerTests
    {
        [Test]
        public void test_THAT_logical_or_expression_IS_valid_expression()
        {
            Expression<Func<SPItem, bool>> expr = x => (string) x["Email"] == "test@example.com" ||
                                                       (int) x["Count1"] == 1;
            var analyzerFactory = new AnalyzerFactory(null);
            var analyzer = new OrElseAnalyzer(analyzerFactory);
            Assert.That(analyzer.IsValid(expr), Is.True);
        }

        [Test]
        public void test_THAT_binary_or_expression_IS_not_valid_expression()
        {
            Expression<Func<SPItem, bool>> expr = x => (string)x["Email"] == "test@example.com" |
                                                       (int)x["Count1"] == 1;
            var analyzerFactory = new AnalyzerFactory(null);
            var analyzer = new OrElseAnalyzer(analyzerFactory);
            Assert.That(analyzer.IsValid(expr), Is.False);
        }

        [Test]
        public void test_THAT_valid_orelse_expression_IS_recognized_successfully()
        {
            // arrange
            var analyzerFactoryStub = MockRepository.GenerateStub<IAnalyzerFactory>();
            var analyzerStub = MockRepository.GenerateStub<ILogicalAnalyzer>();
            analyzerFactoryStub.Stub(f => f.CreateLogicalAnalyzer(ExpressionType.Equal)).Return(analyzerStub);
            analyzerStub.Stub(a => a.IsValid(null)).Return(true).IgnoreArguments();
            var analyzer = new OrElseAnalyzer(analyzerFactoryStub);


            Expression<Func<SPItem, bool>> expr = x => (string)x["Email"] == "test@example.com" ||
                                                       (int)x["Count1"] == 1;
            // act
            var operation = analyzer.GetOperation(expr);

            // assert
            Assert.That(operation, Is.InstanceOf<OrElseOperation>());
        }
    }
}


