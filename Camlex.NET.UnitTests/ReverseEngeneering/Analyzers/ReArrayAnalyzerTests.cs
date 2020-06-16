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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CamlexNET.Impl.Operands;
using CamlexNET.Impl.Operations.Array;
using CamlexNET.Impl.ReverseEngeneering.Caml.Analyzers;
using CamlexNET.Interfaces;
using CamlexNET.Interfaces.ReverseEngeneering;
using NUnit.Framework;
using Rhino.Mocks;

namespace CamlexNET.UnitTests.ReverseEngeneering.Analyzers
{
    [TestFixture]
    public class ReArrayAnalyzerTests
    {
        [Test]
        public void test_WHEN_xml_is_null_THEN_expression_is_not_valid()
        {
            var analyzer = new ReArrayAnalyzer(null, null);
            Assert.IsFalse(analyzer.IsValid());
        }

        [Test]
        public void test_WHEN_no_field_refs_specified_THEN_expression_is_not_valid()
        {
            var xml =
                "  <OrderBy>" +
                "  </OrderBy>";

            var analyzer = new ReArrayAnalyzer(XmlHelper.Get(xml), null);
            Assert.IsFalse(analyzer.IsValid());
        }

        [Test]
        public void test_WHEN_1_field_ref_specified_THEN_expression_is_valid()
        {
            var xml =
                "  <OrderBy>" +
                "    <FieldRef Name=\"Title\" />" +
                "  </OrderBy>";

            var analyzer = new ReArrayAnalyzer(XmlHelper.Get(xml), null);
            Assert.IsTrue(analyzer.IsValid());
        }

        [Test]
        public void test_WHEN_2_field_refs_specified_THEN_expression_is_valid()
        {
            var xml =
                "  <OrderBy>" +
                "    <FieldRef Name=\"Title\" />" +
                "    <FieldRef Name=\"Status\" />" +
                "  </OrderBy>";

            var analyzer = new ReArrayAnalyzer(XmlHelper.Get(xml), null);
            Assert.IsTrue(analyzer.IsValid());
        }

        [Test]
        public void test_WHEN_expression_is_not_valid_THEN_exception_is_thrown()
        {
            var analyzer = new ReArrayAnalyzer(null, null);
            Assert.Throws<CamlAnalysisException>(() => analyzer.GetOperation());
        }

        [Test]
        public void test_WHEN_expression_is_valid_THEN_operation_is_returned()
        {
            var xml =
                "  <OrderBy>" +
                "    <FieldRef Name=\"Title\" />" +
                "    <FieldRef Name=\"Status\" Ascending=\"True\" />" +
                "  </OrderBy>";

            var b = MockRepository.GenerateStub<IReOperandBuilder>();
            b.Stub(c => c.CreateFieldRefOperand(null)).Return(new FieldRefOperand("1")).IgnoreArguments();
            b.Stub(c => c.CreateFieldRefOperandWithOrdering(null, null)).Return(new FieldRefOperandWithOrdering(new FieldRefOperand("2"), new Camlex.Asc())).IgnoreArguments();

            var analyzer = new ReArrayAnalyzer(XmlHelper.Get(xml), b);
            var operation = analyzer.GetOperation();
            Assert.IsInstanceOf<ArrayOperation>(operation);
            var arrayOperation = (ArrayOperation)operation;
            Assert.That(arrayOperation.ToExpression().ToString(), Is.EqualTo("new [] {x.get_Item(\"1\"), (x.get_Item(\"2\") As Asc)}"));
        }
    }
}
