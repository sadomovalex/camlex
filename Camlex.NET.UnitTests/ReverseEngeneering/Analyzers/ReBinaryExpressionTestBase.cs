using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using CamlexNET.Impl;
using CamlexNET.Impl.Operands;
using CamlexNET.Impl.ReverseEngeneering;
using CamlexNET.Impl.ReverseEngeneering.Caml.Analyzers;
using CamlexNET.Interfaces.ReverseEngeneering;
using NUnit.Framework;
using Rhino.Mocks;

namespace CamlexNET.UnitTests.ReverseEngeneering.Analyzers
{
    internal class ReBinaryExpressionTestBase<TAnalyzer, TOperation>
        where TAnalyzer : ReBinaryExpressionBaseAnalyzer
        where TOperation : BinaryOperationBase
    {
        #region Helpers
        class SupportedValueType
        {
            public Type SupportedType { get; set; }
            public string ExampleValue { get; set; }
        }

        List<SupportedValueType> supportedTypesWithExamples = new List<SupportedValueType>
        {
            new SupportedValueType { SupportedType = typeof(DataTypes.Boolean), ExampleValue = bool.TrueString },
            new SupportedValueType { SupportedType = typeof(DataTypes.Boolean), ExampleValue = 1.ToString() },
            new SupportedValueType { SupportedType = typeof(DataTypes.DateTime), ExampleValue = DateTime.Now.ToString() },
            new SupportedValueType { SupportedType = typeof(DataTypes.DateTime), ExampleValue = Camlex.Now },
            new SupportedValueType { SupportedType = typeof(DataTypes.Guid), ExampleValue = Guid.NewGuid().ToString() },
            new SupportedValueType { SupportedType = typeof(DataTypes.Integer), ExampleValue = 12345.ToString() },
            new SupportedValueType { SupportedType = typeof(DataTypes.Lookup), ExampleValue = "lookup" },
            new SupportedValueType { SupportedType = typeof(DataTypes.Text), ExampleValue = "text" }
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

            var analyzer = constructor(XmlHelper.Get(xml), null);
            Assert.IsTrue(analyzer.IsValid());
        }

        internal void BASE_test_WHEN_supported_value_type_specified_THEN_expression_is_valid
            (Func<XElement, IReOperandBuilder, TAnalyzer> constructor, string operationName) 
        {
            typeof(DataTypes).GetMembers()
                .Where(x => x.MemberType == MemberTypes.NestedType).Select(x => x.Name).ToList().ForEach(x =>
                {
                    var foundItem = supportedTypesWithExamples
                        .Where(y => y.SupportedType.Name == x).FirstOrDefault();
                    if (foundItem == null) return;

                    var xml = string.Format(
                        "<{0}>" +
                        "    <FieldRef Name=\"Title\" />" +
                        "    <Value Type=\"{1}\">{2}</Value>" +
                        "</{0}>",
                        operationName, foundItem.SupportedType.Name, foundItem.ExampleValue);

                    var analyzer = constructor(XmlHelper.Get(xml), null);
                    Assert.IsTrue(analyzer.IsValid());
                });
        }

        internal void BASE_test_WHEN_not_supported_value_type_specified_THEN_expression_is_not_valid
            (Func<XElement, IReOperandBuilder, TAnalyzer> constructor, string operationName) 
        {
            typeof(DataTypes).GetMembers()
                .Where(x => x.MemberType == MemberTypes.NestedType).Select(x => x.Name).ToList().ForEach(x =>
                {
                    var foundItem = supportedTypesWithExamples
                        .Where(y => y.SupportedType.Name == x).FirstOrDefault();
                    if (foundItem != null) return;

                    var xml = string.Format(
                        "<{0}>" +
                        "    <FieldRef Name=\"Title\" />" +
                        "    <Value Type=\"{1}\">testValue</Value>" +
                        "</{0}>",
                    operationName, x);

                    var analyzer = constructor(XmlHelper.Get(xml), null);
                    Assert.IsFalse(analyzer.IsValid());
                });
        }

        internal void BASE_test_WHEN_expression_is_not_valid_THEN_exception_is_thrown
            (Func<XElement, IReOperandBuilder, TAnalyzer> constructor) 
        {
            var analyzer = constructor(null, null);
            analyzer.GetOperation();
        }

        internal TOperation BASE_test_WHEN_expression_is_valid_THEN_operation_is_returned
            (Func<XElement, IReOperandBuilder, TAnalyzer> constructor, string operationName) 
        {
            var xml = string.Format(
                "<{0}>" +
                "    <FieldRef Name=\"Title\" />" +
                "    <Value Type=\"Text\">testValue</Value>" +
                "</{0}>",
                operationName);

            var b = MockRepository.GenerateStub<IReOperandBuilder>();
            b.Stub(c => c.CreateFieldRefOperand(null)).Return(new FieldRefOperand("Title")).IgnoreArguments();
            b.Stub(c => c.CreateValueOperand(null, null)).Return(new TextValueOperand("testValue")).IgnoreArguments();

            var analyzer = constructor(XmlHelper.Get(xml), b);
            var operation = analyzer.GetOperation();
            Assert.IsInstanceOf<TOperation>(operation);
            return (TOperation)operation;
        }
    }
}