using System.Xml.Linq;
using Camlex.NET.Impl.Factories;
using Camlex.NET.Impl.Operands;
using Camlex.NET.Impl.Operations.Leq;
using Camlex.NET.Impl.Operations.Lt;
using Camlex.NET.UnitTests.Helpers;
using NUnit.Framework;
using Rhino.Mocks;

namespace Camlex.NET.UnitTests.Operations.Lt
{
    [TestFixture]
    public class LtOperationTests
    {
        [Test]
        public void test_THAT_lt_operation_IS_rendered_to_caml_properly()
        {
            // arrange
            var fieldRefOperandStub = MockRepository.GenerateStub<FieldRefOperand>("");
            var valueOperandStub = MockRepository.GenerateStub<IntegerValueOperand>(0);

            fieldRefOperandStub.Stub(o => o.ToCaml()).Return(new XElement("fieldRefOperandStub"));
            valueOperandStub.Stub(o => o.ToCaml()).Return(new XElement("valueOperandStub"));

            var resultBuilder = new OperationResultBuilder();
            var operation = new LtOperation(resultBuilder, fieldRefOperandStub, valueOperandStub);

            // act
            string caml = operation.ToResult().ToString();

            // assert
            string expected =
                @"<Lt>
                    <fieldRefOperandStub />
                    <valueOperandStub />
                </Lt>";
            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }
    }
}


