using System.Xml.Linq;
using CamlexNET.Impl.Factories;
using CamlexNET.Impl.Operands;
using CamlexNET.Impl.Operations.BeginsWith;
using CamlexNET.UnitTests.Helpers;
using NUnit.Framework;
using Rhino.Mocks;

namespace CamlexNET.UnitTests.Operations.BeginsWith
{
    [TestFixture]
    public class BeginsWithOperationTests
    {
        [Test]
        public void test_THAT_beginswith_operation_IS_renderedtocaml_properly()
        {
            // arrange
            var fieldRefOperandStub = MockRepository.GenerateStub<FieldRefOperand>("");
            var valueOperandStub = MockRepository.GenerateStub<TextValueOperand>("");

            fieldRefOperandStub.Stub(o => o.ToCaml()).Return(new XElement("fieldRefOperandStub"));
            valueOperandStub.Stub(o => o.ToCaml()).Return(new XElement("valueOperandStub"));

            var resultBuilder = new OperationResultBuilder();
            var operation = new BeginsWithOperation(resultBuilder, fieldRefOperandStub, valueOperandStub);

            // act
            var caml = operation.ToResult().ToString();

            // assert
            const string expected =
                @"<BeginsWith>
                    <fieldRefOperandStub />
                    <valueOperandStub />
                </BeginsWith>";
            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }
    }
}
