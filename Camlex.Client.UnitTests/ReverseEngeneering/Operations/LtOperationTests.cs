﻿#region Copyright(c) Alexey Sadomov, Vladimir Timashkov. All Rights Reserved.

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

using System.Linq.Expressions;
using CamlexNET.Impl.Operands;
using CamlexNET.Impl.Operations.Lt;
using NUnit.Framework;

namespace CamlexNET.UnitTests.ReverseEngeneering.Operations
{
    [TestFixture]
    public class LtOperationTests
    {
        [Test]
        [SetCulture("ru-RU")]
        [TestCase(1, "(Convert(x.get_Item(\"Count\")) < 1)")]
        [TestCase(-1, "(Convert(x.get_Item(\"Count\")) < -1)")]
        [TestCase(-1.45, "(Convert(x.get_Item(\"Count\")) < -1,45)")]
        public void test_THAT_lt_operation_with_double_IS_converted_to_expression_correctly(double value, string result)
        {
            var op1 = new FieldRefOperand("Count");
            var op2 = new NumberValueOperand(value);
            var op = new LtOperation(null, op1, op2);
            Expression expr = op.ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo(result));
        }

        [Test]
        public void test_THAT_lt_operation_with_int_IS_converted_to_expression_correctly()
        {
            var op1 = new FieldRefOperand("Count");
            var op2 = new IntegerValueOperand(1);
            var op = new LtOperation(null, op1, op2);
            Expression expr = op.ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("(Convert(x.get_Item(\"Count\")) < 1)"));
        }
    }
}