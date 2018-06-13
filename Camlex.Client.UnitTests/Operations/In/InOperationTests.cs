using System.Xml.Linq;
using CamlexNET.Impl.Factories;
using CamlexNET.Impl.Operands;
using CamlexNET.Impl.Operations.In;
using CamlexNET.UnitTests.Helpers;
using NUnit.Framework;
using Rhino.Mocks;

namespace CamlexNET.UnitTests.Operations.In
{
    [TestFixture]
    public class InOperationTests
    {
        [Test]
        public void test_THAT_in_operation_IS_rendered_to_caml_properly()
        {
            var f = MockRepository.GenerateStub<FieldRefOperand>("");
            var v = MockRepository.GenerateStub<IntegerValueOperand>(1);

            f.Stub(o => o.ToCaml()).Return(new XElement("fieldRefOperandStub"));
            v.Stub(o => o.ToCaml()).Return(new XElement("valueOperandStub"));
            var op = new InOperation(new OperationResultBuilder(), f, v);

            string caml = op.ToResult().ToString();

            string expected =
                @"<In>
                    <fieldRefOperandStub />
                    <valueOperandStub />
                </In>";
            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }
    }
}
