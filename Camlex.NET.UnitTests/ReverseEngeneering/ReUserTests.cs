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

using CamlexNET.Impl.Operations.Eq;
using CamlexNET.Impl.ReverseEngeneering.Caml.Analyzers;
using CamlexNET.Impl.ReverseEngeneering.Caml.Factories;
using NUnit.Framework;

namespace CamlexNET.UnitTests.ReverseEngeneering
{
    [TestFixture]
    public class ReUserTests
    {
        [Test]
        public void test_WHEN_type_is_user_value_and_value_is_specified_THEN_expression_is_valid()
        {
            var xml =
                "<Eq>" +
                "    <FieldRef Name=\"foo\" />" +
                "    <Value Type=\"User\">Test User</Value>" +
                "</Eq>";

            var operandBuilder = new ReOperandBuilderFromCaml();
            var analyzer = new ReEqAnalyzer(XmlHelper.Get(xml), operandBuilder);
            var operation = (EqOperation)analyzer.GetOperation();
            Assert.That(operation.ToExpression().ToString(), Is.EqualTo(
                "(x.get_Item(\"foo\") == Convert(Convert(\"Test User\")))"));
        }

        [Test]
        public void test_WHEN_type_is_user_id_and_value_is_specified_THEN_expression_is_valid()
        {
            var xml =
                "<Eq>" +
                "    <FieldRef Name=\"foo\" LookupId=\"True\" />" +
                "    <Value Type=\"User\">123</Value>" +
                "</Eq>";

            var operandBuilder = new ReOperandBuilderFromCaml();
            var analyzer = new ReEqAnalyzer(XmlHelper.Get(xml), operandBuilder);
            var operation = (EqOperation)analyzer.GetOperation();
            Assert.That(operation.ToExpression().ToString(), Is.EqualTo(
                "(x.get_Item(\"foo\") == Convert(Convert(\"123\")))"));
        }

        [Test]
        public void test_WHEN_user_id_tag_is_specified_THEN_expression_is_valid()
        {
            var xml =
                "<Eq>" +
                "    <FieldRef Name=\"foo\"/>" +
                "    <Value Type=\"Integer\"><UserID /></Value>" +
                "</Eq>";

            var operandBuilder = new ReOperandBuilderFromCaml();
            var analyzer = new ReEqAnalyzer(XmlHelper.Get(xml), operandBuilder);
            var operation = (EqOperation)analyzer.GetOperation();
            Assert.That(operation.ToExpression().ToString(), Is.EqualTo(
                "(x.get_Item(\"foo\") == Convert(Convert(Camlex.UserID)))"));
        }
    }
}