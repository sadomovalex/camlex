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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CamlexNET.Impl.Operands;
using NUnit.Framework;

namespace CamlexNET.UnitTests.ReverseEngeneering.Operands
{
    [TestFixture]
    public class DateTimeValueOperandTests
    {
        [Test]
        public void test_THAT_operand_with_native_value_IS_conveted_to_expression_correctly()
        {
            var dt = new DateTime(2011, 4, 12, 22, 00, 00, 00);
            var op = new DateTimeValueOperand(dt, false);
            var expr = op.ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo(dt.ToString()));
        }

        [Test]
        public void test_THAT_operand_with_native_string_value_IS_conveted_to_expression_correctly()
        {
            var dt = new DateTime(2011, 4, 12, 22, 00, 00, 00);
            var op = new DateTimeValueOperand(dt.ToString(), false);
            var expr = op.ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo(dt.ToString()));
        }

        [Test]
        public void test_THAT_operand_with_now_IS_conveted_to_expression_correctly()
        {
            var op = new DateTimeValueOperand(Camlex.Now, false);
            var expr = op.ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("Convert(Convert(Camlex.Now))"));
        }

        [Test]
        public void test_THAT_operand_with_today_IS_conveted_to_expression_correctly()
        {
            var op = new DateTimeValueOperand(Camlex.Today, false);
            var expr = op.ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("Convert(Convert(Camlex.Today))"));
        }

        [Test]
        public void test_THAT_operand_with_week_IS_conveted_to_expression_correctly()
        {
            var op = new DateTimeValueOperand(Camlex.Week, false);
            var expr = op.ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("Convert(Convert(Camlex.Week))"));
        }

        [Test]
        public void test_THAT_operand_with_month_IS_conveted_to_expression_correctly()
        {
            var op = new DateTimeValueOperand(Camlex.Month, false);
            var expr = op.ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("Convert(Convert(Camlex.Month))"));
        }

        [Test]
        public void test_THAT_operand_with_year_IS_conveted_to_expression_correctly()
        {
            var op = new DateTimeValueOperand(Camlex.Year, false);
            var expr = op.ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("Convert(Convert(Camlex.Year))"));
        }

        [Test]
        public void test_THAT_native_operand_with_includetimevalue_IS_conveted_to_expression_correctly()
        {
            var dt = new DateTime(2011, 4, 16, 22, 00, 00, 00);
            var op = new DateTimeValueOperand(dt.ToString(), true);
            var expr = op.ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo(dt + ".IncludeTimeValue()"));
        }

        [Test]
        public void test_THAT_string_based_operand_with_includetimevalue_IS_conveted_to_expression_correctly()
        {
            var op = new DateTimeValueOperand(Camlex.Now, true);
            var expr = op.ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("Convert(Convert(Camlex.Now)).IncludeTimeValue()"));
        }
    }
}
