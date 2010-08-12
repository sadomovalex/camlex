using System.Xml.Linq;
using CamlexNET.Impl.Factories;
using CamlexNET.Impl.Operands;
using CamlexNET.Impl.Operations.DateRangesOverlap;
using CamlexNET.UnitTests.Helpers;
using NUnit.Framework;
using Rhino.Mocks;

namespace CamlexNET.UnitTests.Operations.DateRangesOverlap
{
    [TestFixture]
    public class DateRangesOverlapOperationTests
    {
        [Test]
        public void test_THAT_daterangesoverlap_operation_IS_rendered_to_caml_properly()
        {
            // arrange
            var startFieldRefOperandStub = MockRepository.GenerateStub<FieldRefOperand>("");
            var stopFieldRefOperandStub = MockRepository.GenerateStub<FieldRefOperand>("");
            var recurrenceFieldRefOperandStub = MockRepository.GenerateStub<FieldRefOperand>("");
            var dateTimevalueOperandStub = MockRepository.GenerateStub<TextValueOperand>("");

            startFieldRefOperandStub.Stub(o => o.ToCaml()).Return(new XElement("startFieldRefOperandStub"));
            stopFieldRefOperandStub.Stub(o => o.ToCaml()).Return(new XElement("stopFieldRefOperandStub"));
            recurrenceFieldRefOperandStub.Stub(o => o.ToCaml()).Return(new XElement("recurrenceFieldRefOperandStub"));
            dateTimevalueOperandStub.Stub(o => o.ToCaml()).Return(new XElement("dateTimevalueOperandStub"));

            var resultBuilder = new OperationResultBuilder();
            var operation = new DateRangesOverlapOperation(resultBuilder,
                startFieldRefOperandStub, stopFieldRefOperandStub, recurrenceFieldRefOperandStub, dateTimevalueOperandStub);

            // act
            var caml = operation.ToResult().ToString();

            // assert
            const string expected =
                @"<DateRangesOverlap>
                    <startFieldRefOperandStub />
                    <stopFieldRefOperandStub />
                    <recurrenceFieldRefOperandStub />
                    <dateTimevalueOperandStub />
                </DateRangesOverlap>";
            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }
    }
}
