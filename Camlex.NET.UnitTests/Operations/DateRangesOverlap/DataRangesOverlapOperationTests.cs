﻿using System.Xml.Linq;
using CamlexNET.Impl.Factories;
using CamlexNET.Impl.Operands;
using CamlexNET.Impl.Operations.DataRangesOverlap;
using CamlexNET.UnitTests.Helpers;
using NUnit.Framework;
using Rhino.Mocks;

namespace CamlexNET.UnitTests.Operations.DateRangesOverlap
{
    [TestFixture]
    public class DataRangesOverlapOperationTests
    {
        [Test]
        public void test_THAT_datarangesoverlap_operation_IS_rendered_to_caml_properly()
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
            var operation = new DataRangesOverlapOperation(resultBuilder,
                startFieldRefOperandStub, stopFieldRefOperandStub, recurrenceFieldRefOperandStub, dateTimevalueOperandStub);

            // act
            var caml = operation.ToResult().ToString();

            // assert
            const string expected =
                @"<DataRangesOverlap>
                    <startFieldRefOperandStub />
                    <stopFieldRefOperandStub />
                    <recurrenceFieldRefOperandStub />
                    <dateTimevalueOperandStub />
                </DataRangesOverlap>";
            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }
    }
}