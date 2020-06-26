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

using CamlexNET.Impl.Operands;
using CamlexNET.Impl.Operations.Membership;
using CamlexNET.Impl.ReverseEngeneering;
using CamlexNET.Impl.ReverseEngeneering.Caml.Analyzers;
using CamlexNET.Impl.ReverseEngeneering.Caml.Factories;
using CamlexNET.Interfaces.ReverseEngeneering;
using NUnit.Framework;
using Rhino.Mocks;

namespace CamlexNET.UnitTests.ReverseEngeneering.Analyzers
{
    internal class ReMembershipAnalyzerTest
    {
        [Test]
        public void test_WHEN_xml_is_null_THEN_expression_is_not_valid()
        {
            var analyzer = new ReMembershipAnalyzer(null, null);
            Assert.IsFalse(analyzer.IsValid());
        }

        [Test]
        public void test_WHEN_no_membership_type_specified_THEN_expression_is_not_valid()
        {
            var xml = string.Format(
                "<Membership>" +
                "</Membership>");

            var analyzer = new ReMembershipAnalyzer(XmlHelper.Get(xml), null);
            Assert.IsFalse(analyzer.IsValid());
        }

        [Test]
        public void test_WHEN_no_field_ref_specified_THEN_expression_is_not_valid()
        {
            var xml = string.Format(
                "<Membership Type=\"CurrentUserGroups\">" +
                "</Membership>");

            var analyzer = new ReMembershipAnalyzer(XmlHelper.Get(xml), null);
            Assert.IsFalse(analyzer.IsValid());
        }

        [Test]
        public void test_WHEN_field_ref_and_membership_type_specified_THEN_expression_is_valid()
        {
            var xml = string.Format(
                "<Membership Type=\"CurrentUserGroups\">" +
                "    <FieldRef Name=\"Field\" />" +
                "</Membership>");

            var analyzer = new ReMembershipAnalyzer(XmlHelper.Get(xml), new ReOperandBuilderFromCaml());
            Assert.IsTrue(analyzer.IsValid());
        }

        [Test]
        public void test_WHEN_expression_is_spweballusers_THEN_operation_is_returned()
        {
            var xml = string.Format(
                "<Membership Type=\"SPWeb.AllUsers\">" +
                "    <FieldRef Name=\"Field\" />" +
                "</Membership>");

            var b = MockRepository.GenerateStub<IReOperandBuilder>();
            b.Stub(c => c.CreateFieldRefOperand(null)).Return(new FieldRefOperand("Field")).IgnoreArguments();

            var analyzer = new ReMembershipAnalyzer(XmlHelper.Get(xml), b);
            var operation = analyzer.GetOperation();
            Assert.IsInstanceOf<MembershipOpeartion>(operation);
            var operationT = (MembershipOpeartion)operation;
            Assert.That(operationT.ToExpression().ToString(), Is.EqualTo(
                string.Format("Membership(x.get_Item(\"Field\"), new {0}())", typeof(Camlex.SPWebAllUsers).Name)));
        }

        [Test]
        public void test_WHEN_expression_is_spgroup_THEN_operation_is_returned()
        {
            var xml = string.Format(
                "<Membership Type=\"SPGroup\" ID=\"7\">" +
                "    <FieldRef Name=\"Field\" />" +
                "</Membership>");

            var b = MockRepository.GenerateStub<IReOperandBuilder>();
            b.Stub(c => c.CreateFieldRefOperand(null)).Return(new FieldRefOperand("Field")).IgnoreArguments();

            var analyzer = new ReMembershipAnalyzer(XmlHelper.Get(xml), b);
            var operation = analyzer.GetOperation();
            Assert.IsInstanceOf<MembershipOpeartion>(operation);
            var operationT = (MembershipOpeartion)operation;
            Assert.That(operationT.ToExpression().ToString(), Is.EqualTo(
                string.Format("Membership(x.get_Item(\"Field\"), new {0}({1}))", typeof(Camlex.SPGroup).Name, 7)));
        }

        [Test]
        public void test_WHEN_expression_is_spgroup_and_id_not_specified_THEN_expression_is_not_valid()
        {
            var xml = string.Format(
                "<Membership Type=\"SPGroup\">" +
                "    <FieldRef Name=\"Field\" />" +
                "</Membership>");

            var analyzer = new ReMembershipAnalyzer(XmlHelper.Get(xml), null);
            Assert.IsFalse(analyzer.IsValid());
        }

        [Test]
        public void test_WHEN_expression_is_spwebgroups_THEN_operation_is_returned()
        {
            var xml = string.Format(
                "<Membership Type=\"SPWeb.Groups\">" +
                "    <FieldRef Name=\"Field\" />" +
                "</Membership>");

            var b = MockRepository.GenerateStub<IReOperandBuilder>();
            b.Stub(c => c.CreateFieldRefOperand(null)).Return(new FieldRefOperand("Field")).IgnoreArguments();

            var analyzer = new ReMembershipAnalyzer(XmlHelper.Get(xml), b);
            var operation = analyzer.GetOperation();
            Assert.IsInstanceOf<MembershipOpeartion>(operation);
            var operationT = (MembershipOpeartion)operation;
            Assert.That(operationT.ToExpression().ToString(), Is.EqualTo(
                string.Format("Membership(x.get_Item(\"Field\"), new {0}())", typeof(Camlex.SPWebGroups).Name)));
        }

        [Test]
        public void test_WHEN_expression_is_currentusergroups_THEN_operation_is_returned()
        {
            var xml = string.Format(
                "<Membership Type=\"CurrentUserGroups\">" +
                "    <FieldRef Name=\"Field\" />" +
                "</Membership>");

            var b = MockRepository.GenerateStub<IReOperandBuilder>();
            b.Stub(c => c.CreateFieldRefOperand(null)).Return(new FieldRefOperand("Field")).IgnoreArguments();

            var analyzer = new ReMembershipAnalyzer(XmlHelper.Get(xml), b);
            var operation = analyzer.GetOperation();
            Assert.IsInstanceOf<MembershipOpeartion>(operation);
            var operationT = (MembershipOpeartion)operation;
            Assert.That(operationT.ToExpression().ToString(), Is.EqualTo(
                string.Format("Membership(x.get_Item(\"Field\"), new {0}())", typeof(Camlex.CurrentUserGroups).Name)));
        }

        [Test]
        public void test_WHEN_expression_is_spwebusers_THEN_operation_is_returned()
        {
            var xml = string.Format(
                "<Membership Type=\"SPWeb.Users\">" +
                "    <FieldRef Name=\"Field\" />" +
                "</Membership>");

            var b = MockRepository.GenerateStub<IReOperandBuilder>();
            b.Stub(c => c.CreateFieldRefOperand(null)).Return(new FieldRefOperand("Field")).IgnoreArguments();

            var analyzer = new ReMembershipAnalyzer(XmlHelper.Get(xml), b);
            var operation = analyzer.GetOperation();
            Assert.IsInstanceOf<MembershipOpeartion>(operation);
            var operationT = (MembershipOpeartion)operation;
            Assert.That(operationT.ToExpression().ToString(), Is.EqualTo(
                string.Format("Membership(x.get_Item(\"Field\"), new {0}())", typeof(Camlex.SPWebUsers).Name)));
        }
    }
}
