#region Copyright(c) Alexey Sadomov, Vladimir Timashkov, Stef Heyenrath. All Rights Reserved.
// -----------------------------------------------------------------------------
// Copyright(c) 2010 Alexey Sadomov, Vladimir Timashkov, Stef Heyenrath. All Rights Reserved.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//
//   1. No Trademark License - Microsoft Public License (Ms-PL) does not grant you rights to use
//      authors names, logos, or trademarks.
//   2. If you distribute any portion of the software, you must retain all copyright,
//      patent, trademark, and attribution notices that are present in the software.
//   3. If you distribute any portion of the software in source code form, you may do
//      so only under this license by including a complete copy of Microsoft Public License (Ms-PL)
//      with your distribution. If you distribute any portion of the software in compiled
//      or object code form, you may only do so under a license that complies with
//      Microsoft Public License (Ms-PL).
//   4. The names of the authors may not be used to endorse or promote products
//      derived from this software without specific prior written permission.
//
// The software is licensed "as-is." You bear the risk of using it. The authors
// give no express warranties, guarantees or conditions. You may have additional consumer
// rights under your local laws which this license cannot change. To the extent permitted
// under your local laws, the authors exclude the implied warranties of merchantability,
// fitness for a particular purpose and non-infringement.
// -----------------------------------------------------------------------------
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CamlexNET.Impl.Factories;
using CamlexNET.Impl.Operands;
using CamlexNET.Impl.Operations.AndAlso;
using CamlexNET.Impl.Operations.Array;
using CamlexNET.Impl.Operations.BeginsWith;
using CamlexNET.Impl.Operations.Constant;
using CamlexNET.Impl.Operations.Contains;
using CamlexNET.Impl.Operations.Eq;
using CamlexNET.Impl.Operations.Geq;
using CamlexNET.Impl.Operations.Gt;
using CamlexNET.Impl.Operations.In;
using CamlexNET.Impl.Operations.IsNotNull;
using CamlexNET.Impl.Operations.IsNull;
using CamlexNET.Impl.Operations.Leq;
using CamlexNET.Impl.Operations.Lt;
using CamlexNET.Impl.Operations.Neq;
using CamlexNET.Impl.Operations.OrElse;
using CamlexNET.Interfaces;
using Microsoft.SharePoint.Client;
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
			Expression<Func<ListItem, bool>> expr = x => (int)x["Count"] == 1;
			var operandBuilder = new OperandBuilder();
			var analyzerFactory = new AnalyzerFactory(operandBuilder, null);
			var analyzer = analyzerFactory.Create(expr);
			Assert.That(analyzer, Is.InstanceOf<EqAnalyzer>());
		}

		[Test]
		public void test_WHEN_expression_is_neq_THEN_neq_analyzer_is_created()
		{
			Expression<Func<ListItem, bool>> expr = x => (int)x["Count"] != 1;
			var operandBuilder = new OperandBuilder();
			var analyzerFactory = new AnalyzerFactory(operandBuilder, null);
			var analyzer = analyzerFactory.Create(expr);
			Assert.That(analyzer, Is.InstanceOf<NeqAnalyzer>());
		}

		[Test]
		public void test_WHEN_expression_is_andalso_THEN_andalso_analyzer_is_created()
		{
			Expression<Func<ListItem, bool>> expr = x => (int)x["Count"] == 1 && (int)x["Count"] == 1;
			var analyzerFactory = new AnalyzerFactory(null, null);
			var analyzer = analyzerFactory.Create(expr);
			Assert.That(analyzer, Is.InstanceOf<AndAlsoAnalyzer>());
		}

		[Test]
		public void test_WHEN_expression_is_orelse_THEN_orelse_analyzer_is_created()
		{
			Expression<Func<ListItem, bool>> expr = x => (int)x["Count"] == 1 || (int)x["Count"] == 1;
			var analyzerFactory = new AnalyzerFactory(null, null);
			var analyzer = analyzerFactory.Create(expr);
			Assert.That(analyzer, Is.InstanceOf<OrElseAnalyzer>());
		}

		[Test]
		public void test_WHEN_expression_is_array_THEN_array_analyzer_is_created()
		{
			Expression<Func<ListItem, object[]>> expr = x => new[] { x["Count"] };
			var analyzerFactory = new AnalyzerFactory(null, null);
			var analyzer = analyzerFactory.Create(expr);
			Assert.That(analyzer, Is.InstanceOf<ArrayAnalyzer>());
		}

		[Test]
		public void test_WHEN_expression_is_geq_THEN_geq_analyzer_is_created()
		{
			Expression<Func<ListItem, bool>> expr = x => (int)x["Count"] >= 1;
			var analyzerFactory = new AnalyzerFactory(null, null);
			var analyzer = analyzerFactory.Create(expr);
			Assert.That(analyzer, Is.InstanceOf<GeqAnalyzer>());
		}

		[Test]
		public void test_WHEN_expression_is_gt_THEN_gt_analyzer_is_created()
		{
			Expression<Func<ListItem, bool>> expr = x => (int)x["Count"] > 1;
			var analyzerFactory = new AnalyzerFactory(null, null);
			var analyzer = analyzerFactory.Create(expr);
			Assert.That(analyzer, Is.InstanceOf<GtAnalyzer>());
		}

		[Test]
		public void test_WHEN_expression_is_leq_THEN_leq_analyzer_is_created()
		{
			Expression<Func<ListItem, bool>> expr = x => (int)x["Count"] <= 1;
			var analyzerFactory = new AnalyzerFactory(null, null);
			var analyzer = analyzerFactory.Create(expr);
			Assert.That(analyzer, Is.InstanceOf<LeqAnalyzer>());
		}

		[Test]
		public void test_WHEN_expression_is_lt_THEN_lt_analyzer_is_created()
		{
			Expression<Func<ListItem, bool>> expr = x => (int)x["Count"] < 1;
			var analyzerFactory = new AnalyzerFactory(null, null);
			var analyzer = analyzerFactory.Create(expr);
			Assert.That(analyzer, Is.InstanceOf<LtAnalyzer>());
		}

		[Test]
		public void test_WHEN_expression_is_isnotnull_THEN_isnotnull_analyzer_is_created()
		{
			var operandBuilder = MockRepository.GenerateStub<IOperandBuilder>();
			operandBuilder.Stub(b => b.CreateValueOperandForNativeSyntax(null)).Return(new NullValueOperand()).IgnoreArguments();

			Expression<Func<ListItem, bool>> expr = x => x["Count"] != null;

			var analyzerFactory = new AnalyzerFactory(operandBuilder, null);
			var analyzer = analyzerFactory.Create(expr);
			Assert.That(analyzer, Is.InstanceOf<IsNotNullAnalyzer>());
		}

		[Test]
		public void test_WHEN_expression_is_isnull_THEN_isnull_analyzer_is_created()
		{
			var operandBuilder = MockRepository.GenerateStub<IOperandBuilder>();
			operandBuilder.Stub(b => b.CreateValueOperandForNativeSyntax(null)).Return(new NullValueOperand()).IgnoreArguments();

			Expression<Func<ListItem, bool>> expr = x => x["Count"] == null;

			var analyzerFactory = new AnalyzerFactory(operandBuilder, null);
			var analyzer = analyzerFactory.Create(expr);
			Assert.That(analyzer, Is.InstanceOf<IsNullAnalyzer>());
		}

		[Test]
		public void test_WHEN_expression_IS_beginswith_THEN_beginswith_analyzer_IS_created()
		{
			Expression<Func<ListItem, bool>> expr = x => ((string)x["Count"]).StartsWith("foo");
			var analyzerFactory = new AnalyzerFactory(null, null);
			var analyzer = analyzerFactory.Create(expr);
			Assert.That(analyzer, Is.InstanceOf<BeginsWithAnalyzer>());
		}

		[Test]
		public void test_WHEN_expression_IS_contains_THEN_contains_analyzer_IS_created()
		{
			Expression<Func<ListItem, bool>> expr = x => ((string)x["Count"]).Contains("foo");
			var analyzerFactory = new AnalyzerFactory(null, null);
			var analyzer = analyzerFactory.Create(expr);
			Assert.That(analyzer, Is.InstanceOf<ContainsAnalyzer>());
        }

        [Test]
        public void test_WHEN_expression_IS_in_THEN_in_analyzer_IS_created()
        {
            Expression<Func<SPListItem, bool>> expr = x => (new[] { 0, 1, 2 }).Contains((int)x[new Guid("{1DF87A41-D795-4C0F-915F-DC3D54B296AA}")]);
            var analyzerFactory = new AnalyzerFactory(null, null);
            var analyzer = analyzerFactory.Create(expr);
            Assert.That(analyzer, Is.InstanceOf<InAnalyzer>());
		}

        [Test]
        public void test_WHEN_expression_IS_constant_THEN_constant_analyzer_IS_created()
        {
            int c = 10;
            Expression<Func<int>> expr = () => c;
            var analyzerFactory = new AnalyzerFactory(null, null);
            var analyzer = analyzerFactory.Create(expr);
            Assert.That(analyzer, Is.InstanceOf<ConstantAnalyzer>());
        }
	}
}
