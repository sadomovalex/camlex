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
using System.Reflection;
using System.Xml.Linq;
using CamlexNET.Impl;
using CamlexNET.Impl.Operands;
using CamlexNET.Impl.ReverseEngeneering;
using CamlexNET.Impl.ReverseEngeneering.Caml.Factories;
using CamlexNET.Interfaces.ReverseEngeneering;
using NUnit.Framework;
using Rhino.Mocks;

namespace CamlexNET.UnitTests.ReverseEngeneering.Analyzers.TestBase
{
    internal class ReCompositeExpressionTestBase<TAnalyzer, TOperation>
        where TAnalyzer : ReCompositeExpressionBaseAnalyzer
        where TOperation : CompositeOperationBase
    {
        #region Helpers

        private List<string> SupportedSubOperations = new List<string>
        {
            Tags.And, Tags.Or, Tags.Eq, Tags.Neq,
            Tags.Geq, Tags.Gt, Tags.Leq, Tags.Lt,
            Tags.BeginsWith, Tags.Contains,
            Tags.IsNull, Tags.IsNotNull, Tags.DateRangesOverlap
        };

        #endregion

        internal void BASE_test_WHEN_xml_is_null_THEN_expression_is_not_valid
            (Func<XElement, IReOperandBuilder, IReAnalyzerFactory, TAnalyzer> constructor) 
        {
            var analyzer = constructor(null, null, null);
            Assert.IsFalse(analyzer.IsValid());
        }

        internal void BASE_test_WHEN_neither_field_ref_nor_value_specified_THEN_expression_is_not_valid
            (Func<XElement, IReOperandBuilder, IReAnalyzerFactory, TAnalyzer> constructor, string operationName) 
        {
            var xml = string.Format(
                "  <{0}>" +
                "  </{0}>",
                operationName);

            var analyzer = constructor(XmlHelper.Get(xml), null, null);
            Assert.IsFalse(analyzer.IsValid());
        }

        internal void BASE_test_WHEN_both_suboperations_are_valid_THEN_expression_is_valid
            (Func<XElement, IReOperandBuilder, IReAnalyzerFactory, TAnalyzer> constructor, string operationName)
        {
            var allTags = typeof(Tags).GetMembers()
                .Where(x => x.MemberType == MemberTypes.Field).Select(x => x.Name).ToList();
            allTags.ForEach(first =>
            {
                int t;
                allTags.ForEach(second =>
                {
                    if (!SupportedSubOperations.Any(x => string.Compare(x, first, false) == 0) ||
                        !SupportedSubOperations.Any(x => string.Compare(x, second, false) == 0)) return;

                    var xml = string.Format(
                        "<{0}>" +
                        "    <{1}></{1}>" +
                        "    <{2}></{2}>" +
                        "</{0}>",
                        operationName, first, second);

                    var reAanalyzer = MockRepository.GenerateStub<IReAnalyzer>();
                    reAanalyzer.Stub(x => x.IsValid()).Return(true);
                    var reAanalyzerFactory = MockRepository.GenerateStub<IReAnalyzerFactory>();
                    reAanalyzerFactory.Stub(c => c.Create(null)).Return(reAanalyzer).IgnoreArguments();

                    var analyzer = constructor(XmlHelper.Get(xml), null, reAanalyzerFactory);
                    Assert.IsTrue(analyzer.IsValid());
                });
            });
        }

        internal void BASE_test_WHEN_either_suboperation_is_not_valid_THEN_expression_is_not_valid
            (Func<XElement, IReOperandBuilder, IReAnalyzerFactory, TAnalyzer> constructor, string operationName)
        {
            var allTags = typeof(Tags).GetMembers()
                .Where(x => x.MemberType == MemberTypes.Field).Select(x => x.Name).ToList();
            allTags.ForEach(first =>
            {
                int t;
                allTags.ForEach(second =>
                {
                    if (SupportedSubOperations.Any(x => string.Compare(x, first, false) == 0) &&
                        SupportedSubOperations.Any(x => string.Compare(x, second, false) == 0)) return;

                    var xml = string.Format(
                        "<{0}>" +
                        "    <{1}></{1}>" +
                        "    <{2}></{2}>" +
                        "</{0}>",
                        operationName, first, second);

                    var reAanalyzer = MockRepository.GenerateStub<IReAnalyzer>();
                    reAanalyzer.Stub(x => x.IsValid()).Return(true);
                    var reAanalyzerFactory = MockRepository.GenerateStub<IReAnalyzerFactory>();
                    reAanalyzerFactory.Stub(c => c.Create(null)).Return(reAanalyzer).IgnoreArguments();

                    var analyzer = constructor(XmlHelper.Get(xml), null, reAanalyzerFactory);
                    Assert.IsFalse(analyzer.IsValid());
                });
            });
        }

        internal void BASE_test_WHEN_expression_is_valid_THEN_operation_is_returned
            (Func<XElement, IReOperandBuilder, IReAnalyzerFactory, TAnalyzer> constructor, string operationName, string operationSymbol)
        {
            var xml = string.Format(
                "<{0}>" +
                "    <Eq>" +
                "        <FieldRef Name=\"Title\" />" +
                "        <Value Type=\"Text\">testValue</Value>" +
                "    </Eq>" +
                "    <Eq>" +
                "        <FieldRef Name=\"Title\" />" +
                "        <Value Type=\"Text\">testValue</Value>" +
                "    </Eq>" +
                "</{0}>",
                operationName);

            var reOperandBuilder = MockRepository.GenerateStub<IReOperandBuilder>();
            reOperandBuilder.Stub(c => c.CreateFieldRefOperand(null)).Return(new FieldRefOperand("Title")).IgnoreArguments();
            reOperandBuilder.Stub(c => c.CreateValueOperand(null)).Return(new TextValueOperand("testValue")).IgnoreArguments();

            var reAnalyzerFactory = new ReAnalyzerFromCamlFactory(reOperandBuilder);
            var analyzer = constructor(XmlHelper.Get(xml), reOperandBuilder, reAnalyzerFactory);
            var operation = analyzer.GetOperation();
            Assert.IsInstanceOf<TOperation>(operation);
            var operationT = (TOperation)operation;
            Assert.That(operationT.ToExpression().ToString(), Is.EqualTo(
                string.Format("((Convert(x.get_Item(\"Title\")) = \"testValue\") {0} (Convert(x.get_Item(\"Title\")) = \"testValue\"))", operationSymbol)));
        }
   }
}