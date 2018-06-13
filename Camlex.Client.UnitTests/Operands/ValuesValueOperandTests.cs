using System.Collections.Generic;
using CamlexNET.Impl.Operands;
using CamlexNET.Interfaces;
using CamlexNET.UnitTests.Helpers;
using NUnit.Framework;

namespace CamlexNET.UnitTests.Operands
{
    [TestFixture]
    public class ValuesValueOperandTests
    {
        [Test]
        [ExpectedException(typeof(CantCreateValuesValueOperandException))]
        public void test_WHEN_values_are_null_THEN_exception_is_thrown()
        {
            new ValuesValueOperand(null);
        }

        [Test]
        [ExpectedException(typeof(CantCreateValuesValueOperandException))]
        public void test_WHEN_values_are_empty_THEN_exception_is_thrown()
        {
            new ValuesValueOperand(new List<IOperand>());
        }

        [Test]
        public void test_THAT_operand_IS_transformed_to_caml_properly()
        {
            var values = new List<IOperand>();
            values.Add(new BooleanValueOperand(true));
            values.Add(new TextValueOperand("test"));
            values.Add(new IntegerValueOperand(1));
            var operand = new ValuesValueOperand(values);
            var caml = operand.ToCaml().ToString();

            string expected =
                "<Values>" +
                "  <Value Type=\"Boolean\">1</Value>" +
                "  <Value Type=\"Text\">test</Value>" +
                "  <Value Type=\"Integer\">1</Value>" +
                "</Values>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }
    }
}
