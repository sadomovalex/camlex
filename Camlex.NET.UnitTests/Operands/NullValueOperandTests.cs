using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CamlexNET.Impl.Operands;
using NUnit.Framework;

namespace CamlexNET.UnitTests.Operands
{
    [TestFixture]
    public class NullValueOperandTests
    {
        [Test]
        [ExpectedException(typeof(NullValueOperandCannotBeTranslatedToCamlException))]
        public void test_WHEN_nullvalue_operand_is_rendered_to_caml_THEN_exception_is_thrown()
        {
            var operand = new NullValueOperand();
            operand.ToCaml();
        }
    }
}
