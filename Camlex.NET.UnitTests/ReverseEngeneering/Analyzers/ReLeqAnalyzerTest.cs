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
using CamlexNET.Impl.Operations.Leq;
using CamlexNET.Impl.ReverseEngeneering.Caml.Analyzers;
using CamlexNET.Interfaces.ReverseEngeneering;
using CamlexNET.UnitTests.ReverseEngeneering.Analyzers.TestBase;
using NUnit.Framework;

namespace CamlexNET.UnitTests.ReverseEngeneering.Analyzers
{
    internal class ReLeqAnalyzerTest : ReBinaryExpressionTestBase<ReLeqAnalyzer, LeqOperation>
    {
        private readonly Func<XElement, IReOperandBuilder, ReLeqAnalyzer>
            ANALYZER_CONSTRUCTOR = (el, operandBuilder) => new ReLeqAnalyzer(el, operandBuilder);
        private const string OPERATION_NAME = Tags.Leq;
        private const string OPERATION_SYMBOL = ReflectionHelper.LessThanOrEqualMethodSymbol;

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
        public void test_WHEN_field_ref_specified_and_value_not_specified_THEN_expression_is_not_valid()
        {
            BASE_test_WHEN_field_ref_specified_and_value_not_specified_THEN_expression_is_not_valid(ANALYZER_CONSTRUCTOR, OPERATION_NAME);
        }

        [Test]
        public void test_WHEN_field_ref_not_specified_and_value_specified_THEN_expression_is_not_valid()
        {
            BASE_test_WHEN_field_ref_not_specified_and_value_specified_THEN_expression_is_not_valid(ANALYZER_CONSTRUCTOR, OPERATION_NAME);
        }

        [Test]
        public void test_WHEN_field_ref_and_text_value_specified_THEN_expression_is_valid()
        {
            BASE_test_WHEN_field_ref_and_text_value_specified_THEN_expression_is_valid(ANALYZER_CONSTRUCTOR, OPERATION_NAME);
        }

        [Test]
        public void test_WHEN_field_ref_without_name_attribute_and_text_value_specified_THEN_expression_is_not_valid()
        {
            BASE_test_WHEN_field_ref_without_name_attribute_and_text_value_specified_THEN_expression_is_not_valid(ANALYZER_CONSTRUCTOR, OPERATION_NAME);
        }

        [Test]
        public void test_WHEN_field_ref_and_text_value_without_type_attribute_specified_THEN_expression_is_not_valid()
        {
            BASE_test_WHEN_field_ref_and_text_value_without_type_attribute_specified_THEN_expression_is_not_valid(ANALYZER_CONSTRUCTOR, OPERATION_NAME);
        }

        [Test]
        public void test_WHEN_supported_value_type_specified_THEN_expression_is_valid()
        {
            BASE_test_WHEN_supported_value_type_specified_THEN_expression_is_valid(ANALYZER_CONSTRUCTOR, OPERATION_NAME, OperationType.Comparison);
        }

        [Test]
        public void test_WHEN_not_supported_value_type_specified_THEN_expression_is_not_valid()
        {
            BASE_test_WHEN_not_supported_value_type_specified_THEN_expression_is_not_valid(ANALYZER_CONSTRUCTOR, OPERATION_NAME, OperationType.Comparison);
        }

        [Test]
        public void test_WHEN_supported_value_type_with_incorrect_value_specified_THEN_expression_is_not_valid()
        {
            BASE_test_WHEN_supported_value_type_with_incorrect_value_specified_THEN_expression_is_not_valid(ANALYZER_CONSTRUCTOR, OPERATION_NAME, OperationType.Textual);
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
            BASE_test_WHEN_expression_is_valid_THEN_operation_is_returned(ANALYZER_CONSTRUCTOR, OPERATION_NAME, OperationType.Comparison, OPERATION_SYMBOL);
        }
    }
}