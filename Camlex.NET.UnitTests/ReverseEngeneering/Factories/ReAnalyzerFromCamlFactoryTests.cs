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
using CamlexNET.Impl.ReverseEngeneering.Caml.Analyzers;
using CamlexNET.Impl.ReverseEngeneering.Caml.Factories;
using NUnit.Framework;

namespace CamlexNET.UnitTests.ReverseEngeneering.Factories
{
    [TestFixture]
    public class ReAnalyzerFromCamlFactoryTests
    {
        [Test]
        public void test_THAT_for_order_by_array_analyzer_IS_returned()
        {
            var xml =
                "  <OrderBy>" +
                "    <FieldRef Name=\"Title\" Ascending=\"True\" />" +
                "  </OrderBy>";

            var f = new ReAnalyzerFromCamlFactory(null);
            Assert.IsInstanceOf<ReArrayAnalyzer>(f.Create(XmlHelper.Get(xml)));
        }

        [Test]
        public void test_THAT_for_group_by_array_analyzer_IS_returned()
        {
            var xml =
                "  <GroupBy>" +
                "    <FieldRef Name=\"Title\" />" +
                "  </GroupBy>";

            var f = new ReAnalyzerFromCamlFactory(null);
            Assert.IsInstanceOf<ReArrayAnalyzer>(f.Create(XmlHelper.Get(xml)));
        }

        [Test]
        public void test_THAT_for_view_fields_array_analyzer_IS_returned()
        {
            var xml =
                "  <ViewFields>" +
                "    <FieldRef Name=\"Title\" />" +
                "  </ViewFields>";

            var f = new ReAnalyzerFromCamlFactory(null);
            Assert.IsInstanceOf<ReArrayAnalyzer>(f.Create(XmlHelper.Get(xml)));
        }

        [Test]
        [ExpectedException(typeof(CamlAnalysisException))]
        public void test_WHEN_where_is_empty_THEN_exception_is_thrown()
        {
            var xml =
                "<Where>" +
                "</Where>";
            var f = new ReAnalyzerFromCamlFactory(null);
            f.Create(XmlHelper.Get(xml));
        }

        [Test]
        [ExpectedException(typeof(CamlAnalysisException))]
        public void test_WHEN_where_contains_unknown_child_THEN_exception_is_thrown()
        {
            var xml =
                "<Where>" +
                "<foo></foo>" +
                "</Where>";
            var f = new ReAnalyzerFromCamlFactory(null);
            f.Create(XmlHelper.Get(xml));
        }

        [Test]
        public void test_THAT_for_where_correct_analyzer_IS_returned()
        {
            var f = new ReAnalyzerFromCamlFactory(null);
            Assert.IsInstanceOf<ReEqAnalyzer>(f.Create(XmlHelper.Get("<Where><Eq></Eq></Where>")));
            Assert.IsInstanceOf<ReGeqAnalyzer>(f.Create(XmlHelper.Get("<Where><Geq></Geq></Where>")));
            Assert.IsInstanceOf<ReGtAnalyzer>(f.Create(XmlHelper.Get("<Where><Gt></Gt></Where>")));
            Assert.IsInstanceOf<ReLeqAnalyzer>(f.Create(XmlHelper.Get("<Where><Leq></Leq></Where>")));
            Assert.IsInstanceOf<ReLtAnalyzer>(f.Create(XmlHelper.Get("<Where><Lt></Lt></Where>")));
            Assert.IsInstanceOf<ReNeqAnalyzer>(f.Create(XmlHelper.Get("<Where><Neq></Neq></Where>")));
            Assert.IsInstanceOf<ReIsNullAnalyzer>(f.Create(XmlHelper.Get("<Where><IsNull></IsNull></Where>")));
            Assert.IsInstanceOf<ReIsNotNullAnalyzer>(f.Create(XmlHelper.Get("<Where><IsNotNull></IsNotNull></Where>")));
            Assert.IsInstanceOf<ReAndAlsoAnalyzer>(f.Create(XmlHelper.Get("<Where><And></And></Where>")));
            Assert.IsInstanceOf<ReBeginsWithAnalyzer>(f.Create(XmlHelper.Get("<Where><BeginsWith></BeginsWith></Where>")));
            Assert.IsInstanceOf<ReContainsAnalyzer>(f.Create(XmlHelper.Get("<Where><Contains></Contains></Where>")));
            Assert.IsInstanceOf<ReDateRangesOverlapAnalyzer>(f.Create(XmlHelper.Get("<Where><DateRangesOverlap></DateRangesOverlap></Where>")));
            Assert.IsInstanceOf<ReOrElseAnalyzer>(f.Create(XmlHelper.Get("<Where><Or></Or></Where>")));
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void test_WHEN_null_is_specified_THEN_exception_is_thrown()
        {
            var f = new ReAnalyzerFromCamlFactory(null);
            f.Create(null);
        }

        [Test]
        [ExpectedException(typeof(CamlAnalysisException))]
        public void test_THAT_for_unknown_tag_exception_IS_thrown()
        {
            var f = new ReAnalyzerFromCamlFactory(null);
            f.Create(XmlHelper.Get("<foo></foo>"));
        }
    }
}
