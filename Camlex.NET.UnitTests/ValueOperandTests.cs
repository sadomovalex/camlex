using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Camlex.NET.Impl.Operands;
using NUnit.Framework;

namespace Camlex.NET.UnitTests
{
    [TestFixture]
    public class ValueOperandTests
    {
        [Test]
        public void test_THAT_integer_value_IS_rendered_to_caml_properly()
        {
            var fr = new IntegerValueOperand(DataType.Integer, 1);
            string caml = fr.ToCaml().ToString();
            Assert.That(caml, Is.EqualTo("<Value Type=\"Integer\">1</Value>"));
        }
    }
}
