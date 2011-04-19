using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CamlexNET.Impl.Operands;
using NUnit.Framework;

namespace CamlexNET.UnitTests.ReverseEngeneering.Operands
{
    [TestFixture]
    public class GenericStringBasedValueOperandTests
    {
        [Test]
        public void test_THAT_operand_with_value_IS_conveted_to_expression_correctly()
        {
            var op = new GenericStringBasedValueOperand(typeof(DataTypes.Text), "foo");
            var expr = op.ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("Convert(Convert(\"foo\"))"));
        }
    }
}
