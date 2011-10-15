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
    public class FieldRefOperandWithOrderingTests
    {
        [Test]
        public void test_THAT_field_ref_operand_with_asc_IS_converted_to_expression_correctly()
        {
            var op = new FieldRefOperand("Title");
            var opOrder = new FieldRefOperandWithOrdering(op, new Camlex.Asc());
            var expr = opOrder.ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("(x.get_Item(\"Title\") As Asc)"));
            // problem 1 - Asc is not accessible. Only Camlex.Asc can be used
            // problem 2 - As instead of as is used. It won't be compiled
        }

        [Test]
        public void test_THAT_field_ref_operand_with_desc_IS_converted_to_expression_correctly()
        {
            var op = new FieldRefOperand("Title");
            var opOrder = new FieldRefOperandWithOrdering(op, new Camlex.Desc());
            var expr = opOrder.ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("(x.get_Item(\"Title\") As Desc)"));
        }

        [Test]
        public void test_THAT_field_ref_operand_with_none_IS_converted_to_expression_correctly()
        {
            var op = new FieldRefOperand("Title");
            var opOrder = new FieldRefOperandWithOrdering(op, new Camlex.None());
            var expr = opOrder.ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("x.get_Item(\"Title\")"));
        }
    }
}
