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
    public class BooleanValueOperandTests
    {
        [Test]
        public void test_THAT_operand_with_true_IS_conveted_to_expression_correctly()
        {
            var op = new BooleanValueOperand(true);
            var expr = op.ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("true").Using(new CaseInsensetiveComparer()));
        }

        [Test]
        public void test_THAT_operand_with_false_IS_conveted_to_expression_correctly()
        {
            var op = new BooleanValueOperand(false);
            var expr = op.ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("false").Using(new CaseInsensetiveComparer()));
        }
    }
}
