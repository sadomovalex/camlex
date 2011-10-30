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
using CamlexNET.Impl.ReverseEngeneering.Caml.Factories;
using CamlexNET.UnitTests.Helpers;
using NUnit.Framework;

namespace CamlexNET.UnitTests.ReverseEngeneering.Factories
{
    [TestFixture]
    public class ReOperandBuilderFromCamlTests
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void test_WHEN_xml_is_null_THEN_exception_is_thrown()
        {
            var b = new ReOperandBuilderFromCaml();
            b.CreateFieldRefOperand(null);
        }

        [Test]
        [ExpectedException(typeof(CamlAnalysisException))]
        public void test_WHEN_id_is_not_correct_guid_THEN_exception_is_thrown()
        {
            var xml = "<FieldRef ID=\"foo\" />";

            var b = new ReOperandBuilderFromCaml();
            b.CreateFieldRefOperand(XmlHelper.Get(xml));
        }

        [Test]
        [ExpectedException(typeof(CamlAnalysisException))]
        public void test_WHEN_id_and_name_are_not_specified_THEN_exception_is_thrown()
        {
            var xml = "<FieldRef />";

            var b = new ReOperandBuilderFromCaml();
            b.CreateFieldRefOperand(XmlHelper.Get(xml));
        }

        [Test]
        [ExpectedException(typeof(CamlAnalysisException))]
        public void test_WHEN_both_id_and_name_are_specified_THEN_exception_is_thrown()
        {
            var xml = "<FieldRef ID=\"{7392DB71-A87B-41EC-8BC5-F0B8421B14FA}\" Name=\"Title\" />";

            var b = new ReOperandBuilderFromCaml();
            b.CreateFieldRefOperand(XmlHelper.Get(xml));
        }

        [Test]
        public void test_WHEN_when_id_is_specified_THEN_field_ref_operand_is_created_successfully()
        {
            var xml = "<FieldRef ID=\"{7392DB71-A87B-41EC-8BC5-F0B8421B14FA}\" Foo=\"Bar\" />";

            var b = new ReOperandBuilderFromCaml();
            var operand = b.CreateFieldRefOperand(XmlHelper.Get(xml));
            Assert.IsInstanceOf<FieldRefOperand>(operand);
            Assert.That(operand.ToCaml().ToString(), Is.EqualTo("<FieldRef ID=\"7392db71-a87b-41ec-8bc5-f0b8421b14fa\" Foo=\"Bar\" />"));
        }

        [Test]
        public void test_WHEN_when_name_is_specified_THEN_field_ref_operand_is_created_successfully()
        {
            var xml = "<FieldRef Name=\"Title\" Foo=\"Bar\" />";

            var b = new ReOperandBuilderFromCaml();
            var operand = b.CreateFieldRefOperand(XmlHelper.Get(xml));
            Assert.IsInstanceOf<FieldRefOperand>(operand);
            Assert.That(operand.ToCaml().ToString(), Is.EqualTo("<FieldRef Name=\"Title\" Foo=\"Bar\" />"));
        }

        [Test]
        public void test_THAT_field_ref_with_ordering_asc_IS_created_successfully()
        {
            var xml = "<FieldRef Name=\"Title\" Ascending=\"True\" />";

            var b = new ReOperandBuilderFromCaml();
            var operand = b.CreateFieldRefOperandWithOrdering(XmlHelper.Get(xml), new Camlex.Asc());
            Assert.IsInstanceOf<FieldRefOperandWithOrdering>(operand);
            Assert.That(operand.ToExpression().ToString(), Is.EqualTo("(x.get_Item(\"Title\") As Asc)"));
        }

        [Test]
        public void test_THAT_field_ref_with_ordering_desc_IS_created_successfully()
        {
            var xml = "<FieldRef Name=\"Title\" Ascending=\"False\" />";

            var b = new ReOperandBuilderFromCaml();
            var operand = b.CreateFieldRefOperandWithOrdering(XmlHelper.Get(xml), new Camlex.Desc());
            Assert.IsInstanceOf<FieldRefOperandWithOrdering>(operand);
            Assert.That(operand.ToExpression().ToString(), Is.EqualTo("(x.get_Item(\"Title\") As Desc)"));
        }

