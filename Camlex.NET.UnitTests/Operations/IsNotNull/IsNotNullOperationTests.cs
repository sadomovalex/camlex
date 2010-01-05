using System.Xml.Linq;
using Camlex.NET.Impl.Operands;
using Camlex.NET.Impl.Operations.IsNotNull;
using Camlex.NET.Impl.Operations.Lt;
using Camlex.NET.UnitTests.Helpers;
using NUnit.Framework;
using Rhino.Mocks;

namespace Camlex.NET.UnitTests.Operations.IsNotNull
{
    [TestFixture]
    public class IsNotNullOperationTests
    {
        [Test]
        public void test_THAT_isnotnull_operation_IS_rendered_to_caml_properly()
        {
            // arrange
            var fieldRefOperandStub = MockRepository.GenerateStub<FieldRefOperand>("");
            var valueOperandStub = MockRepository.GenerateStub<IntegerValueOperand>(0);

            fieldRefOperandStub.Stub(o => o.ToCaml()).Return(new XElement("fieldRefOperandStub"));
            valueOperandStub.Stub(o => o.ToCaml()).Return(new XElement("valueOperandStub"));

            var operation = new IsNotNullOperation(null, fieldRefOperandStub);

            // act
            string caml = operation.ToResult().ToString();

            // assert
            string expected =
                @"<IsNotNull>
                    <fieldRefOperandStub />
                </IsNotNull>";
            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }
    }
}


