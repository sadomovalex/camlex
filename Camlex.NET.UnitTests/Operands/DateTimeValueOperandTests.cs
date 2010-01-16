#region Copyright(c) Alexey Sadomov, Vladimir Timashkov. All Rights Reserved.
// -----------------------------------------------------------------------------
// Copyright(c) 2007 Alexey Sadomov, Vladimir Timashkov. All Rights Reserved.
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
using CamlexNET;
using CamlexNET.Impl.Operands;
using NUnit.Framework;

namespace Camlex.NET.UnitTests.Operands
{
    [TestFixture]
    public class DateTimeValueOperandTests
    {
        [Test]
        public void test_THAT_datetime_value_with_includetimevalue_IS_rendered_to_caml_properly()
        {
            var dateTime = DateTime.Now;
            var operand = new DateTimeValueOperand(dateTime, true);
            var caml = operand.ToCaml().ToString();
            Assert.That(caml, Is.EqualTo("<Value Type=\"DateTime\" IncludeTimeValue=\"True\">" + dateTime.ToString("s") + "</Value>"));
        }

        [Test]
        public void test_THAT_datetime_value_without_includetimevalue_IS_rendered_to_caml_properly()
        {
            var dateTime = DateTime.Now;
            var operand = new DateTimeValueOperand(dateTime, false);
            var caml = operand.ToCaml().ToString();
            Assert.That(caml, Is.EqualTo("<Value Type=\"DateTime\">" + dateTime.ToString("s") + "</Value>"));
        }

        [Test]
        public void test_THAT_datetime_value_IS_successfully_created_from_valid_string()
        {
            var operand = new DateTimeValueOperand("02.01.2010 03:04:05", true);
            Assert.That(operand.Value, Is.EqualTo(new DateTime(2010, 1, 2, 3, 4, 5)));
        }

        [Test]
        [ExpectedException(typeof(InvalidValueForOperandTypeException))]
        public void test_WHEN_datetime_value_is_not_valid_datetime_THEN_exception_is_thrown()
        {
            var operand = new DateTimeValueOperand("abc", true);
        }
    }
}
