using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Camlex.NET.Impl.Factories;
using Camlex.NET.Impl.Operations.AndAlso;
using Camlex.NET.Impl.Operations.Array;
using Camlex.NET.Impl.Operations.Eq;
using Camlex.NET.Impl.Operations.Geq;
using Camlex.NET.Impl.Operations.Gt;
using Camlex.NET.Impl.Operations.Leq;
using Camlex.NET.Impl.Operations.OrElse;
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

        [Test]
        public void test_WHEN_expression_is_orelse_THEN_orelse_analyzer_is_created()
        {
            var analyzerFactory = new AnalyzerFactory(null);
            var analyzer = analyzerFactory.Create(ExpressionType.OrElse);
            Assert.That(analyzer, Is.InstanceOf<OrElseAnalyzer>());
        }

        [Test]
        public void test_WHEN_expression_is_array_THEN_array_analyzer_is_created()
        {
            var analyzerFactory = new AnalyzerFactory(null);
            var analyzer = analyzerFactory.Create(ExpressionType.NewArrayInit);
            Assert.That(analyzer, Is.InstanceOf<ArrayAnalyzer>());
        }

        [Test]
        public void test_WHEN_expression_is_geq_THEN_geq_analyzer_is_created()
        {
            var analyzerFactory = new AnalyzerFactory(null);
            var analyzer = analyzerFactory.Create(ExpressionType.GreaterThanOrEqual);
            Assert.That(analyzer, Is.InstanceOf<GeqAnalyzer>());
        }

        [Test]
        public void test_WHEN_expression_is_gt_THEN_gt_analyzer_is_created()
        {
            var analyzerFactory = new AnalyzerFactory(null);
            var analyzer = analyzerFactory.Create(ExpressionType.GreaterThan);
            Assert.That(analyzer, Is.InstanceOf<GtAnalyzer>());
        }

        [Test]
        public void test_WHEN_expression_is_leq_THEN_leq_analyzer_is_created()
        {
            var analyzerFactory = new AnalyzerFactory(null);
            var analyzer = analyzerFactory.Create(ExpressionType.LessThanOrEqual);
            Assert.That(analyzer, Is.InstanceOf<LeqAnalyzer>());
        }
    }
}
