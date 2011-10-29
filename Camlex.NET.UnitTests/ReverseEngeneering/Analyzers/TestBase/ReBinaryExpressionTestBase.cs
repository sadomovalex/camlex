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
using CamlexNET.Interfaces;
using CamlexNET.Interfaces.ReverseEngeneering;
using NUnit.Framework;
using Rhino.Mocks;

namespace CamlexNET.UnitTests.ReverseEngeneering.Analyzers.TestBase
{
    internal class ReBinaryExpressionTestBase<TAnalyzer, TOperation>
        where TAnalyzer : ReBinaryExpressionBaseAnalyzer
        where TOperation : BinaryOperationBase
    {
        #region Helpers

        internal enum OperationType
        {
            Equality, Comparison, Textual
        }

        class SupportedValueType
        {
            public Type SupportedType { get; set; }
            public List<string> ExamplesOfCorrectValue { get; set; }
            public List<string> ExamplesOfIncorrectValue { get; set; }
            public bool ComparisonOperationsSupport { get; set; }
            public bool TextualOperationsSupport { get; set; }
        }

        List<SupportedValueType> supportedTypesWithExamples = new List<SupportedValueType>
        {
            new SupportedValueType
            {
                SupportedType = typeof(DataTypes.Boolean),
                ExamplesOfCorrectValue = new List<string> { bool.TrueString, 1.ToString() },
                ExamplesOfIncorrectValue = new List<string> { string.Empty, DateTime.Now.ToString(), Camlex.Now, Guid.NewGuid().ToString(), 12345.ToString(), "string" },
                ComparisonOperationsSupport = false,
                TextualOperationsSupport = false
            },
            new SupportedValueType
            {
                SupportedType = typeof(DataTypes.DateTime),
                ExamplesOfCorrectValue = new List<string> { DateTime.Now.ToString(), Camlex.Now },
                ExamplesOfIncorrectValue = new List<string> { string.Empty, bool.TrueString, Guid.NewGuid().ToString(), 12345.ToString(), "string" },
                ComparisonOperationsSupport = true,
                TextualOperationsSupport = false
            },
            new SupportedValueType
            {
                SupportedType = typeof(DataTypes.Guid),
                ExamplesOfCorrectValue = new List<string> { Guid.NewGuid().ToString() },
                ExamplesOfIncorrectValue = new List<string> { string.Empty, bool.TrueString, DateTime.Now.ToString(), Camlex.Now, 12345.ToString(), "string" },
                ComparisonOperationsSupport = false,
                TextualOperationsSupport = false
            },
            new SupportedValueType
            {
                SupportedType = typeof(DataTypes.Integer),
                ExamplesOfCorrectValue = new List<string> { 12345.ToString() },
                ExamplesOfIncorrectValue = new List<string> { string.Empty, bool.TrueString, DateTime.Now.ToString(), Camlex.Now, Guid.NewGuid().ToString(), "string" },
                ComparisonOperationsSupport = true,
                TextualOperationsSupport = false
            },
            new SupportedValueType
            {
                SupportedType = typeof(DataTypes.Lookup),
                ExamplesOfCorrectValue = new List<string> { "lookup" },
                ExamplesOfIncorrectValue = new List<string> { string.Empty },
                ComparisonOperationsSupport = true,
                TextualOperationsSupport = false
            },
            new SupportedValueType
            {
                SupportedType = typeof(DataTypes.Text),
                ExamplesOfCorrectValue = new List<string> { string.Empty, "foo" },
                ExamplesOfIncorrectValue = new List<string>(),
                ComparisonOperationsSupport = true,
                TextualOperationsSupport = true
            }
            /*new SupportedValueType
            {
                SupportedType = typeof(DataTypes.AllDayEvent),
                ExamplesOfCorrectValue = new List<string> { string.Empty, "foo" },
                ExamplesOfIncorrectValue = new List<string>(),
                ComparisonOperationsSupport = false,
                TextualOperationsSupport = true
            },
            new SupportedValueType
            {
                SupportedType = typeof(DataTypes.Attachments),
                ExamplesOfCorrectValue = new List<string> { string.Empty, "foo" },
                ExamplesOfIncorrectValue = new List<string>(),
                ComparisonOperationsSupport = false,
                TextualOperationsSupport = true
            },
            new SupportedValueType
            {
                SupportedType = typeof(DataTypes.Calculated),
                ExamplesOfCorrectValue = new List<string> { string.Empty, "foo" },
                ExamplesOfIncorrectValue = new List<string>(),
                ComparisonOperationsSupport = false,
                TextualOperationsSupport = true
            },
            new SupportedValueType
            {
                SupportedType = typeof(DataTypes.Choice),
                ExamplesOfCorrectValue = new List<string> { string.Empty, "foo" },
                ExamplesOfIncorrectValue = new List<string>(),
                ComparisonOperationsSupport = false,
                TextualOperationsSupport = true
            },
            new SupportedValueType
            {
                SupportedType = typeof(DataTypes.Computed),
                ExamplesOfCorrectValue = new List<string> { string.Empty, "foo" },
                ExamplesOfIncorrectValue = new List<string>(),
                ComparisonOperationsSupport = false,
                TextualOperationsSupport = true
            },
            new SupportedValueType
            {
                SupportedType = typeof(DataTypes.ContentTypeId),
                ExamplesOfCorrectValue = new List<string> { string.Empty, "foo" },
                ExamplesOfIncorrectValue = new List<string>(),
                ComparisonOperationsSupport = false,
                TextualOperationsSupport = true
            },
            new SupportedValueType
            {
                SupportedType = typeof(DataTypes.Counter),
                ExamplesOfCorrectValue = new List<string> { string.Empty, "foo" },
                ExamplesOfIncorrectValue = new List<string>(),
                ComparisonOperationsSupport = false,
                TextualOperationsSupport = true
            },
            new SupportedValueType
            {
                SupportedType = typeof(DataTypes.CrossProjectLink),
                ExamplesOfCorrectValue = new List<string> { string.Empty, "foo" },
                ExamplesOfIncorrectValue = new List<string>(),
                ComparisonOperationsSupport = false,
                TextualOperationsSupport = true
            },
            new SupportedValueType
            {
                SupportedType = typeof(DataTypes.Currency),
                ExamplesOfCorrectValue = new List<string> { string.Empty, "foo" },
                ExamplesOfIncorrectValue = new List<string>(),
                ComparisonOperationsSupport = false,
                TextualOperationsSupport = true
            },
            new SupportedValueType
            {
                SupportedType = typeof(DataTypes.Error),
                ExamplesOfCorrectValue = new List<string> { string.Empty, "foo" },
                ExamplesOfIncorrectValue = new List<string>(),
                ComparisonOperationsSupport = false,
                TextualOperationsSupport = true
            },
            new SupportedValueType
            {
                SupportedType = typeof(DataTypes.File),
                ExamplesOfCorrectValue = new List<string> { string.Empty, "foo" },
                ExamplesOfIncorrectValue = new List<string>(),
                ComparisonOperationsSupport = false,
                TextualOperationsSupport = true
            },
            new SupportedValueType
            {
                SupportedType = typeof(DataTypes.GridChoice),
                ExamplesOfCorrectValue = new List<string> { string.Empty, "foo" },
                ExamplesOfIncorrectValue = new List<string>(),
                ComparisonOperationsSupport = false,
                TextualOperationsSupport = true
            },
            new SupportedValueType
            {
                SupportedType = typeof(DataTypes.Invalid),
                ExamplesOfCorrectValue = new List<string> { string.Empty, "foo" },
                ExamplesOfIncorrectValue = new List<string>(),
                ComparisonOperationsSupport = false,
                TextualOperationsSupport = true
            },
            new SupportedValueType
            {
                SupportedType = typeof(DataTypes.MaxItems),
                ExamplesOfCorrectValue = new List<string> { string.Empty, "foo" },
                ExamplesOfIncorrectValue = new List<string>(),
                ComparisonOperationsSupport = false,
                TextualOperationsSupport = true
            },
            new SupportedValueType
            {
                SupportedType = typeof(DataTypes.ModStat),
                ExamplesOfCorrectValue = new List<string> { string.Empty, "foo" },
                ExamplesOfIncorrectValue = new List<string>(),
                ComparisonOperationsSupport = false,
                TextualOperationsSupport = true
            },
            new SupportedValueType
            {
                SupportedType = typeof(DataTypes.MultiChoice),
                ExamplesOfCorrectValue = new List<string> { string.Empty, "foo" },
                ExamplesOfIncorrectValue = new List<string>(),
                ComparisonOperationsSupport = false,
                TextualOperationsSupport = true
            },
            new SupportedValueType
            {
                SupportedType = typeof(DataTypes.Note),
                ExamplesOfCorrectValue = new List<string> { string.Empty, "foo" },
                ExamplesOfIncorrectValue = new List<string>(),
                ComparisonOperationsSupport = true,
                TextualOperationsSupport = true
            },
            new SupportedValueType
            {
                SupportedType = typeof(DataTypes.Number),
                ExamplesOfCorrectValue = new List<string> { string.Empty, "foo" },
                ExamplesOfIncorrectValue = new List<string>(),
                ComparisonOperationsSupport = true,
                TextualOperationsSupport = true
            },
            new SupportedValueType
            {
                SupportedType = typeof(DataTypes.PageSeparator),
                ExamplesOfCorrectValue = new List<string> { string.Empty, "foo" },
                ExamplesOfIncorrectValue = new List<string>(),
                ComparisonOperationsSupport = false,
                TextualOperationsSupport = true
            },
            new SupportedValueType
            {
                SupportedType = typeof(DataTypes.Recurrence),
                ExamplesOfCorrectValue = new List<string> { string.Empty, "foo" },
                ExamplesOfIncorrectValue = new List<string>(),
                ComparisonOperationsSupport = false,
                TextualOperationsSupport = true
            },
            new SupportedValueType
            {
                SupportedType = typeof(DataTypes.Text),
                ExamplesOfCorrectValue = new List<string> { string.Empty, "foo" },
                ExamplesOfIncorrectValue = new List<string>(),
                ComparisonOperationsSupport = true,
                TextualOperationsSupport = true
            },
            new SupportedValueType
            {
                SupportedType = typeof(DataTypes.ThreadIndex),
                ExamplesOfCorrectValue = new List<string> { string.Empty, "foo" },
                ExamplesOfIncorrectValue = new List<string>(),
                ComparisonOperationsSupport = false,
                TextualOperationsSupport = true
            },
            new SupportedValueType
            {
                SupportedType = typeof(DataTypes.Threading),
                ExamplesOfCorrectValue = new List<string> { string.Empty, "foo" },
                ExamplesOfIncorrectValue = new List<string>(),
                ComparisonOperationsSupport = false,
                TextualOperationsSupport = true
            },
            new SupportedValueType
            {
                SupportedType = typeof(DataTypes.URL),
                ExamplesOfCorrectValue = new List<string> { string.Empty, "foo" },
                ExamplesOfIncorrectValue = new List<string>(),
                ComparisonOperationsSupport = false,
                TextualOperationsSupport = true
            },
            new SupportedValueType
            {
                SupportedType = typeof(DataTypes.User),
                ExamplesOfCorrectValue = new List<string> { string.Empty, "foo" },
                ExamplesOfIncorrectValue = new List<string>(),
                ComparisonOperationsSupport = false,
                TextualOperationsSupport = true
            },
            new SupportedValueType
            {
                SupportedType = typeof(DataTypes.WorkflowEventType),
                ExamplesOfCorrectValue = new List<string> { string.Empty, "foo" },
                ExamplesOfIncorrectValue = new List<string>(),
                ComparisonOperationsSupport = false,
                TextualOperationsSupport = true
            },
            new SupportedValueType
            {
                SupportedType = typeof(DataTypes.WorkflowStatus),
                ExamplesOfCorrectValue = new List<string> { string.Empty, "foo" },
                ExamplesOfIncorrectValue = new List<string>(),
                ComparisonOperationsSupport = false,
                TextualOperationsSupport = true
            }*/
        };

        #endregion

        internal void BASE_test_WHEN_xml_is_null_THEN_expression_is_not_valid
            (Func<XElement, IReOperandBuilder, TAnalyzer> constructor) 
        {
            var analyzer = constructor(null, null);
            Assert.IsFalse(analyzer.IsValid());
        }

        internal void BASE_test_WHEN_neither_field_ref_nor_value_specified_THEN_expression_is_not_valid
            (Func<XElement, IReOperandBuilder, TAnalyzer> constructor, string operationName) 
        {
            var xml = string.Format(
                "  <{0}>" +
                "  </{0}>",
                operationName);

            var analyzer = constructor(XmlHelper.Get(xml), null);
            Assert.IsFalse(analyzer.IsValid());
        }

        internal void BASE_test_WHEN_field_ref_specified_and_value_not_specified_THEN_expression_is_not_valid
            (Func<XElement, IReOperandBuilder, TAnalyzer> constructor, string operationName) 
        {
            var xml = string.Format(
                "<{0}>" +
                "    <FieldRef Name=\"Title\" />" +
                "</{0}>",
                operationName);

            var analyzer = constructor(XmlHelper.Get(xml), null);
            Assert.IsFalse(analyzer.IsValid());
        }

        internal void BASE_test_WHEN_field_ref_not_specified_and_value_specified_THEN_expression_is_not_valid
            (Func<XElement, IReOperandBuilder, TAnalyzer> constructor, string operationName) 
        {
            var xml = string.Format(
                "<{0}>" +
                "    <Value Type=\"Text\">testValue</Value>" +
                "</{0}>",
                operationName);

            var analyzer = constructor(XmlHelper.Get(xml), null);
            Assert.IsFalse(analyzer.IsValid());
        }

        internal void BASE_test_WHEN_field_ref_and_text_value_specified_THEN_expression_is_valid
            (Func<XElement, IReOperandBuilder, TAnalyzer> constructor, string operationName) 
        {
            var xml = string.Format(
                "<{0}>" +
                "    <FieldRef Name=\"Title\" />" +
                "    <Value Type=\"Text\">testValue</Value>" +
                "</{0}>",
                operationName);

            var analyzer = constructor(XmlHelper.Get(xml), new ReOperandBuilderFromCaml());
            Assert.IsTrue(analyzer.IsValid());
        }

        internal void BASE_test_WHEN_field_ref_without_name_attribute_and_text_value_specified_THEN_expression_is_not_valid
            (Func<XElement, IReOperandBuilder, TAnalyzer> constructor, string operationName)
        {
            var xml = string.Format(
                "<{0}>" +
                "    <FieldRef />" +
                "    <Value Type=\"Text\">testValue</Value>" +
                "</{0}>",
                operationName);

            var analyzer = constructor(XmlHelper.Get(xml), new ReOperandBuilderFromCaml());
            Assert.IsFalse(analyzer.IsValid());
        }

        internal void BASE_test_WHEN_field_ref_and_text_value_without_type_attribute_specified_THEN_expression_is_not_valid
            (Func<XElement, IReOperandBuilder, TAnalyzer> constructor, string operationName)
        {
            var xml = string.Format(
                "<{0}>" +
                "    <FieldRef Name=\"Title\" />" +
                "    <Value>testValue</Value>" +
                "</{0}>",
                operationName);

            var analyzer = constructor(XmlHelper.Get(xml), new ReOperandBuilderFromCaml());
            Assert.IsFalse(analyzer.IsValid());
        }

        internal void BASE_test_WHEN_supported_value_type_specified_THEN_expression_is_valid
            (Func<XElement, IReOperandBuilder, TAnalyzer> constructor, string operationName, OperationType operationType) 
        {
            typeof(DataTypes).GetMembers()
                .Where(x => x.MemberType == MemberTypes.NestedType).Select(x => x.Name).ToList().ForEach(x =>
                {
                    var foundItem = supportedTypesWithExamples
                        .Where(y => y.SupportedType.Name == x).FirstOrDefault();
                    if (foundItem == null ||
                        (operationType == OperationType.Comparison && !foundItem.ComparisonOperationsSupport) ||
                        (operationType == OperationType.Textual && !foundItem.TextualOperationsSupport)) return;

                    foundItem.ExamplesOfCorrectValue.ForEach(value =>
                    {
                        var xml = string.Format(
                            "<{0}>" +
                            "    <FieldRef Name=\"Title\" />" +
                            "    <Value Type=\"{1}\">{2}</Value>" +
                            "</{0}>",
                            operationName, foundItem.SupportedType.Name, value);

                        var analyzer = constructor(XmlHelper.Get(xml), new ReOperandBuilderFromCaml());
                        Assert.IsTrue(analyzer.IsValid());
                    });
                });
        }

        internal void BASE_test_WHEN_not_supported_value_type_specified_THEN_expression_is_not_valid
            (Func<XElement, IReOperandBuilder, TAnalyzer> constructor, string operationName, OperationType operationType) 
        {
            typeof(DataTypes).GetMembers()
                .Where(x => x.MemberType == MemberTypes.NestedType).Select(x => x.Name).ToList().ForEach(x =>
                {
                    var foundItem = supportedTypesWithExamples
                        .Where(y => y.SupportedType.Name == x).FirstOrDefault();
                    if (foundItem != null &&
                        !(operationType == OperationType.Comparison && !foundItem.ComparisonOperationsSupport) &&
                        !(operationType == OperationType.Textual && !foundItem.TextualOperationsSupport)) return;

                    var xml = string.Format(
                        "<{0}>" +
                        "    <FieldRef Name=\"Title\" />" +
                        "    <Value Type=\"{1}\">testValue</Value>" +
                        "</{0}>",
                    operationName, x);

                    var analyzer = constructor(XmlHelper.Get(xml), new ReOperandBuilderFromCaml());
                    Assert.IsFalse(analyzer.IsValid());
                });
        }

        internal void BASE_test_WHEN_supported_value_type_with_incorrect_value_specified_THEN_expression_is_not_valid
            (Func<XElement, IReOperandBuilder, TAnalyzer> constructor, string operationName, OperationType operationType)
        {
            typeof(DataTypes).GetMembers()
                .Where(x => x.MemberType == MemberTypes.NestedType).Select(x => x.Name).ToList().ForEach(x =>
                {
                    var foundItem = supportedTypesWithExamples
                        .Where(y => y.SupportedType.Name == x).FirstOrDefault();
                    if (foundItem == null ||
                        (operationType == OperationType.Comparison && !foundItem.ComparisonOperationsSupport) ||
                        (operationType == OperationType.Textual && !foundItem.TextualOperationsSupport)) return;

                    foundItem.ExamplesOfIncorrectValue.ForEach(value =>
                    {
                        var xml = string.Format(
                            "<{0}>" +
                            "    <FieldRef Name=\"Title\" />" +
                            "    <Value Type=\"{1}\">{2}</Value>" +
                            "</{0}>",
                            operationName, foundItem.SupportedType.Name, value);

                        var analyzer = constructor(XmlHelper.Get(xml), new ReOperandBuilderFromCaml());
                        Assert.IsFalse(analyzer.IsValid());
                    });
                });
        }

        internal void BASE_test_WHEN_expression_is_not_valid_THEN_exception_is_thrown
            (Func<XElement, IReOperandBuilder, TAnalyzer> constructor) 
        {
            var analyzer = constructor(null, null);
            analyzer.GetOperation();
        }

        internal void BASE_test_WHEN_expression_is_valid_THEN_operation_is_returned
            (Func<XElement, IReOperandBuilder, TAnalyzer> constructor, string operationName, OperationType operationType, string operationSymbol)
        {
            typeof(DataTypes).GetMembers()
                .Where(x => x.MemberType == MemberTypes.NestedType).Select(x => x.Name).ToList().ForEach(x =>
                {
                    var foundItem = supportedTypesWithExamples
                        .Where(y => y.SupportedType.Name == x).FirstOrDefault();
                    if (foundItem == null ||
                        (operationType == OperationType.Comparison && !foundItem.ComparisonOperationsSupport) ||
                        (operationType == OperationType.Textual && !foundItem.TextualOperationsSupport)) return;

                    foundItem.ExamplesOfCorrectValue.ForEach(value =>
                    {
                        var xml = string.Format(
                            "<{0}>" +
                            "    <FieldRef Name=\"Title\" />" +
                            "    <Value Type=\"{1}\">{2}</Value>" +
                            "</{0}>",
                            operationName, foundItem.SupportedType.Name, value);

                        var b = MockRepository.GenerateStub<IReOperandBuilder>();
                        b.Stub(c => c.CreateFieldRefOperand(null)).Return(new FieldRefOperand("Title")).IgnoreArguments();
                        if (operationName == Tags.Geq || operationName == Tags.Gt ||
                            operationName == Tags.Leq || operationName == Tags.Lt)
                        {
                            b.Stub(c => c.IsOperationComparison(null)).Return(true).IgnoreArguments();
                        }
                        else
                        {
                            b.Stub(c => c.IsOperationComparison(null)).Return(false).IgnoreArguments();
                        }

                        var valueOperand = default(IOperand);
                        if (operationType == OperationType.Equality)
                            valueOperand = new TextValueOperand(value);
                        if (operationType == OperationType.Comparison)
                            valueOperand = new GenericStringBasedValueOperand(foundItem.SupportedType, value);
                        if (operationType == OperationType.Textual)
                            valueOperand = new TextValueOperand(value);

                        b.Stub(c => c.CreateValueOperand(null)).Return(valueOperand).IgnoreArguments();
                        var analyzer = constructor(XmlHelper.Get(xml), b);
                        var operation = analyzer.GetOperation();
                        Assert.IsInstanceOf<TOperation>(operation);
                        var operationT = (TOperation)operation;

                        var exprectedResult = default(string);
                        if (operationType == OperationType.Equality)
                            exprectedResult = string.Format("(Convert(x.get_Item(\"Title\")) {0} \"{1}\")", operationSymbol, value);
                        if (operationType == OperationType.Comparison)
                            exprectedResult = string.Format("(x.get_Item(\"Title\") {0} Convert(Convert(\"{1}\")))", operationSymbol, value);
                        if (operationType == OperationType.Textual)
                            exprectedResult = string.Format("Convert(x.get_Item(\"Title\")).{0}(\"{1}\")", operationSymbol, value);

                        Assert.That(operationT.ToExpression().ToString(), Is.EqualTo(exprectedResult));
                    });
                });
        }
    }
}