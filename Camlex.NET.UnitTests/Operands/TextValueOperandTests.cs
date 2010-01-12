using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CamlexNET.Impl.Operands;
using NUnit.Framework;

namespace CamlexNET.UnitTests.Operands
{
    [TestFixture]
    public class TextValueOperandTests
    {
        [Test]
        public void test_THAT_text_value_IS_rendered_to_caml_properly()
        {
            var operand = new TextValueOperand("foo");
            string caml = operand.ToCaml().ToString();
            Assert.That(caml, Is.EqualTo("<Value Type=\"Text\">foo</Value>"));
        }
    }
}
