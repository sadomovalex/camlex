using System.Xml.Linq;
using Camlex.NET.Impl.Operands;
using Camlex.NET.Impl.Operations.Eq;
using Camlex.NET.UnitTests.Helpers;
using NUnit.Framework;
using Rhino.Mocks;

namespace Camlex.NET.UnitTests.Operations.Eq
{
    [TestFixture]
    public class EqOperationTests
    {
        [Test]
        public void test_THAT_operation_IS_rendered_to_caml_properly()
        {
            // arrange
            var fieldRefOperandStub = MockRepository.GenerateStub<FieldRefOperand>("");
            var valueOperandStub = MockRepository.GenerateStub<IntegerValueOperand>(0);

            fieldRefOperandStub.Stub(o => o.ToCaml()).Return(new XElement("fieldRefOperandStub"));
            valueOperandStub.Stub(o => o.ToCaml()).Return(new XElement("valueOperandStub"));

            var operation =  new EqOperation(fieldRefOperandStub, valueOperandStub);

            // act
            string caml = operation.ToCaml().ToString();

            // assert
            string expected =
                @"<Eq>
                    <fieldRefOperandStub />
                    <valueOperandStub />
                </Eq>";
            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }
    }
}


