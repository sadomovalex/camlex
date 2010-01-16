#region Copyright(c) Alexey Sadomov, Vladimir Timashkov. All Rights Reserved.
// -----------------------------------------------------------------------------
// Copyright(c) 2007 Alexey Sadomov, Vladimir Timashkov. All Rights Reserved.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//
//   1.  No Trademark License - Microsoft Public License (Ms-PL) does not grant you rights to use
//       authors names, logos, or trademarks.
//   2.  If you distribute any portion of the software, you must retain all copyright,
//       patent, trademark, and attribution notices that are present in the software.
//   3.  If you distribute any portion of the software in source code form, you may do
//       so only under this license by including a complete copy of Microsoft Public License (Ms-PL)
//       with your distribution. If you distribute any portion of the software in compiled
//       or object code form, you may only do so under a license that complies with
//       Microsoft Public License (Ms-PL).
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
using System.Linq.Expressions;
using System.Text;
using System.Xml.Linq;
using CamlexNET.Impl;
using CamlexNET.Impl.Operations.Results;
using CamlexNET.Interfaces;
using CamlexNET.UnitTests.Helpers;
using Microsoft.SharePoint;
using NUnit.Framework;
using Rhino.Mocks;

namespace CamlexNET.UnitTests
{
    [TestFixture]
    public class GenericTranslatorTests
    {
        XElementOperationResult xelementResult(string name)
        {
            return new XElementOperationResult(new XElement(name));
        }
        [Test]
        public void test_THAT_where_clause_IS_rendered_to_caml_properly()
        {
            var analyzer = MockRepository.GenerateStub<IAnalyzer>();
            var operation = MockRepository.GenerateStub<IOperation>();
            var translator = new GenericTranslator(analyzer);

            Expression<Func<SPItem, bool>> expr = x => true;
            analyzer.Stub(a => a.IsValid(expr)).Return(true);
            analyzer.Stub(a => a.GetOperation(expr)).Return(operation);
            operation.Stub(o => o.ToResult()).Return(xelementResult("foo"));

            string caml = translator.TranslateWhere(expr).ToString();
            string expected = "<Where><foo /></Where>";

            Assert.That(caml, Is.EqualTo(expected).Using(new CamlComparer()));
        }
    }
}
