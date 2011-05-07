using System;
using CamlexNET.Impl.Operands;
using CamlexNET.Impl.Operations.AndAlso;
using CamlexNET.Impl.Operations.DateRangesOverlap;
using CamlexNET.Impl.Operations.Eq;
using CamlexNET.Impl.Operations.OrElse;
using NUnit.Framework;

namespace CamlexNET.UnitTests.ReverseEngeneering.Operations
{
    [TestFixture]
    public class DateRangesOverlapOperationTests
    {
        [Test]
        public void test_THAT_date_ranges_overlap_operation_with_native_date_time_IS_converted_to_expression_correctly()
        {
            var op1 = new FieldRefOperand("StartField");
            var op2 = new FieldRefOperand("StopField");
            var op3 = new FieldRefOperand("RecurrenceID");
            var dt = new DateTime(2011, 5, 7, 21, 30, 00, 00);
            var op4 = new DateTimeValueOperand(dt, false);
            var op = new DateRangesOverlapOperation(null, op1, op2, op3, op4);

            var expr = op.ToExpression();
            Assert.That(expr.ToString(),
                Is.EqualTo(string.Format("DateRangesOverlap(x.get_Item(\"StartField\"), x.get_Item(\"StopField\"), x.get_Item(\"RecurrenceID\"), {0})", dt)));
        }

        [Test]
        public void test_THAT_date_ranges_overlap_operation_with_string_based_date_time_IS_converted_to_expression_correctly()
        {
            var op1 = new FieldRefOperand("StartField");
            var op2 = new FieldRefOperand("StopField");
            var op3 = new FieldRefOperand("RecurrenceID");
            var dt = new DateTime(2011, 5, 7, 21, 30, 00, 00);
            var op4 = new DateTimeValueOperand(Camlex.Now, false);
            var op = new DateRangesOverlapOperation(null, op1, op2, op3, op4);

            var expr = op.ToExpression();
            Assert.That(expr.ToString(),
                Is.EqualTo("DateRangesOverlap(x.get_Item(\"StartField\"), x.get_Item(\"StopField\"), x.get_Item(\"RecurrenceID\"), Convert(Convert(\"Now\")))"));
        }
    }
}
