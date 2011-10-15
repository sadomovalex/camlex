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
using System.Linq.Expressions;
using System.Xml.Linq;
using CamlexNET.Interfaces.ReverseEngeneering;
using Microsoft.SharePoint;

namespace CamlexNET.Impl.ReverseEngeneering.Caml
{
    internal class ReTranslatorFromCaml : IReTranslator
    {
        private readonly IReAnalyzer analyzerForWhere;
        private readonly IReAnalyzer analyzerForOrderBy;
        private readonly IReAnalyzer analyzerForGroupBy;
        private readonly IReAnalyzer analyzerForViewFields;

        public XElement Where { get { return this.getElement(this.analyzerForWhere); } }
        public XElement OrderBy { get { return this.getElement(this.analyzerForOrderBy); } }
        public XElement GroupBy { get { return this.getElement(this.analyzerForGroupBy); } }
        public XElement ViewFields { get { return this.getElement(this.analyzerForViewFields); } }

        private XElement getElement(IReAnalyzer analyzer)
        {
            return (analyzer == null ? null : analyzer.Element);
        }

        public ReTranslatorFromCaml(IReAnalyzer analyzerForWhere, IReAnalyzer analyzerForOrderBy,
            IReAnalyzer analyzerForGroupBy, IReAnalyzer analyzerForViewFields)
        {
            this.analyzerForWhere = analyzerForWhere;
            this.analyzerForOrderBy = analyzerForOrderBy;
            this.analyzerForGroupBy = analyzerForGroupBy;
            this.analyzerForViewFields = analyzerForViewFields;
        }

        public LambdaExpression TranslateWhere()
        {
            return this.translate(this.analyzerForWhere, Tags.Where);
        }

        private LambdaExpression translate(IReAnalyzer analyzer, string tag)
        {
            if (analyzer == null)
            {
                return null;
            }
            if (!analyzer.IsValid())
            {
                throw new IncorrectCamlException(tag);
            }
            var operation = analyzer.GetOperation();
            var expr = operation.ToExpression();
            return Expression.Lambda(expr, Expression.Parameter(typeof(SPListItem), ReflectionHelper.CommonParameterName));
        }

        public LambdaExpression TranslateOrderBy()
        {
            return this.translate(this.analyzerForOrderBy, Tags.OrderBy);
        }

        public LambdaExpression TranslateGroupBy()
        {
            return this.translate(this.analyzerForGroupBy, Tags.GroupBy);
        }

        public LambdaExpression TranslateViewFields()
        {
            return this.translate(this.analyzerForViewFields, Tags.ViewFields);
        }
    }
}
