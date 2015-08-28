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
using System.Reflection;
using System.Xml.Linq;
using CamlexNET.Impl.Factories;
using CamlexNET.Impl.Operands;
using CamlexNET.Interfaces;
using Microsoft.SharePoint;

namespace CamlexNET.Impl.Operations.Join
{
    internal class JoinOperation : BinaryOperationBase
    {
        public JoinOperation(IOperationResultBuilder operationResultBuilder,
            IOperand fieldRefOperand, IOperand valueOperand)
            : base(operationResultBuilder, fieldRefOperand, valueOperand)
        {
        }

        public override IOperationResult ToResult()
        {
            var primaryElement = this.fieldRefOperand.ToCaml();
            this.sortAttributes(primaryElement);

            var foreignElement = this.valueOperand.ToCaml();
            this.sortAttributes(foreignElement);

            string foreignList = string.Empty;
            if (foreignElement.HasAttributes)
            {
                var listAttr = foreignElement.Attributes().FirstOrDefault(a => a.Name == Attributes.List);
                if (listAttr != null)
                {
                    foreignList = listAttr.Value;
                }
            }

            var result = new XElement(Tags.Join, new XAttribute(Attributes.ListAlias, foreignList), new XElement(Tags.Eq, primaryElement, foreignElement));
            return this.operationResultBuilder.CreateResult(result);
        }

        private void sortAttributes(XElement e)
        {
            if (!e.HasAttributes)
            {
                return;
            }
            var attributes = e.Attributes().ToList().OrderBy(a => a.Name.LocalName);
            e.RemoveAttributes();
            e.Add(attributes);
        }

        public override Expression ToExpression()
        {
            var op1 = this.fieldRefOperand as FieldRefOperand;
            if (op1 == null)
            {
                throw new NullReferenceException("fieldRefOperand is null");
            }

            var op2 = this.valueOperand as FieldRefOperand;
            if (op2 == null)
            {
                throw new NullReferenceException("valueOperand is null");
            }

            var attrs1 = op1.Attributes;
            if (attrs1 == null)
            {
                throw new NullReferenceException("fieldRefOperand.Attributes are null");
            }

            var attrs2 = op2.Attributes;
            if (attrs2 == null)
            {
                throw new NullReferenceException("valueOperand.Attributes are null");
            }

            var primaryListMethodInfo = ReflectionHelper.GetExtensionMethods(typeof(Camlex).Assembly, typeof(object)).FirstOrDefault(
                m => m.Name == ReflectionHelper.PrimaryListMethodName);
            var foreignListMethodInfo = ReflectionHelper.GetExtensionMethods(typeof(Camlex).Assembly, typeof(object)).FirstOrDefault(
                m => m.Name == ReflectionHelper.ForeignListMethodName);

            string fieldName = op1.FieldName;
            if (string.IsNullOrEmpty(fieldName))
            {
                throw new Exception("Name is empty");
            }

            if (!attrs2.Any(a => a.Key == Attributes.List))
            {
                throw new Exception("List attribute is not specified");
            }
            string foreignListName = attrs2.First(a => a.Key == Attributes.List).Value;
            if (string.IsNullOrEmpty(fieldName))
            {
                throw new Exception("List is empty");
            }

            string primaryListName = string.Empty;
            if (attrs1.Any(a => a.Key == Attributes.List))
            {
                primaryListName = attrs1.First(a => a.Key == Attributes.List).Value;
            }

            var listItemIndexerMethodInfo = typeof(SPListItem).GetProperty(ReflectionHelper.Item, typeof(object), new[] { typeof(string) }, null).GetGetMethod();
            var fieldRefExpr = Expression.Call(Expression.Parameter(typeof(SPListItem), ReflectionHelper.CommonParameterName),
                listItemIndexerMethodInfo, new[] { Expression.Constant(fieldName) });

            Expression ex = null;
            if (string.IsNullOrEmpty(primaryListName))
            {
                ex = Expression.Call(null, foreignListMethodInfo, fieldRefExpr, Expression.Constant(foreignListName));
            }
            else
            {
                var internalExpr = Expression.Call(null, primaryListMethodInfo, fieldRefExpr, Expression.Constant(primaryListName));
                ex = Expression.Call(null, foreignListMethodInfo, internalExpr, Expression.Constant(foreignListName));
            }
            return ex;
        }
    }
}


