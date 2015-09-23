#region Copyright(c) Alexey Sadomov, Vladimir Timashkov, Stef Heyenrath. All Rights Reserved.
// -----------------------------------------------------------------------------
// Copyright(c) 2010 Alexey Sadomov, Vladimir Timashkov, Stef Heyenrath. All Rights Reserved.
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
using System.Linq.Expressions;
using System.Xml.Linq;
using CamlexNET.Impl.Operations.Array;
using CamlexNET.Impl.Operations.Join;
using CamlexNET.Interfaces.ReverseEngeneering;
using Microsoft.SharePoint.Client;

namespace CamlexNET.Impl.ReverseEngeneering.Caml
{
    internal class ReTranslatorFromCaml : IReTranslator
    {
        private readonly IReAnalyzer analyzerForWhere;
        private readonly IReAnalyzer analyzerForOrderBy;
        private readonly IReAnalyzer analyzerForGroupBy;
		private readonly IReAnalyzer analyzerForRowLimit;
        private readonly IReAnalyzer analyzerForViewFields;
        private readonly IReAnalyzer analyzerForJoins;
        private readonly IReAnalyzer analyzerForProjectedFields;

        public XElement Where { get { return this.getElement(this.analyzerForWhere); } }
        public XElement OrderBy { get { return this.getElement(this.analyzerForOrderBy); } }
        public XElement GroupBy { get { return this.getElement(this.analyzerForGroupBy); } }
		public XElement RowLimit { get { return this.getElement(this.analyzerForRowLimit); } }
        public XElement ViewFields { get { return this.getElement(this.analyzerForViewFields); } }
        public XElement Joins { get { return this.getElement(this.analyzerForJoins); } }
        public XElement ProjectedFields { get { return this.getElement(this.analyzerForProjectedFields); } }

	    public ReTranslatorFromCaml(IReAnalyzer analyzerForWhere, IReAnalyzer analyzerForOrderBy,
            IReAnalyzer analyzerForGroupBy, IReAnalyzer analyzerForViewFields, IReAnalyzer analyzerForJoins, IReAnalyzer analyzerForProjectedField, IReAnalyzer analyzerForRowLimits)
        {
            this.analyzerForWhere = analyzerForWhere;
            this.analyzerForOrderBy = analyzerForOrderBy;
            this.analyzerForGroupBy = analyzerForGroupBy;
            this.analyzerForViewFields = analyzerForViewFields;
	        this.analyzerForRowLimit = analyzerForRowLimit;
            this.analyzerForProjectedFields = analyzerForProjectedFields;
            this.analyzerForJoins = analyzerForJoins;
            this.analyzerForProjectedFields = analyzerForProjectedFields;
        }

        public LambdaExpression TranslateWhere()
        {
            return this.translateWhere(this.analyzerForWhere, Tags.Where);
        }

        private LambdaExpression translateWhere(IReAnalyzer analyzer, string tag)
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
            return Expression.Lambda(expr, Expression.Parameter(typeof(ListItem), ReflectionHelper.CommonParameterName));
        }

		public Expression TranslateRowLimit()
		{
			return this.translateRowLimit(this.analyzerForRowLimit, Tags.RowLimit);
		}

