using System;
using System.Linq.Expressions;
using CamlexNET.Impl.Factories;
using CamlexNET.Impl.Operands;
using CamlexNET.Impl.Operations.AndAlso;
using CamlexNET.Impl.Operations.Array;
using CamlexNET.Impl.Operations.BeginsWith;
using CamlexNET.Impl.Operations.Contains;
using CamlexNET.Impl.Operations.Eq;
using CamlexNET.Impl.Operations.Geq;
using CamlexNET.Impl.Operations.Gt;
using CamlexNET.Impl.Operations.IsNotNull;
using CamlexNET.Impl.Operations.IsNull;
using CamlexNET.Impl.Operations.Leq;
using CamlexNET.Impl.Operations.Lt;
using CamlexNET.Impl.Operations.Neq;
using CamlexNET.Impl.Operations.OrElse;
using CamlexNET.Interfaces;
using Microsoft.SharePoint;
using NUnit.Framework;
using Rhino.Mocks;

namespace CamlexNET.UnitTests.Factories
{
    [TestFixture]
    public class AnalyzerFactoryTests
    {
        [Test]
        public void test_WHEN_expression_is_eq_THEN_eq_analyzer_is_created()
        {
            Expression<Func<SPItem, bool>> expr = x => (int)x["Count"] == 1;
            var operandBuilder = new OperandBuilder();
            var analyzerFactory = new AnalyzerFactory(operandBuilder, null);
            var analyzer = analyzerFactory.Create(expr);
            Assert.That(analyzer, Is.InstanceOf<EqAnalyzer>());
        }

        [Test]
        public void test_WHEN_expression_is_neq_THEN_neq_analyzer_is_created()
        {
            Expression<Func<SPItem, bool>> expr = x => (int)x["Count"] != 1;
            var operandBuilder = new OperandBuilder();
            var analyzerFactory = new AnalyzerFactory(operandBuilder, null);
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

        [Test]
        public void TestWhenExpressionIsBeginsWithThenBeginsWithAnalyzerIsCreated()
        {
            Expression<Func<SPItem, bool>> expr = x => ((string)x["Count"]).StartsWith("foo");
            var analyzerFactory = new AnalyzerFactory(null, null);
            var analyzer = analyzerFactory.Create(expr);
            Assert.That(analyzer, Is.InstanceOf<BeginsWithAnalyzer>());
        }

        [Test]
        public void TestWhenExpressionIsContainsThenContainsAnalyzerIsCreated()
        {
            Expression<Func<SPItem, bool>> expr = x => ((string)x["Count"]).Contains("foo");
            var analyzerFactory = new AnalyzerFactory(null, null);
            var analyzer = analyzerFactory.Create(expr);
            Assert.That(analyzer, Is.InstanceOf<ContainsAnalyzer>());
        }
    }
}
