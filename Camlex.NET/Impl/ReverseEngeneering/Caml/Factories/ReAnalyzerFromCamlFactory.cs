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
using System.Linq;
using System.Xml.Linq;
using CamlexNET.Impl.ReverseEngeneering.Caml.Analyzers;
using CamlexNET.Interfaces.ReverseEngeneering;

namespace CamlexNET.Impl.ReverseEngeneering.Caml.Factories
{
    internal class ReAnalyzerFromCamlFactory : IReAnalyzerFactory
    {
        private readonly IReOperandBuilder operandBuilder;
        //private readonly ReOperationResultBuilder operationResultBuilder;

        public ReAnalyzerFromCamlFactory(IReOperandBuilder operandBuilder)
        {
            this.operandBuilder = operandBuilder;
        }

        public IReAnalyzer Create(XElement el)
        {
            if (el == null)
            {
                throw new ArgumentNullException("el");
            }
            if (el.Name == Tags.Where)
            {
                return this.getAnalyzerForWhere(el);
            }
            if (el.Name == Tags.OrderBy || el.Name == Tags.GroupBy ||
                el.Name == Tags.ViewFields)
            {
                return this.getAnalyzerForAray(el);
            }
            var reAnalyzer = getAnalyzer(el);
            if (reAnalyzer == null)
            {
                throw new CamlAnalysisException(string.Format("Tag '{0}' is not supported", el.Name));
            }
            return reAnalyzer;
        }

        private IReAnalyzer getAnalyzerForWhere(XElement el)
        {
            if (el.Elements().Count() != 1)
            {
                throw new CamlAnalysisException("WHERE tag should contain only 1 child element");
            }
            var element = el.Elements().First();
            var reAnalyzer = getAnalyzer(element);
            if (reAnalyzer == null)
            {
                throw new CamlAnalysisException(
                    string.Format("WHERE tag contain element which can't be translated: \n{0}", element));
            }
            return reAnalyzer;
        }

        private IReAnalyzer getAnalyzerForAray(XElement el)
        {
            return new ReArrayAnalyzer(el, this.operandBuilder);
        }

        private IReAnalyzer getAnalyzer(XElement el)
        {
            if (el.Name == Tags.And) return new ReAndAlsoAnalyzer(el, operandBuilder, this);
            if (el.Name == Tags.BeginsWith) return new ReBeginsWithAnalyzer(el, operandBuilder);
            if (el.Name == Tags.Contains) return new ReContainsAnalyzer(el, operandBuilder);
            if (el.Name == Tags.Eq) return new ReEqAnalyzer(el, operandBuilder);
            if (el.Name == Tags.Geq) return new ReGeqAnalyzer(el, operandBuilder);
            if (el.Name == Tags.Gt) return new ReGtAnalyzer(el, operandBuilder);
            if (el.Name == Tags.IsNotNull) return new ReIsNotNullAnalyzer(el, operandBuilder);
            if (el.Name == Tags.IsNull) return new ReIsNullAnalyzer(el, operandBuilder);
            if (el.Name == Tags.Leq) return new ReLeqAnalyzer(el, operandBuilder);
            if (el.Name == Tags.Lt) return new ReLtAnalyzer(el, operandBuilder);
            if (el.Name == Tags.Neq) return new ReNeqAnalyzer(el, operandBuilder);
            if (el.Name == Tags.Or) return new ReAndAlsoAnalyzer(el, operandBuilder, this);
            return null;
        }

//        private IReAnalyzer getAnalyzer<T>(XElement el, string tagName, Func<XElement, IReOperandBuilder, T> constructor)
//            where T : IReAnalyzer
//        {
//            var element = el.Elements().FirstOrDefault(e => e.Name == tagName);
//            if (element != null)
//            {
//                return constructor(element, this.operandBuilder);
//            }
//        }
    }
}
