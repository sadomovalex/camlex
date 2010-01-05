using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Camlex.NET.Impl.AndAlso;
using Camlex.NET.Impl.Eq;
using Camlex.NET.Impl.Factories;
using Camlex.NET.Interfaces;
using Microsoft.SharePoint;
using NUnit.Framework;
using Rhino.Mocks;

namespace Camlex.NET.UnitTests.AndAlso
{
    [TestFixture]
    public class AndAlsoAnalyzerTests
    {
        [Test]
        public void test_THAT_logical_and_expression_IS_valid_expression()
        {
            Expression<Func<SPItem, bool>> expr = x => (string) x["Email"] == "test@example.com" &&
                                                       (int) x["Count1"] == 1;
            var analyzerFactory = new AnalyzerFactory(null);
            var analyzer = new AndAlsoAnalyzer(analyzerFactory);
            Assert.That(analyzer.IsValid(expr), Is.True);
        }

        [Test]
        public void test_THAT_binary_and_expression_IS_not_valid_expression()
        {
            Expression<Func<SPItem, bool>> expr = x => (string)x["Email"] == "test@example.com" &
                                                       (int)x["Count1"] == 1;
            var analyzerFactory = new AnalyzerFactory(null);
            var analyzer = new AndAlsoAnalyzer(analyzerFactory);
            Assert.That(analyzer.IsValid(expr), Is.False);
        }

        [Test]
        public void test_THAT_valid_andalso_expression_IS_recognized_successfully()
        {
            // arrange
            var analyzerFactoryStub = MockRepository.GenerateStub<IAnalyzerFactory>();
            var analyzerStub = MockRepository.GenerateStub<IAnalyzer>();
            analyzerFactoryStub.Stub(f => f.Create(ExpressionType.Equal)).Return(analyzerStub);
            analyzerStub.Stub(a => a.IsValid(null)).Return(true).IgnoreArguments();
            var analyzer = new AndAlsoAnalyzer(analyzerFactoryStub);


            Expression<Func<SPItem, bool>> expr = x => (string)x["Email"] == "test@example.com" &&
                                                       (int)x["Count1"] == 1;
            // act
            var operation = analyzer.GetOperation(expr);

            // assert
            Assert.That(operation, Is.InstanceOf<AndAlsoOperation>());
        }
    }
}
