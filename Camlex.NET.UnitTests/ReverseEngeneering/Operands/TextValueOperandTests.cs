using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CamlexNET.Impl.Operands;
using NUnit.Framework;

namespace CamlexNET.UnitTests.ReverseEngeneering.Operands
{
    [TestFixture]
    public class TextValueOperandTests
    {
        [Test]
        public void test_THAT_operand_with_not_empty_string_IS_conveted_to_expression_correctly()
        {
            var op = new TextValueOperand("foo");
            var expr = op.ToExpression();
            Assert.That("\"foo\"", Is.EqualTo(expr.ToString()));
        }
    }
}
