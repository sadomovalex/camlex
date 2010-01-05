using System.Xml.Linq;
using Camlex.NET.Impl.Eq;
using Camlex.NET.Impl.Geq;
using Camlex.NET.Impl.Operands;
using Camlex.NET.UnitTests.Helpers;
using NUnit.Framework;
using Rhino.Mocks;

namespace Camlex.NET.UnitTests.Geq
{
    [TestFixture]
    public class GeqOperationTests
    {
        [Test]
        public void test_THAT_geq_operation_IS_rendered_to_caml_properly()
        {
            // arrange
            var fieldRefOperandStub = MockRepository.GenerateStub<FieldRefOperand>("");
            var valueOperandStub = MockRepository.GenerateStub<IntegerValueOperand>(0);

            fieldRefOperandStub.Stub(o => o.ToCaml()).Return(new XElement("fieldRefOperandStub"));
            valueOperandStub.Stub(o => o.ToCaml()).Return(new XElement("valueOperandStub"));

            var operation =  new GeqOperation(fieldRefOperandStub, valueOperandStub);

            // act
            string caml = operation.ToCaml().ToString();

            // assert
            string expected =
                @"<Geq>
                    <fieldRefOperandStub />
                    <valueOperandStub />
                </Geq>";
            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }
    }
}


