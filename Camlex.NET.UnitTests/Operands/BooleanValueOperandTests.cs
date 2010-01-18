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

namespace CamlexNET.UnitTests.Operands
{
    [TestFixture]
    public class BooleanValueOperandTests
    {
        [Test]
        public void test_THAT_boolean_value_IS_rendered_to_caml_properly()
        {
            var operand = new BooleanValueOperand(true);
            string caml = operand.ToCaml().ToString();
            Assert.That(caml, Is.EqualTo("<Value Type=\"Boolean\">1</Value>"));
        }

        [Test]
        public void test_THAT_boolean_value_IS_successfully_created_from_valid_string1()
        {
            var operand = new BooleanValueOperand("True");
            Assert.That(operand.Value, Is.True);
        }

        [Test]
        public void test_THAT_boolean_value_IS_successfully_created_from_valid_string2()
        {
            var operand = new BooleanValueOperand("False");
            Assert.That(operand.Value, Is.False);
        }

        [Test]
        public void test_THAT_boolean_value_IS_successfully_created_from_valid_string3()
        {
            var operand = new BooleanValueOperand("1");
            Assert.That(operand.Value, Is.True);
        }

        [Test]
        public void test_THAT_boolean_value_IS_successfully_created_from_valid_string4()
        {
            var operand = new BooleanValueOperand("0");
            Assert.That(operand.Value, Is.False);
        }

        [Test]
        [ExpectedException(typeof(InvalidValueForOperandTypeException))]
        public void test_WHEN_string_is_not_valid_boolean_THEN_exception_is_thrown1()
        {
            var operand = new BooleanValueOperand("asd");
            Assert.That(operand.Value, Is.False);
        }

        [Test]
        [ExpectedException(typeof(InvalidValueForOperandTypeException))]
        public void test_WHEN_string_is_not_valid_boolean_THEN_exception_is_thrown2()
        {
            var operand = new BooleanValueOperand("2");
            Assert.That(operand.Value, Is.False);
        }
    }
}
