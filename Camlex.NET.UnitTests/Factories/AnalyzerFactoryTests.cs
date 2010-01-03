using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Camlex.NET.Impl.AndAlso;
using Camlex.NET.Impl.Eq;
using Camlex.NET.Impl.Factories;
using NUnit.Framework;

namespace Camlex.NET.UnitTests.Factories
{
    [TestFixture]
    public class AnalyzerFactoryTests
    {
        [Test]
        public void test_WHEN_expression_is_eq_THEN_eq_analyzer_is_created()
        {
            var analyzerFactory = new AnalyzerFactory(null);
            var analyzer = analyzerFactory.Create(ExpressionType.Equal);
            Assert.That(analyzer, Is.InstanceOf<EqAnalyzer>());
        }

        [Test]
        public void test_WHEN_expression_is_andalso_THEN_andalso_analyzer_is_created()
        {
            var analyzerFactory = new AnalyzerFactory(null);
            var analyzer = analyzerFactory.Create(ExpressionType.AndAlso);
            Assert.That(analyzer, Is.InstanceOf<AndAlsoAnalyzer>());
        }
    }
}
