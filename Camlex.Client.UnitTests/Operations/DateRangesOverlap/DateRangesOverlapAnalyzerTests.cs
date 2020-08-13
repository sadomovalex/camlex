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
using System.Linq.Expressions;
using CamlexNET.Impl.Operations.DateRangesOverlap;
using CamlexNET.Interfaces;
using Microsoft.SharePoint.Client;
using NUnit.Framework;
using Rhino.Mocks;

namespace CamlexNET.UnitTests.Operations.DateRangesOverlap
{
	[TestFixture]
	public class DateRangesOverlapAnalyzerTests
	{
		[Test]
		public void test_THAT_daterangesoverlap_expression_with_native_constant_IS_valid()
		{
			var analyzer = new DateRangesOverlapAnalyzer(null, null);
			Expression<Func<ListItem, bool>> expr = x => Camlex.DateRangesOverlap(x["start"], x["stop"], x["recurrence"], DateTime.Now);
			Assert.That(analyzer.IsValid(expr), Is.True);
		}

		[Test]
		public void test_THAT_daterangesoverlap_expression_with_native_variable_IS_valid()
		{
			var analyzer = new DateRangesOverlapAnalyzer(null, null);
			var now = DateTime.Now;
			Expression<Func<ListItem, bool>> expr = x => Camlex.DateRangesOverlap(x["start"], x["stop"], x["recurrence"], now);
			Assert.That(analyzer.IsValid(expr), Is.True);
		}

		[Test]
		public void test_THAT_daterangesoverlap_expression_with_string_constant_IS_valid()
		{
			var analyzer = new DateRangesOverlapAnalyzer(null, null);
			Expression<Func<ListItem, bool>> expr = x => Camlex.DateRangesOverlap(x["start"], x["stop"], x["recurrence"], ((DataTypes.DateTime)"02.01.2010 03:04:05"));
			Assert.That(analyzer.IsValid(expr), Is.True);
		}

		[Test]
		public void test_THAT_daterangesoverlap_expression_with_string_variable_IS_valid()
		{
			var analyzer = new DateRangesOverlapAnalyzer(null, null);
			const string now = "02.01.2010 03:04:05";
			Expression<Func<ListItem, bool>> expr = x => Camlex.DateRangesOverlap(x["start"], x["stop"], x["recurrence"], ((DataTypes.DateTime)now));
			Assert.That(analyzer.IsValid(expr), Is.True);
		}

		[Test]
		public void test_THAT_daterangesoverlap_expression_with_special_constant_IS_valid()
		{
			var analyzer = new DateRangesOverlapAnalyzer(null, null);
			Expression<Func<ListItem, bool>> expr = x => Camlex.DateRangesOverlap(x["start"], x["stop"], x["recurrence"], (DataTypes.DateTime)Camlex.Month);
			Assert.That(analyzer.IsValid(expr), Is.True);
		}

        [Test]
        public void test_THAT_daterangesoverlap_expression_IS_determined_properly()
        {
            // arrange
            Expression<Func<ListItem, bool>> expr = x => Camlex.DateRangesOverlap(x["start"], x["stop"], x["recurrence"], DateTime.Now);

            var operandBuilder = MockRepository.GenerateStub<IOperandBuilder>();
            operandBuilder.Stub(b => b.CreateFieldRefOperand(expr.Body, null)).Return(null);
            operandBuilder.Stub(b => b.CreateValueOperandForNativeSyntax(expr.Body)).Return(null);

            var analyzer = new DateRangesOverlapAnalyzer(null, operandBuilder);

            // act
            var operation = analyzer.GetOperation(expr);

            //assert
            Assert.That(operation, Is.InstanceOf<DateRangesOverlapOperation>());
        }
    }
}
