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
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Linq;
using CamlexNET.Impl.Operands;
using CamlexNET.Interfaces;

namespace CamlexNET.Impl.Operations.Includes
{
    internal class IncludesOperationBase : BinaryOperationBase
    {
        public IncludesOperationBase(IOperationResultBuilder operationResultBuilder,
            IOperand fieldRefOperand, IOperand valueOperand)
            : base(operationResultBuilder, fieldRefOperand, valueOperand)
        {
        }

        public override IOperationResult ToResult()
        {
            var result = new XElement(Tags.Includes,
                             fieldRefOperand.ToCaml(),
                             valueOperand.ToCaml());
            return operationResultBuilder.CreateResult(result);
        }

        public override Expression ToExpression()
        {
            // in the field ref operand we don't know what type of the value it has. So perform
            // conversion here
            var fieldRef = this.getFieldRefOperandExpression();
            var value = this.getValueOperandExpression();

            if (fieldRef.Type != typeof(object))
            {
                fieldRef = Expression.Convert(fieldRef, typeof(object));
            }

            if (value.Type != typeof(object))
            {
                value = Expression.Convert(value, typeof(object));
            }

            bool hasLookupId = false;
            List<KeyValuePair<string, string>> attrs = null;
            if (this.fieldRefOperand is FieldRefOperand)
            {
                attrs = (this.fieldRefOperand as FieldRefOperand).Attributes;
                if (attrs != null)
                {
                    hasLookupId = attrs.Any(a => a.Key == Attributes.LookupId);
                }
            }

            MethodInfo mi = null;
            if (hasLookupId)
            {
                mi = typeof(ExtensionMethods).GetMethod(ReflectionHelper.IncludesMethodName,
                    new[]
                    {
                        typeof(object), typeof(object), typeof(bool)
                    });
                return Expression.Call(mi, fieldRef, value, Expression.Constant(true));
            }
            else
            {
                mi = typeof(ExtensionMethods).GetMethod(ReflectionHelper.IncludesMethodName,
                    new[]
                    {
                        typeof(object), typeof(object)
                    });
                return Expression.Call(mi, fieldRef, value);
            }
        }
    }
}
