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
    public class FieldRefOperandTests
    {
        [Test]
        public void test_THAT_field_ref_operand_with_guid_IS_converted_to_expression_correctly()
        {
            var id = new Guid("{7742D3F8-A2B7-430F-BDEB-B2DDBF853901}");
            var op = new FieldRefOperand(id);
            var expr = op.ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("x.get_Item(7742D3F8-A2B7-430F-BDEB-B2DDBF853901)").Using(new CaseInsensetiveComparer()));
        }

        [Test]
        public void test_THAT_field_ref_operand_with_filed_name_IS_converted_to_expression_correctly()
        {
            var op = new FieldRefOperand("Title");
            var expr = op.ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("x.get_Item(\"Title\")").Using(new CaseInsensetiveComparer()));
        }
    }
}
