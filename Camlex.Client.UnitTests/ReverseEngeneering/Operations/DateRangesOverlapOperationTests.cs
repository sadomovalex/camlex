#region Copyright(c) Alexey Sadomov, Vladimir Timashkov. All Rights Reserved.
// -----------------------------------------------------------------------------
// Copyright(c) 2010 Alexey Sadomov, Vladimir Timashkov. All Rights Reserved.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//
//   1. No Trademark License - Microsoft Public License (Ms-PL) does not grant you rights to use
//      authors names, logos, or trademarks.
//   2. If you distribute any portion of the software, you must retain all copyright,
//      patent, trademark, and attribution notices that are present in the software.
//   3. If you distribute any portion of the software in source code form, you may do
//      so only under this license by including a complete copy of Microsoft Public License (Ms-PL)
//      with your distribution. If you distribute any portion of the software in compiled
//      or object code form, you may only do so under a license that complies with
//      Microsoft Public License (Ms-PL).
//   4. The names of the authors may not be used to endorse or promote products
//      derived from this software without specific prior written permission.
//
// The software is licensed "as-is." You bear the risk of using it. The authors
// give no express warranties, guarantees or conditions. You may have additional consumer
// rights under your local laws which this license cannot change. To the extent permitted
// under your local laws, the authors exclude the implied warranties of merchantability,
// fitness for a particular purpose and non-infringement.
// -----------------------------------------------------------------------------
#endregion
using System;
using CamlexNET.Impl.Operands;
using CamlexNET.Impl.Operations.DateRangesOverlap;
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
            var dt = new DateTime(2011, 5, 7, 21, 30, 00);
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
            var op4 = new DateTimeValueOperand(Camlex.Now, false);
            var op = new DateRangesOverlapOperation(null, op1, op2, op3, op4);

            var expr = op.ToExpression();
            Assert.That(expr.ToString(),
                Is.EqualTo("DateRangesOverlap(x.get_Item(\"StartField\"), x.get_Item(\"StopField\"), x.get_Item(\"RecurrenceID\"), Convert(Convert(Camlex.Now)))"));
        }
    }
}
