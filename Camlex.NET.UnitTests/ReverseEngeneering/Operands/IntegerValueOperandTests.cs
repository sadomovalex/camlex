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
    public class IntegerValueOperandTests
    {
        [Test]
        public void test_THAT_operand_with_123_IS_conveted_to_expression_correctly()
        {
            var op = new IntegerValueOperand(123);
            var expr = op.ToExpression();
            Assert.That("123", Is.EqualTo(expr.ToString()).Using(new CaseInsensetiveComparer()));
        }
    }
}
