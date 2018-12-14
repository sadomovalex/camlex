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
using CamlexNET.Impl.Operations.AndAlso;
using CamlexNET.Impl.Operations.Eq;
using NUnit.Framework;

namespace CamlexNET.UnitTests.ReverseEngeneering.Operations
{
    [TestFixture]
    public class AndAlsoOperationTests
    {
        [Test]
        public void test_THAT_and_also_operation_IS_converted_to_expression_correctly()
        {
            var op11 = new FieldRefOperand("Status");
            var op12 = new BooleanValueOperand(true);
            var op1 = new EqOperation(null, op11, op12);

            var op21 = new FieldRefOperand("Status");
            var op22 = new BooleanValueOperand(false);
            var op2 = new EqOperation(null, op21, op22);

            var op = new AndAlsoOperation(null, op1, op2);

            var expr = op.ToExpression();
            Assert.That(expr.ToString(), Is.EqualTo("(Convert(x.get_Item(\"Status\")) AndAlso Not(Convert(x.get_Item(\"Status\"))))"));
        }
    }
}