		private Expression translateRowLimit(IReAnalyzer analyzer, string tag)
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
		    return expr;
		}

        private LambdaExpression translateArrayOperation(IReAnalyzer analyzer, string tag)
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

            if (!(operation is ArrayOperation))
            {
                throw new CamlAnalysisException("ArrayOperation is expected");
            }

            int operandsCount = ((ArrayOperation) operation).OperandsCount;
            if (operandsCount == 0)
            {
                throw new CamlAnalysisException(
                    "ArrayOperation doesn't contain operands. There should be at least one operand");
            }

            var expr = operation.ToExpression();

            // this is important to specify the type of returning value explicitly in generic, becase otherwise
            // it will determine type by itself and it will be incorrect for ArrayOperation with orderings.
            // I.e. it will be Func<ListItem, Camlex+Desc>, instead of Func<ListItem,object>. In turn
            // it will cause exception in ReLinker, because parameter of type Func<ListItem, Camlex+Desc>
            // can not be passed to the method OrderBy(Expression<Func<ListItem, object>> expr);
            if (operandsCount == 1)
            {
                return Expression.Lambda<Func<ListItem, object>>(expr,
                                                           Expression.Parameter(typeof (ListItem),
                                                                                ReflectionHelper.CommonParameterName));
            }

            return Expression.Lambda<Func<ListItem, object[]>>(expr,
                                                       Expression.Parameter(typeof(ListItem),
                                                                            ReflectionHelper.CommonParameterName));
        }

        public LambdaExpression TranslateOrderBy()
        {
            return this.translateArrayOperation(this.analyzerForOrderBy, Tags.OrderBy);
        }

        public LambdaExpression TranslateGroupBy(out GroupByParams groupByParams)
        {
            groupByParams = new GroupByParams();
            if (this.GroupBy != null)
            {
                groupByParams.HasCollapse = this.GroupBy.Attributes(Attributes.Collapse).Count() > 0;
                groupByParams.HasGroupLimit = this.GroupBy.Attributes(Attributes.GroupLimit).Count() > 0;

                groupByParams.Collapse = false;
                groupByParams.GroupLimit = 0;

                if (groupByParams.HasCollapse)
                {
                    if (!bool.TryParse((string) this.GroupBy.Attribute(Attributes.Collapse), out groupByParams.Collapse))
                    {
                        throw new CantParseBooleanAttributeException(Attributes.Collapse);
                    }
                }
                if (groupByParams.HasGroupLimit)
                {
                    if (
                        !int.TryParse((string) this.GroupBy.Attribute(Attributes.GroupLimit),
                                      out groupByParams.GroupLimit))
                    {
                        throw new CantParseIntegerAttributeException(Attributes.GroupLimit);
                    }
                }
            }
            return this.translateArrayOperation(this.analyzerForGroupBy, Tags.GroupBy);
        }

        public LambdaExpression TranslateViewFields()
        {
            return this.translateArrayOperation(this.analyzerForViewFields, Tags.ViewFields);
        }

        public List<KeyValuePair<LambdaExpression, JoinType>> TranslateJoins()
        {
            if (analyzerForJoins == null)
            {
                return null;
            }
            if (!analyzerForJoins.IsValid())
            {
                throw new IncorrectCamlException(Tags.Joins);
            }
            var operations = analyzerForJoins.GetOperations();
            if (operations == null)
            {
                return new List<KeyValuePair<LambdaExpression, JoinType>>();
            }
            var result = new List<KeyValuePair<LambdaExpression, JoinType>>();
            foreach (var operation in operations)
            {
                var expr = operation.ToExpression();
                result.Add(new KeyValuePair<LambdaExpression, JoinType>(Expression.Lambda(expr, Expression.Parameter(typeof(ListItem), ReflectionHelper.CommonParameterName)),
                    ((JoinOperation)operation).Type));
            }
            return result;
        }

        public List<LambdaExpression> TranslateProjectedFields()
        {
            if (analyzerForProjectedFields == null)
            {
                return null;
            }
            if (!analyzerForProjectedFields.IsValid())
            {
                throw new IncorrectCamlException(Tags.ProjectedFields);
            }
            var operations = analyzerForProjectedFields.GetOperations();
            if (operations == null)
            {
                return new List<LambdaExpression>();
            }
            var result = new List<LambdaExpression>();
            foreach (var operation in operations)
            {
                var expr = operation.ToExpression();
                result.Add(Expression.Lambda(expr, Expression.Parameter(typeof(ListItem), ReflectionHelper.CommonParameterName)));
            }
            return result;
        }

		private XElement getElement(IReAnalyzer analyzer)
		{
			return (analyzer == null ? null : analyzer.Element);
		}
    }
}
