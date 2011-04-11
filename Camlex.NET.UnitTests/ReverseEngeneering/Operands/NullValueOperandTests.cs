using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CamlexNET.Impl.Operands;
using CamlexNET.UnitTests.Helpers;
using NUnit.Framework;

namespace CamlexNET.UnitTests.ReverseEngeneering.Operands
{
    [TestFixture]
    public class NullValueOperandTests
    {
        [Test]
        public void test_THAT_operand_with_123_IS_conveted_to_expression_correctly()
        {
            var op = new NullValueOperand();
            var expr = op.ToExpression();
            Assert.That("null", Is.EqualTo(expr.ToString()));
        }
    }
}
