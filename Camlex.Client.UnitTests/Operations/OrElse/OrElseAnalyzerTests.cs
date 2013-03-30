﻿#region Copyright(c) Alexey Sadomov, Vladimir Timashkov, Stef Heyenrath. All Rights Reserved.
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
using CamlexNET.Impl.Factories;
using CamlexNET.Impl.Operations.OrElse;
using CamlexNET.Interfaces;
using Microsoft.SharePoint.Client;
using NUnit.Framework;
using Rhino.Mocks;

namespace CamlexNET.UnitTests.Operations.OrElse
{
	[TestFixture]
	public class OrElseAnalyzerTests
	{
		[Test]
		public void test_THAT_logical_or_expression_IS_valid_expression()
		{
			Expression<Func<ListItem, bool>> expr = x => (string)x["Email"] == "test@example.com" ||
													   (int)x["Count1"] == 1;
			var operandBuilder = new OperandBuilder();
			var analyzerFactory = new AnalyzerFactory(operandBuilder, null);
			var analyzer = new OrElseAnalyzer(null, analyzerFactory);
			Assert.That(analyzer.IsValid(expr), Is.True);
		}

		[Test]
		public void test_THAT_binary_or_expression_IS_not_valid_expression()
		{
			Expression<Func<ListItem, bool>> expr = x => (string)x["Email"] == "test@example.com" |
													   (int)x["Count1"] == 1;
			var operandBuilder = new OperandBuilder();
			var analyzerFactory = new AnalyzerFactory(operandBuilder, null);
			var analyzer = new OrElseAnalyzer(null, analyzerFactory);
			Assert.That(analyzer.IsValid(expr), Is.False);
		}

		[Test]
		public void test_THAT_valid_orelse_expression_IS_recognized_successfully()
		{
			// arrange
			var analyzerFactoryStub = MockRepository.GenerateStub<IAnalyzerFactory>();
			var analyzerStub = MockRepository.GenerateStub<IAnalyzer>();
			analyzerFactoryStub.Stub(f => f.Create(null)).Return(analyzerStub).IgnoreArguments();
			analyzerStub.Stub(a => a.IsValid(null)).Return(true).IgnoreArguments();
			var analyzer = new OrElseAnalyzer(null, analyzerFactoryStub);


			Expression<Func<ListItem, bool>> expr = x => (string)x["Email"] == "test@example.com" ||
													   (int)x["Count1"] == 1;
			// act
			var operation = analyzer.GetOperation(expr);

			// assert
			Assert.That(operation, Is.InstanceOf<OrElseOperation>());
		}
	}
}


