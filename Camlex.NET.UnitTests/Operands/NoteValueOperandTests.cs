using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Camlex.NET.Impl.Operands;
using NUnit.Framework;

namespace Camlex.NET.UnitTests.Operands
{
    [TestFixture]
    public class NoteValueOperandTests
    {
        [Test]
        public void test_THAT_note_value_IS_rendered_to_caml_properly()
        {
            var operand = new NoteValueOperand("foo");
            var caml = operand.ToCaml().ToString();
            Assert.That(caml, Is.EqualTo("<Value Type=\"Note\">foo</Value>"));
        }
    }
}
