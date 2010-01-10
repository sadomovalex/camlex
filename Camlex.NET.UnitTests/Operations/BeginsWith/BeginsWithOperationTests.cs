using System.Xml.Linq;
using Camlex.NET.Impl.Factories;
using Camlex.NET.Impl.Operands;
using Camlex.NET.Impl.Operations.BeginsWith;
using Camlex.NET.UnitTests.Helpers;
using NUnit.Framework;
using Rhino.Mocks;

namespace Camlex.NET.UnitTests.Operations.BeginsWith
{
    [TestFixture]
    public class BeginsWithOperationTests
    {
        [Test]
        public void TestThatBeginsWithOperationIsRenderedToCamlProperly()
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
