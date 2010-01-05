using System.Xml.Linq;
using Camlex.NET.Impl.Operands;
using Camlex.NET.Impl.Operations.Leq;
using Camlex.NET.UnitTests.Helpers;
using NUnit.Framework;
using Rhino.Mocks;

namespace Camlex.NET.UnitTests.Operations.Leq
{
    [TestFixture]
    public class LeqOperationTests
    {
        [Test]
        public void test_THAT_leq_operation_IS_rendered_to_caml_properly()
        {
            // arrange
            var fieldRefOperandStub = MockRepository.GenerateStub<FieldRefOperand>("");
            var valueOperandStub = MockRepository.GenerateStub<IntegerValueOperand>(0);

            fieldRefOperandStub.Stub(o => o.ToCaml()).Return(new XElement("fieldRefOperandStub"));
            valueOperandStub.Stub(o => o.ToCaml()).Return(new XElement("valueOperandStub"));

            var operation = new LeqOperation(null, fieldRefOperandStub, valueOperandStub);

            // act
            string caml = operation.ToResult().ToString();

            // assert
            string expected =
                @"<Leq>
                    <fieldRefOperandStub />
                    <valueOperandStub />
                </Leq>";
            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }
    }
}


