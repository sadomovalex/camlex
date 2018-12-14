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
using System.Xml.Linq;
using CamlexNET.Impl.Operations.AndAlso;
using CamlexNET.Impl.ReverseEngeneering.Caml.Analyzers;
using CamlexNET.Interfaces.ReverseEngeneering;
using CamlexNET.UnitTests.ReverseEngeneering.Analyzers.TestBase;
using NUnit.Framework;

namespace CamlexNET.UnitTests.ReverseEngeneering.Analyzers
{
    internal class ReAndAlsoAnalyzerTest : ReCompositeExpressionTestBase<ReAndAlsoAnalyzer, AndAlsoOperation>
    {
        private readonly Func<XElement, IReOperandBuilder, IReAnalyzerFactory, ReAndAlsoAnalyzer>
            ANALYZER_CONSTRUCTOR = (el, operandBuilder, analyzerFactory) => new ReAndAlsoAnalyzer(el, operandBuilder, analyzerFactory);
        private const string OPERATION_NAME = Tags.And;
        private const string OPERATION_SYMBOL = "AndAlso";

        [Test]
        public void test_WHEN_xml_is_null_THEN_expression_is_not_valid()
        {
            BASE_test_WHEN_xml_is_null_THEN_expression_is_not_valid(ANALYZER_CONSTRUCTOR);
        }

        [Test]
        public void test_WHEN_neither_field_ref_nor_value_specified_THEN_expression_is_not_valid()
        {
            BASE_test_WHEN_neither_field_ref_nor_value_specified_THEN_expression_is_not_valid(ANALYZER_CONSTRUCTOR, OPERATION_NAME);
        }

        [Test]
        public void test_WHEN_both_suboperations_are_valid_THEN_expression_is_not_valid()
        {
            BASE_test_WHEN_both_suboperations_are_valid_THEN_expression_is_valid(ANALYZER_CONSTRUCTOR, OPERATION_NAME);
        }

        [Test]
        public void test_WHEN_either_suboperation_is_not_valid_THEN_expression_is_not_valid()
        {
            BASE_test_WHEN_either_suboperation_is_not_valid_THEN_expression_is_not_valid(ANALYZER_CONSTRUCTOR, OPERATION_NAME);
        }

        [Test]
        [ExpectedException(typeof(CamlAnalysisException))]
        public void test_WHEN_expression_is_not_valid_THEN_exception_is_thrown()
        {
            BASE_test_WHEN_expression_is_not_valid_THEN_exception_is_thrown(ANALYZER_CONSTRUCTOR);
        }

        [Test]
        public void test_WHEN_expression_is_valid_THEN_operation_is_returned()
        {
            BASE_test_WHEN_expression_is_valid_THEN_operation_is_returned(ANALYZER_CONSTRUCTOR, OPERATION_NAME, OPERATION_SYMBOL);
        }
    }
}