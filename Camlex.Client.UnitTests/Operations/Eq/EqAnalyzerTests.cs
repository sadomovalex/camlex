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
using CamlexNET.Impl.Operations.Eq;
using CamlexNET.Interfaces;
using Microsoft.SharePoint.Client;
using NUnit.Framework;
using Rhino.Mocks;

namespace CamlexNET.UnitTests.Operations.Eq
{
	[TestFixture]
	public class EqAnalyzerTests
	{
		[Test]
		public void test_THAT_eq_expression_IS_valid()
		{
			var analyzer = new EqAnalyzer(null, null);
			Expression<Func<ListItem, bool>> expr = x => (string)x["Title"] == "testValue";
			Assert.That(analyzer.IsValid(expr), Is.True);
		}

		[Test]
		public void test_THAT_string_based_eq_expression_IS_valid()
		{
			var analyzer = new EqAnalyzer(null, null);
			Expression<Func<ListItem, bool>> expr = x => x["Title"] == (DataTypes.Text)"testValue";
			Assert.That(analyzer.IsValid(expr), Is.True);
		}

		[Test]
		public void test_THAT_string_based_eq_expression_IS_not_valid()
		{
			var analyzer = new EqAnalyzer(null, null);
			Expression<Func<ListItem, bool>> expr = x => x["Title"] == (DataTypes.Text)new BaseFieldType();
			Assert.That(analyzer.IsValid(expr), Is.False);
		}

		[Test]
		public void test_THAT_string_based_eq_expression_with_variable_IS_not_valid()
		{
			var analyzer = new EqAnalyzer(null, null);
			var foo = new DataTypes.Text();
			Expression<Func<ListItem, bool>> expr = x => x["Title"] == foo;
			Assert.That(analyzer.IsValid(expr), Is.False);
		}

		[Test]
		public void test_THAT_eq_expression_IS_determined_properly()
		{
			// arrange
			Expression<Func<ListItem, bool>> expr = x => (string)x["Title"] == "testValue";

			var operandBuilder = MockRepository.GenerateStub<IOperandBuilder>();
			operandBuilder.Stub(b => b.CreateFieldRefOperand(expr.Body, null)).Return(null);
			operandBuilder.Stub(b => b.CreateValueOperandForNativeSyntax(expr.Body)).Return(null);

			var analyzer = new EqAnalyzer(null, operandBuilder);

			// act
			var operation = analyzer.GetOperation(expr);

			//assert
			Assert.That(operation, Is.InstanceOf<EqOperation>());
		}

		[Test]
		public void test_THAT_string_based_eq_expression_IS_determined_properly()
		{
			// arrange
			Expression<Func<ListItem, bool>> expr = x => x["Title"] == (DataTypes.Text)"testValue";

			var operandBuilder = MockRepository.GenerateStub<IOperandBuilder>();
			operandBuilder.Stub(b => b.CreateFieldRefOperand(expr.Body, null)).Return(null);
			operandBuilder.Stub(b => b.CreateValueOperandForNativeSyntax(expr.Body)).Return(null);

			var analyzer = new EqAnalyzer(null, operandBuilder);

			// act
			var operation = analyzer.GetOperation(expr);

			//assert
			Assert.That(operation, Is.InstanceOf<EqOperation>());
		}
	}
}