        [Test]
        public void test_THAT_field_ref_with_lookup_id_IS_created_successfully()
        {
            var xml = "<FieldRef Name=\"Status\" LookupId=\"True\" />";

            var b = new ReOperandBuilderFromCaml();
            var operand = b.CreateFieldRefOperand(XmlHelper.Get(xml));
            Assert.IsInstanceOf<FieldRefOperand>(operand);
            Assert.That(operand.ToExpression().ToString(), Is.EqualTo("x.get_Item(\"Status\")"));
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void test_WHEN_xml_is_null_THEN_exception_is_thrown_for_create_value_operand()
        {
            var b = new ReOperandBuilderFromCaml();
            b.CreateValueOperand(null, false);
        }

        [Test]
        [ExpectedException(typeof(CamlAnalysisException))]
        public void test_WHEN_type_attr_is_missing_THEN_exception_is_thrown_for_create_value_operand()
        {
            var xml = "<Operation><Value>1</Value></Operation>";

            var b = new ReOperandBuilderFromCaml();
            b.CreateValueOperand(XmlHelper.Get(xml), false);
        }

        [Test]
        [ExpectedException(typeof(CamlAnalysisException))]
        public void test_WHEN_type_attr_has_incorrect_value_THEN_exception_is_thrown_for_create_value_operand()
        {
            var xml = "<Operation><Value Type=\"foo\">1</Value></Operation>";

            var b = new ReOperandBuilderFromCaml();
            b.CreateValueOperand(XmlHelper.Get(xml), false);
        }

        [Test]
        public void test_WHEN_type_attr_has_correct_value_THEN_value_operand_is_sucessfully_created()
        {
            var b = new ReOperandBuilderFromCaml();
            Assert.That(b.CreateValueOperand(XmlHelper.Get("<Operation><Value Type=\"Text\">foo</Value></Operation>"), false).ToCaml().ToString(),
                Is.EqualTo("<Value Type=\"Text\">foo</Value>"));
            Assert.That(b.CreateValueOperand(XmlHelper.Get("<Operation><Value Type=\"Integer\">123</Value></Operation>"), false).ToCaml().ToString(),
                Is.EqualTo("<Value Type=\"Integer\">123</Value>"));
            Assert.That(b.CreateValueOperand(XmlHelper.Get("<Operation><Value Type=\"Boolean\">1</Value></Operation>"), false).ToCaml().ToString(),
                Is.EqualTo("<Value Type=\"Boolean\">1</Value>"));
            Assert.That(b.CreateValueOperand(XmlHelper.Get("<Operation><Value Type=\"Boolean\">0</Value></Operation>"), false).ToCaml().ToString(),
                Is.EqualTo("<Value Type=\"Boolean\">0</Value>"));
            Assert.That(b.CreateValueOperand(XmlHelper.Get("<Operation><Value Type=\"DateTime\">2010-02-01T03:04:05Z</Value></Operation>"), false).ToCaml().ToString(),
                Is.EqualTo("<Value Type=\"DateTime\">2010-02-01T03:04:05Z</Value>"));
            Assert.That(b.CreateValueOperand(XmlHelper.Get("<Operation><Value Type=\"DateTime\"><Now /></Value></Operation>"), false).ToCaml().ToString(),
                Is.EqualTo("<Value Type=\"DateTime\"><Now /></Value>").Using(new CamlComparer()));
            Assert.That(b.CreateValueOperand(XmlHelper.Get("<Operation><Value Type=\"DateTime\"><Today /></Value></Operation>"), false).ToCaml().ToString(),
                Is.EqualTo("<Value Type=\"DateTime\"><Today /></Value>").Using(new CamlComparer()));
            Assert.That(b.CreateValueOperand(XmlHelper.Get("<Operation><Value Type=\"DateTime\"><Week /></Value></Operation>"), false).ToCaml().ToString(),
                Is.EqualTo("<Value Type=\"DateTime\"><Week /></Value>").Using(new CamlComparer()));
            Assert.That(b.CreateValueOperand(XmlHelper.Get("<Operation><Value Type=\"DateTime\"><Month /></Value></Operation>"), false).ToCaml().ToString(),
                Is.EqualTo("<Value Type=\"DateTime\"><Month /></Value>").Using(new CamlComparer()));
            Assert.That(b.CreateValueOperand(XmlHelper.Get("<Operation><Value Type=\"DateTime\"><Year /></Value></Operation>"), false).ToCaml().ToString(),
                Is.EqualTo("<Value Type=\"DateTime\"><Year /></Value>").Using(new CamlComparer()));
            Assert.That(b.CreateValueOperand(XmlHelper.Get("<Operation><Value Type=\"Guid\">{AD524A0C-D90E-4C04-B6FB-CB6E9F6CA7BD}</Value></Operation>"), false).ToCaml().ToString(),
                Is.EqualTo("<Value Type=\"Guid\">ad524a0c-d90e-4c04-b6fb-cb6e9f6ca7bd</Value>"));

            var valueOperand = b.CreateValueOperand(XmlHelper.Get("<Operation><Value Type=\"Lookup\">1</Value></Operation>"), false);
            Assert.IsInstanceOf<LookupValueValueOperand>(valueOperand);
            Assert.That(valueOperand.ToCaml().ToString(),
                Is.EqualTo("<Value Type=\"Lookup\">1</Value>"));

            valueOperand = b.CreateValueOperand(XmlHelper.Get(
                "<Operation><Value Type=\"Lookup\">1</Value><FieldRef Name=\"Status\" LookupId=\"True\" /></Operation>"), false);
            Assert.IsInstanceOf<LookupIdValueOperand>(valueOperand);
            Assert.That(valueOperand.ToCaml().ToString(),
                Is.EqualTo("<Value Type=\"Lookup\">1</Value>"));

            Assert.That(b.CreateValueOperand(XmlHelper.Get("<Operation><Value Type=\"Note\">foo</Value></Operation>"), false).ToCaml().ToString(),
                Is.EqualTo("<Value Type=\"Note\">foo</Value>"));
        }

        [Test]
        [ExpectedException(typeof(NonSupportedOperandTypeException))]
        public void test_WHEN_comparision_operation_but_type_is_not_compirable_THEN_exception_is_thrown_for_create_value_operand()
        {
            var xml = "<Operation><Value Type=\"Boolean\">True</Value></Operation>";

            var b = new ReOperandBuilderFromCaml();
            b.CreateValueOperand(XmlHelper.Get(xml), true);
        }

        [Test]
        public void test_THAT_compirable_native_operands_ARE_created_sucessfully()
        {
            var b = new ReOperandBuilderFromCaml();
            // no exceptions should occur
            b.CreateValueOperand(XmlHelper.Get("<Operation><Value Type=\"Text\">foo</Value></Operation>"), true);
            b.CreateValueOperand(XmlHelper.Get("<Operation><Value Type=\"Integer\">1</Value></Operation>"), true);
            b.CreateValueOperand(XmlHelper.Get("<Operation><Value Type=\"DateTime\">" + DateTime.Now.ToString("s") + "</Value></Operation>"), true);
        }
    }
}
