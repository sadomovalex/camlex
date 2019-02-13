#region Copyright(c) Alexey Sadomov, Vladimir Timashkov. All Rights Reserved.
// -----------------------------------------------------------------------------
// Copyright(c) 2010 Alexey Sadomov, Vladimir Timashkov. All Rights Reserved.
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
using CamlexNET.Impl.Operations.Includes;
using CamlexNET.Interfaces;
using Microsoft.SharePoint;
using NUnit.Framework;
using Rhino.Mocks;

namespace CamlexNET.UnitTests.Operations.Includes
{
    [TestFixture]
    public class IncludesAnalyzerTests
    {
        [Test]
        public void test_THAT_includes_expression_with_string_type_IS_valid()
        {
            var analyzer = new IncludesAnalyzer(null, null);
            Expression<Func<SPListItem, bool>> expr = x => ((string)x["Count"]).Includes("foo");
            Assert.That(analyzer.IsValid(expr), Is.True);
        }

        [Test]
        public void test_THAT_includes_expression_with_string_type_and_lookupid_IS_valid()
        {
            var analyzer = new IncludesAnalyzer(null, null);
            Expression<Func<SPListItem, bool>> expr = x => ((string)x["Count"]).Includes("foo", true);
            Assert.That(analyzer.IsValid(expr), Is.True);
        }

        [Test]
        public void test_THAT_includes_expression_with_custom_text_type_IS_valid()
        {
            var analyzer = new IncludesAnalyzer(null, null);
            Expression<Func<SPListItem, bool>> expr = x => ((DataTypes.Text)x["Count"]).Includes("foo");
            Assert.That(analyzer.IsValid(expr), Is.True);
        }

        [Test]
        public void test_THAT_includes_expression_with_custom_note_type_IS_valid()
        {
            var analyzer = new IncludesAnalyzer(null, null);
            Expression<Func<SPListItem, bool>> expr = x => ((DataTypes.Note)x["Count"]).Includes("foo");
            Assert.That(analyzer.IsValid(expr), Is.True);
        }

        [Test]
        public void test_THAT_includes_expression_with_string_type_and_variable_IS_valid()
        {
            var analyzer = new IncludesAnalyzer(null, null);
            var stringVar = "Blah-blah-blah";
            Expression<Func<SPListItem, bool>> expr = x => ((string)x["Count"]).Includes(stringVar);
            Assert.That(analyzer.IsValid(expr), Is.True);
        }

        [Test]
        public void test_THAT_includes_expression_with_custom_text_type_and_variable_IS_valid()
        {
            var analyzer = new IncludesAnalyzer(null, null);
            var stringVar = "Blah-blah-blah";
            Expression<Func<SPListItem, bool>> expr = x => ((DataTypes.Text)x["Count"]).Includes(stringVar);
            Assert.That(analyzer.IsValid(expr), Is.True);
        }

        [Test]
        public void test_THAT_includes_expression_with_custom_note_type_and_variable_IS_valid()
        {
            var analyzer = new IncludesAnalyzer(null, null);
            var stringVar = "Blah-blah-blah";
            Expression<Func<SPListItem, bool>> expr = x => ((DataTypes.Note)x["Count"]).Includes(stringVar);
            Assert.That(analyzer.IsValid(expr), Is.True);
        }

        [Test]
        public void test_THAT_includes_expression_IS_determined_properly()
        {
            // arrange
            Expression<Func<SPListItem, bool>> expr = x => ((string)x["Count"]).Includes("foo");
            var operandBuilder = MockRepository.GenerateStub<IOperandBuilder>();
            operandBuilder.Stub(b => b.CreateFieldRefOperand(((MethodCallExpression)expr.Body).Object, null)).Return(null);
            operandBuilder.Stub(b => b.CreateValueOperandForNativeSyntax(((MethodCallExpression)expr.Body).Arguments[0])).Return(null);
            var analyzer = new IncludesAnalyzer(null, operandBuilder);

            // act
            var operation = analyzer.GetOperation(expr);

            //assert
            Assert.That(operation, Is.InstanceOf<IncludesOperation>());
        }

        [Test]
        public void test_THAT_includes_expression_with_string_type_IS_determined_Properly()
        {
            // arrange
            Expression<Func<SPListItem, bool>> expr = x => ((string)x["Count"]).Includes("foo");
            var operandBuilder = MockRepository.GenerateStub<IOperandBuilder>();
            operandBuilder.Stub(b => b.CreateFieldRefOperand(((MethodCallExpression)expr.Body).Object, null)).Return(null);
            operandBuilder.Stub(b => b.CreateValueOperandForNativeSyntax(((MethodCallExpression)expr.Body).Arguments[0])).Return(null);
            var analyzer = new IncludesAnalyzer(null, operandBuilder);

            // act
            var operation = analyzer.GetOperation(expr);

            //assert
            Assert.That(operation, Is.InstanceOf<IncludesOperation>());
        }

        [Test]
        public void test_THAT_includes_expression_with_custom_note_type_IS_determined_properly()
        {
            // arrange
            Expression<Func<SPListItem, bool>> expr = x => ((DataTypes.Note)x["Count"]).Includes("foo");
            var operandBuilder = MockRepository.GenerateStub<IOperandBuilder>();
            operandBuilder.Stub(b => b.CreateFieldRefOperand(((MethodCallExpression)expr.Body).Object, null)).Return(null);
            operandBuilder.Stub(b => b.CreateValueOperandForNativeSyntax(((MethodCallExpression)expr.Body).Arguments[0])).Return(null);
            var analyzer = new IncludesAnalyzer(null, operandBuilder);

            // act
            var operation = analyzer.GetOperation(expr);

            //assert
            Assert.That(operation, Is.InstanceOf<IncludesOperation>());
        }
    }
}
