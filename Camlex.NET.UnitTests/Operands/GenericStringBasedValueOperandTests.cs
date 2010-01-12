using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CamlexNET.Impl.Operands;
using NUnit.Framework;

namespace CamlexNET.UnitTests.Operands
{
    [TestFixture]
    public class GenericStringBasedValueOperandTests
    {
        [Test]
        public void test_THAT_generic_string_based_value_IS_rendered_to_caml_properly()
        {
            var operand = new GenericStringBasedValueOperand(typeof(DataTypes.User), "John Smith");
            string caml = operand.ToCaml().ToString();
            Assert.That(caml, Is.EqualTo("<Value Type=\"User\">John Smith</Value>"));
        }
    }
}
