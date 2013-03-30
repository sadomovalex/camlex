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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CamlexNET.Impl.Operands;
using NUnit.Framework;

namespace CamlexNET.UnitTests.Operands
{
    [TestFixture]
    public class GuidValueOperandTests
    {
        [Test]
        public void test_THAT_guid_value_IS_rendered_to_caml_properly()
        {
            var guid = new Guid("4feaf1f3-5b04-4d93-b0fc-4e48d0c60eed");
            var operand = new GuidValueOperand(guid);
            string caml = operand.ToCaml().ToString();
            Assert.That(caml, Is.EqualTo("<Value Type=\"Guid\">4feaf1f3-5b04-4d93-b0fc-4e48d0c60eed</Value>"));
        }

        [Test]
        public void test_THAT_guid_value_IS_successfully_created_from_valid_string()
        {
            var operand = new GuidValueOperand("4feaf1f3-5b04-4d93-b0fc-4e48d0c60eed");

            var guid = new Guid("4feaf1f3-5b04-4d93-b0fc-4e48d0c60eed");
            Assert.That(operand.Value, Is.EqualTo(guid));
        }

        [Test]
        [ExpectedException(typeof(InvalidValueForOperandTypeException))]
        public void test_WHEN_string_is_not_valid_guid_THEN_exception_is_thrown()
        {
            var operand = new GuidValueOperand("asd");
            var o = operand.Value;
        }
    }
}
