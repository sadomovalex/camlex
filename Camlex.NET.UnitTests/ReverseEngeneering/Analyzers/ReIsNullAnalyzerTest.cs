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
using System.Reflection;
using System.Text;
using CamlexNET.Impl.Operands;
using CamlexNET.Impl.Operations.Array;
using CamlexNET.Impl.Operations.Eq;
using CamlexNET.Impl.ReverseEngeneering.Caml.Analyzers;
using CamlexNET.Interfaces;
using CamlexNET.Interfaces.ReverseEngeneering;
using NUnit.Framework;
using Rhino.Mocks;

namespace CamlexNET.UnitTests.ReverseEngeneering.Analyzers
{
    public class ReIsNullAnalyzerTest
    {
        [Test]
        public void test_WHEN_xml_is_null_THEN_expression_is_not_valid()
        {
            var analyzer = new ReIsNullAnalyzer(null, null);
            Assert.IsFalse(analyzer.IsValid());
        }

        [Test]
        public void test_WHEN_field_ref_not_specified_THEN_expression_is_not_valid()
        {
            var xml =
                "<IsNull>" +
                "</IsNull>";

            var analyzer = new ReIsNullAnalyzer(XmlHelper.Get(xml), null);
            Assert.IsFalse(analyzer.IsValid());
        }

        [Test]
        public void test_WHEN_field_ref_specified_THEN_expression_is_valid()
        {
            var xml =
                "<IsNull>" +
                "    <FieldRef Name=\"Title\" />" +
                "</IsNull>";

            var analyzer = new ReIsNullAnalyzer(XmlHelper.Get(xml), null);
            Assert.IsTrue(analyzer.IsValid());
        }

        [Test]
        public void test_WHEN_field_ref_specified_THEN_is_not_null_expression_is_valid()
        {
            var xml =
                "<IsNotNull>" +
                "    <FieldRef Name=\"Title\" />" +
                "</IsNotNull>";

            var analyzer = new ReIsNotNullAnalyzer(XmlHelper.Get(xml), null);
            Assert.IsTrue(analyzer.IsValid());
        }
    }
}