using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using CamlexNET.Impl.Factories;
using CamlexNET.Impl.Operands;
using CamlexNET.Impl.Operations.Join;
using CamlexNET.Interfaces;
using CamlexNET.UnitTests.Helpers;
using NUnit.Framework;
using Rhino.Mocks;

namespace CamlexNET.UnitTests.Operations.Join
{
    [TestFixture]
    public class JoinOperationTests
    {
        [Test]
        public void test_THAT_join_operation_IS_rendered_to_caml_properly()
        {
            var f = MockRepository.GenerateStub<FieldRefOperand>("");
            var v = MockRepository.GenerateStub<IntegerValueOperand>(1);

            f.Stub(o => o.ToCaml()).Return(new XElement("fieldRefOperandStub"));
            v.Stub(o => o.ToCaml()).Return(new XElement("valueOperandStub"));
            var op = new JoinOperation(new OperationResultBuilder(), f, v, JoinType.Inner);

            string caml = op.ToResult().ToString();

            string expected =
                "<Join Type=\"INNER\" ListAlias=\"\">" +
                "  <Eq>" +
                "    <fieldRefOperandStub />" +
                "    <valueOperandStub />" +
                "  </Eq>" +
                "</Join>";
            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }
    }
}
