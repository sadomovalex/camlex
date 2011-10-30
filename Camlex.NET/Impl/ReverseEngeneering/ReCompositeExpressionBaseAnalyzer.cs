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
using CamlexNET.Interfaces;
using CamlexNET.Interfaces.ReverseEngeneering;

namespace CamlexNET.Impl.ReverseEngeneering
{
    // base class for AndAlso and OrElse analyzers
    internal abstract class ReCompositeExpressionBaseAnalyzer : ReBaseAnalyzer
    {
        protected IReAnalyzerFactory reAnalyzerFactory;

        protected ReCompositeExpressionBaseAnalyzer(XElement el, IReOperandBuilder operandBuilder,
            IReAnalyzerFactory reAnalyzerFactory) 
            : base(el, operandBuilder)
        {
            this.reAnalyzerFactory = reAnalyzerFactory;
        }

        public override bool IsValid()
        {
            if (!base.IsValid())
            {
                return false;
            }
            if (el.Attributes().Count() > 0)
            {
                return false;
            }
            if (el.Elements().Count() != 2)
            {
                return false;
            }
            if (!hasValidSubElement(el.Elements().First()))
            {
                return false;
            }
            if (!hasValidSubElement(el.Elements().Skip(1).First()))
            {
                return false;
            }
            return true;
        }

        protected bool hasValidSubElement(XElement subElement)
        {
            var name = subElement.Name;
            if (name != Tags.And && name != Tags.Or &&
                name != Tags.Eq && name != Tags.Neq &&
                name != Tags.Geq && name != Tags.Gt && name != Tags.Leq && name != Tags.Lt &&
                name != Tags.BeginsWith && name != Tags.Contains &&
                name != Tags.IsNull && name != Tags.IsNotNull &&
                name != Tags.DateRangesOverlap) return false;

            var reSubAnalyzer = reAnalyzerFactory.Create(subElement);
            return reSubAnalyzer.IsValid();
        }

        protected IOperation createOperation(XElement subElement)
        {
            var reSubAnalyzer = reAnalyzerFactory.Create(subElement);
            return reSubAnalyzer.GetOperation();
        }

        protected IOperation getOperation<T>(Func<IOperation, IOperation, T> constructor)
            where T : IOperation
        {
            if (!IsValid())
                throw new CamlAnalysisException(string.Format(
                    "Can't create {0} from the following xml: {1}", typeof(T).Name, el));

            var firstOperation = createOperation(el.Elements().First());
            var secondOperation = createOperation(el.Elements().Skip(1).First());
            return constructor(firstOperation, secondOperation);
        }
    }
}