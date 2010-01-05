using System.Xml.Linq;
using Camlex.NET.Impl.Factories;
using Camlex.NET.Impl.Operands;
using Camlex.NET.Impl.Operations.Geq;
using Camlex.NET.UnitTests.Helpers;
using NUnit.Framework;
using Rhino.Mocks;

namespace Camlex.NET.UnitTests.Operations.Geq
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

            var resultBuilder = new OperationResultBuilder();
            var operation = new GeqOperation(resultBuilder, fieldRefOperandStub, valueOperandStub);

            // act
            string caml = operation.ToResult().ToString();

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


