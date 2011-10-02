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
            var xml =
                "    <FieldRef ID=\"foo\" />";

            var b = new ReOperandBuilderFromCaml();
            b.CreateFieldRefOperand(XmlHelper.Get(xml));
        }

        [Test]
        [ExpectedException(typeof(CamlAnalysisException))]
        public void test_WHEN_id_and_name_are_not_specified_THEN_exception_is_thrown()
        {
            var xml =
                "    <FieldRef />";

            var b = new ReOperandBuilderFromCaml();
            b.CreateFieldRefOperand(XmlHelper.Get(xml));
        }

        [Test]
        [ExpectedException(typeof(CamlAnalysisException))]
        public void test_WHEN_both_id_and_name_are_specified_THEN_exception_is_thrown()
        {
            var xml =
                "    <FieldRef ID=\"{7392DB71-A87B-41EC-8BC5-F0B8421B14FA}\" Name=\"Title\" />";

            var b = new ReOperandBuilderFromCaml();
            b.CreateFieldRefOperand(XmlHelper.Get(xml));
        }

        [Test]
        public void test_WHEN_when_id_is_specified_THEN_field_ref_operand_is_created_successfully()
        {
            var xml =
                "    <FieldRef ID=\"{7392DB71-A87B-41EC-8BC5-F0B8421B14FA}\" Foo=\"Bar\" />";

            var b = new ReOperandBuilderFromCaml();
            var operand = b.CreateFieldRefOperand(XmlHelper.Get(xml));
            Assert.IsInstanceOf<FieldRefOperand>(operand);
            Assert.That(operand.ToCaml().ToString(), Is.EqualTo("<FieldRef ID=\"7392db71-a87b-41ec-8bc5-f0b8421b14fa\" Foo=\"Bar\" />"));
        }

        [Test]
        public void test_WHEN_when_name_is_specified_THEN_field_ref_operand_is_created_successfully()
        {
            var xml =
                "    <FieldRef Name=\"Title\" Foo=\"Bar\" />";

            var b = new ReOperandBuilderFromCaml();
            var operand = b.CreateFieldRefOperand(XmlHelper.Get(xml));
            Assert.IsInstanceOf<FieldRefOperand>(operand);
            Assert.That(operand.ToCaml().ToString(), Is.EqualTo("<FieldRef Name=\"Title\" Foo=\"Bar\" />"));
        }

        [Test]
        public void test_THAT_field_ref_with_ordering_asc_IS_created_successfully()
        {
            var xml =
                "    <FieldRef Name=\"Title\" Ascending=\"True\" />";

            var b = new ReOperandBuilderFromCaml();
            var operand = b.CreateFieldRefOperandWithOrdering(XmlHelper.Get(xml), new Camlex.Asc());
            Assert.IsInstanceOf<FieldRefOperandWithOrdering>(operand);
            Assert.That(operand.ToExpression().ToString(), Is.EqualTo("(x.get_Item(\"Title\") As Asc)"));
        }

        [Test]
        public void test_THAT_field_ref_with_ordering_desc_IS_created_successfully()
        {
            var xml =
                "    <FieldRef Name=\"Title\" Ascending=\"False\" />";

            var b = new ReOperandBuilderFromCaml();
            var operand = b.CreateFieldRefOperandWithOrdering(XmlHelper.Get(xml), new Camlex.Desc());
            Assert.IsInstanceOf<FieldRefOperandWithOrdering>(operand);
            Assert.That(operand.ToExpression().ToString(), Is.EqualTo("(x.get_Item(\"Title\") As Desc)"));
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void test_WHEN_xml_is_null_THEN_exception_is_thrown_for_create_value_operand()
        {
            var b = new ReOperandBuilderFromCaml();
            b.CreateValueOperand(null);
        }

        [Test]
        [ExpectedException(typeof(CamlAnalysisException))]
        public void test_WHEN_type_attr_is_missing_THEN_exception_is_thrown_for_create_value_operand()
        {
            var xml = "<Value>1</Value>";

            var b = new ReOperandBuilderFromCaml();
            b.CreateValueOperand(XmlHelper.Get(xml));
        }

        [Test]
        [ExpectedException(typeof(CamlAnalysisException))]
        public void test_WHEN_type_attr_has_incorrect_value_THEN_exception_is_thrown_for_create_value_operand()
        {
            var xml = "<Value Type=\"foo\">1</Value>";

            var b = new ReOperandBuilderFromCaml();
            b.CreateValueOperand(XmlHelper.Get(xml));
        }

        [Test]
        public void test_WHEN_type_attr_has_correct_value_THEN_value_operand_is_sucessfully_created()
        {
            var b = new ReOperandBuilderFromCaml();
            Assert.That(b.CreateValueOperand(XmlHelper.Get("<Value Type=\"Boolean\">1</Value>")).ToCaml().ToString(),
                Is.EqualTo("<Value Type=\"Boolean\">1</Value>"));
            Assert.That(b.CreateValueOperand(XmlHelper.Get("<Value Type=\"Boolean\">0</Value>")).ToCaml().ToString(),
                Is.EqualTo("<Value Type=\"Boolean\">0</Value>"));
            Assert.That(b.CreateValueOperand(XmlHelper.Get("<Value Type=\"DateTime\">2010-02-01T03:04:05Z</Value>")).ToCaml().ToString(),
                Is.EqualTo("<Value Type=\"DateTime\">2010-02-01T03:04:05Z</Value>"));
            Assert.That(b.CreateValueOperand(XmlHelper.Get("<Value Type=\"DateTime\"><Now /></Value>")).ToCaml().ToString(),
                Is.EqualTo("<Value Type=\"DateTime\"><Now /></Value>").Using(new CamlComparer()));
            Assert.That(b.CreateValueOperand(XmlHelper.Get("<Value Type=\"DateTime\"><Today /></Value>")).ToCaml().ToString(),
                Is.EqualTo("<Value Type=\"DateTime\"><Today /></Value>").Using(new CamlComparer()));
            Assert.That(b.CreateValueOperand(XmlHelper.Get("<Value Type=\"DateTime\"><Week /></Value>")).ToCaml().ToString(),
                Is.EqualTo("<Value Type=\"DateTime\"><Week /></Value>").Using(new CamlComparer()));
            Assert.That(b.CreateValueOperand(XmlHelper.Get("<Value Type=\"DateTime\"><Month /></Value>")).ToCaml().ToString(),
                Is.EqualTo("<Value Type=\"DateTime\"><Month /></Value>").Using(new CamlComparer()));
            Assert.That(b.CreateValueOperand(XmlHelper.Get("<Value Type=\"DateTime\"><Year /></Value>")).ToCaml().ToString(),
                Is.EqualTo("<Value Type=\"DateTime\"><Year /></Value>").Using(new CamlComparer()));
            Assert.That(b.CreateValueOperand(XmlHelper.Get("<Value Type=\"Guid\">{AD524A0C-D90E-4C04-B6FB-CB6E9F6CA7BD}</Value>")).ToCaml().ToString(),
                Is.EqualTo("<Value Type=\"Guid\">ad524a0c-d90e-4c04-b6fb-cb6e9f6ca7bd</Value>"));
            Assert.That(b.CreateValueOperand(XmlHelper.Get("<Value Type=\"Lookup\">1</Value>")).ToCaml().ToString(),
                Is.EqualTo("<Value Type=\"Lookup\">1</Value>"));

        }
    }
}
