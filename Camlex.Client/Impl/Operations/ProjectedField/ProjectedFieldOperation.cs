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
using System.Linq.Expressions;
using CamlexNET.Impl.Operands;
using CamlexNET.Interfaces;
using Microsoft.SharePoint.Client;

namespace CamlexNET.Impl.Operations.ProjectedField
{
    internal class ProjectedFieldOperation : BinaryOperationBase
    {
        public ProjectedFieldOperation(IOperationResultBuilder operationResultBuilder,
            IOperand fieldRefOperand)
            : base(operationResultBuilder, fieldRefOperand, null)
        {
        }

        public override IOperationResult ToResult()
        {
            var primaryElement = this.fieldRefOperand.ToCaml();
            return this.operationResultBuilder.CreateResult(primaryElement);
        }

        public override Expression ToExpression()
        {
            var op = this.fieldRefOperand as FieldRefOperand;
            if (op == null)
            {
                throw new NullReferenceException("fieldRefOperand is null");
            }

            var attrs = op.Attributes;
            if (attrs == null)
            {
                throw new NullReferenceException("fieldRefOperand.Attributes are null");
            }

            string fieldName = op.FieldName;
            if (string.IsNullOrEmpty(fieldName))
            {
                throw new Exception("Name is empty");
            }

            if (!attrs.Any(a => a.Key == Attributes.List))
            {
                throw new Exception("List attribute is not specified");
            }
            string listName = attrs.First(a => a.Key == Attributes.List).Value;
            if (string.IsNullOrEmpty(listName))
            {
                throw new Exception("List is empty");
            }

            if (!attrs.Any(a => a.Key == Attributes.List))
            {
                throw new Exception("List attribute is not specified");
            }
            string showField = attrs.First(a => a.Key == Attributes.ShowField).Value;
            if (string.IsNullOrEmpty(showField))
            {
                throw new Exception("ShowField is empty");
            }

            var listMethodInfo = ReflectionHelper.GetExtensionMethods(typeof(Camlex).Assembly, typeof(object)).FirstOrDefault(
                m => m.Name == ReflectionHelper.ListMethodName);
            var showFieldMethodInfo = ReflectionHelper.GetExtensionMethods(typeof(Camlex).Assembly, typeof(object)).FirstOrDefault(
                m => m.Name == ReflectionHelper.ShowFieldMethodName);
            var listItemIndexerMethodInfo = typeof(ListItem).GetProperty(ReflectionHelper.Item, typeof(object), new[] { typeof(string) }, null).GetGetMethod();
            var fieldRefExpr = Expression.Call(Expression.Parameter(typeof(ListItem), ReflectionHelper.CommonParameterName),
                listItemIndexerMethodInfo, new[] { Expression.Constant(fieldName) });

            var internalExpr = Expression.Call(null, listMethodInfo, fieldRefExpr, Expression.Constant(listName));
            var ex = Expression.Call(null, showFieldMethodInfo, internalExpr, Expression.Constant(showField));
            return ex;
        }
    }
}


