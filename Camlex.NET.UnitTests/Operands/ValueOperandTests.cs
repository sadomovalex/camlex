﻿using Camlex.NET.Impl.Operands;
using NUnit.Framework;

namespace Camlex.NET.UnitTests.Operands
{
    [TestFixture]
    public class ValueOperandTests
    {
        [Test]
        public void test_THAT_integer_value_IS_rendered_to_caml_properly()
        {
            var fr = new IntegerValueOperand(1);
            string caml = fr.ToCaml().ToString();
            Assert.That(caml, Is.EqualTo("<Value Type=\"Integer\">1</Value>"));
        }
    }
}


