using System.Xml.Linq;
using Camlex.NET.Impl.Factories;
using Camlex.NET.Impl.Operands;
using Camlex.NET.Impl.Operations.Contains;
using Camlex.NET.UnitTests.Helpers;
using NUnit.Framework;
using Rhino.Mocks;

namespace Camlex.NET.UnitTests.Operations.Contains
{
    [TestFixture]
    public class ContainsOperationTests
    {
        [Test]
        public void TestThatContainsOperationIsRenderedToCamlProperly()
        {
            // arrange
            var fieldRefOperandStub = MockRepository.GenerateStub<FieldRefOperand>("");
            var valueOperandStub = MockRepository.GenerateStub<TextValueOperand>("");

            fieldRefOperandStub.Stub(o => o.ToCaml()).Return(new XElement("fieldRefOperandStub"));
            valueOperandStub.Stub(o => o.ToCaml()).Return(new XElement("valueOperandStub"));

            var resultBuilder = new OperationResultBuilder();
            var operation = new ContainsOperation(resultBuilder, fieldRefOperandStub, valueOperandStub);

            // act
            var caml = operation.ToResult().ToString();

            // assert
            const string expected =
                @"<Contains>
                    <fieldRefOperandStub />
                    <valueOperandStub />
                </Contains>";
            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }
    }
}
