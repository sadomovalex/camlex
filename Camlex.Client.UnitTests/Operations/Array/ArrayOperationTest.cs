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
using System.Xml.Linq;
using CamlexNET.Impl.Factories;
using CamlexNET.Impl.Operations.Array;
using CamlexNET.Impl.Operands;
using CamlexNET.UnitTests.Helpers;
using NUnit.Framework;
using Rhino.Mocks;

namespace CamlexNET.UnitTests.Array
{
    [TestFixture]
    public class ArrayOperationTest
    {
        [Test]
        public void test_THAT_operation_IS_rendered_to_caml_properly()
        {
            // arrange
            var fieldRefOperandStub1 = MockRepository.GenerateStub<FieldRefOperand>("");
            var fieldRefOperandStub2 = MockRepository.GenerateStub<FieldRefOperand>("");

            fieldRefOperandStub1.Stub(o => o.ToCaml()).Return(new XElement("fieldRefOperandStub1"));
            fieldRefOperandStub2.Stub(o => o.ToCaml()).Return(new XElement("fieldRefOperandStub2"));

            var resultBuilder = new OperationResultBuilder();
            var operation = new ArrayOperation(resultBuilder, fieldRefOperandStub1, fieldRefOperandStub2);

            // act
            string caml = operation.ToResult().ToString();

            // assert
            string expected =
                @"<fieldRefOperandStub1 />
                  <fieldRefOperandStub2 />";
            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }
    }
}
