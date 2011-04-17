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
    public class GuidValueOperandTests
    {
        [Test]
        public void test_THAT_operand_with_guid_IS_conveted_to_expression_correctly()
        {
            var guid = new Guid("{4feaf1f3-5b04-4d93-b0fc-4e48d0c60eed}");
            var op = new GuidValueOperand(guid);
            var expr = op.ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("4feaf1f3-5b04-4d93-b0fc-4e48d0c60eed").Using(new CaseInsensetiveComparer()));
        }
    }
}
