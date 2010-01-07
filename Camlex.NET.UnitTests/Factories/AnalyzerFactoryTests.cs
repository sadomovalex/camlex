using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Camlex.NET.Impl.Factories;
using Camlex.NET.Impl.Operands;
using Camlex.NET.Impl.Operations.AndAlso;
using Camlex.NET.Impl.Operations.Array;
using Camlex.NET.Impl.Operations.Eq;
using Camlex.NET.Impl.Operations.Geq;
using Camlex.NET.Impl.Operations.Gt;
using Camlex.NET.Impl.Operations.IsNotNull;
using Camlex.NET.Impl.Operations.IsNull;
using Camlex.NET.Impl.Operations.Leq;
using Camlex.NET.Impl.Operations.Lt;
using Camlex.NET.Impl.Operations.Neq;
using Camlex.NET.Impl.Operations.OrElse;
using Camlex.NET.Interfaces;
using Microsoft.SharePoint;
using NUnit.Framework;
using Rhino.Mocks;

namespace Camlex.NET.UnitTests.Factories
{
    [TestFixture]
    public class AnalyzerFactoryTests
    {
        [Test]
        public void test_WHEN_expression_is_eq_THEN_eq_analyzer_is_created()
        {
            Expression<Func<SPItem, bool>> expr = x => (int)x["Count"] == 1;
            var analyzerFactory = new AnalyzerFactory(null, null);
            var analyzer = analyzerFactory.Create(expr);
            Assert.That(analyzer, Is.InstanceOf<EqAnalyzer>());
        }

        [Test]
        public void test_WHEN_expression_is_neq_THEN_neq_analyzer_is_created()
        {
            Expression<Func<SPItem, bool>> expr = x => (int)x["Count"] != 1;
            var analyzerFactory = new AnalyzerFactory(null, null);
            var analyzer = analyzerFactory.Create(expr);
            Assert.That(analyzer, Is.InstanceOf<NeqAnalyzer>());
        }

        [Test]
        public void test_WHEN_expression_is_andalso_THEN_andalso_analyzer_is_created()
        {
            Expression<Func<SPItem, bool>> expr = x => (int)x["Count"] == 1 && (int)x["Count"] == 1;
            var analyzerFactory = new AnalyzerFactory(null, null);
            var analyzer = analyzerFactory.Create(expr);
            Assert.That(analyzer, Is.InstanceOf<AndAlsoAnalyzer>());
        }

        [Test]
        public void test_WHEN_expression_is_orelse_THEN_orelse_analyzer_is_created()
        {
            Expression<Func<SPItem, bool>> expr = x => (int)x["Count"] == 1 || (int)x["Count"] == 1;
            var analyzerFactory = new AnalyzerFactory(null, null);
            var analyzer = analyzerFactory.Create(expr);
            Assert.That(analyzer, Is.InstanceOf<OrElseAnalyzer>());
        }

        [Test]
        public void test_WHEN_expression_is_array_THEN_array_analyzer_is_created()
        {
            Expression<Func<SPItem, object[]>> expr = x => new[] { x["Count"] };
            var analyzerFactory = new AnalyzerFactory(null, null);
            var analyzer = analyzerFactory.Create(expr);
            Assert.That(analyzer, Is.InstanceOf<ArrayAnalyzer>());
        }

        [Test]
        public void test_WHEN_expression_is_geq_THEN_geq_analyzer_is_created()
        {
            Expression<Func<SPItem, bool>> expr = x => (int)x["Count"] >= 1;
            var analyzerFactory = new AnalyzerFactory(null, null);
            var analyzer = analyzerFactory.Create(expr);
            Assert.That(analyzer, Is.InstanceOf<GeqAnalyzer>());
        }

        [Test]
        public void test_WHEN_expression_is_gt_THEN_gt_analyzer_is_created()
        {
            Expression<Func<SPItem, bool>> expr = x => (int)x["Count"] > 1;
            var analyzerFactory = new AnalyzerFactory(null, null);
            var analyzer = analyzerFactory.Create(expr);
            Assert.That(analyzer, Is.InstanceOf<GtAnalyzer>());
        }

        [Test]
        public void test_WHEN_expression_is_leq_THEN_leq_analyzer_is_created()
        {
            Expression<Func<SPItem, bool>> expr = x => (int)x["Count"] <= 1;
            var analyzerFactory = new AnalyzerFactory(null, null);
            var analyzer = analyzerFactory.Create(expr);
            Assert.That(analyzer, Is.InstanceOf<LeqAnalyzer>());
        }

        [Test]
        public void test_WHEN_expression_is_lt_THEN_lt_analyzer_is_created()
        {
            Expression<Func<SPItem, bool>> expr = x => (int)x["Count"] < 1;
            var analyzerFactory = new AnalyzerFactory(null, null);
            var analyzer = analyzerFactory.Create(expr);
            Assert.That(analyzer, Is.InstanceOf<LtAnalyzer>());
        }

        [Test]
        public void test_WHEN_expression_is_isnotnull_THEN_isnotnull_analyzer_is_created()
        {
            var operandBuilder = MockRepository.GenerateStub<IOperandBuilder>();
            operandBuilder.Stub(b => b.CreateValueOperandForNativeSyntax(null)).Return(new NullValueOperand()).IgnoreArguments();

            Expression<Func<SPItem, bool>> expr = x => x["Count"] != null;
            
            var analyzerFactory = new AnalyzerFactory(operandBuilder, null);
            var analyzer = analyzerFactory.Create(expr);
            Assert.That(analyzer, Is.InstanceOf<IsNotNullAnalyzer>());
        }

        [Test]
        public void test_WHEN_expression_is_isnull_THEN_isnull_analyzer_is_created()
        {
            var operandBuilder = MockRepository.GenerateStub<IOperandBuilder>();
            operandBuilder.Stub(b => b.CreateValueOperandForNativeSyntax(null)).Return(new NullValueOperand()).IgnoreArguments();

            Expression<Func<SPItem, bool>> expr = x => x["Count"] == null;

            var analyzerFactory = new AnalyzerFactory(operandBuilder, null);
            var analyzer = analyzerFactory.Create(expr);
            Assert.That(analyzer, Is.InstanceOf<IsNullAnalyzer>());
        }
    }
}
