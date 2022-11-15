﻿#region Copyright(c) Alexey Sadomov, Vladimir Timashkov. All Rights Reserved.
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
using CamlexNET.Impl.Factories;
using CamlexNET.Impl.Operands;
using CamlexNET.Interfaces;

namespace CamlexNET.Impl.Operations.Lt
{
    internal class LtOperation : BinaryOperationBase
    {
        public LtOperation(IOperationResultBuilder operationResultBuilder,
            IOperand fieldRefOperand, IOperand valueOperand)
            : base(operationResultBuilder, fieldRefOperand, valueOperand)
        {
        }

        public override IOperationResult ToResult()
        {
            var result = new XElement(Tags.Lt,
                             this.fieldRefOperand.ToCaml(),
                             this.valueOperand.ToCaml());
            return this.operationResultBuilder.CreateResult(result);
        }

        public override Expression ToExpression()
        {
            var fieldRef = this.getFieldRefOperandExpression();
            var value = this.getValueOperandExpression();

            if (!value.Type.IsSubclassOf(typeof(BaseFieldTypeWithOperators)) && this.valueOperand.GetType() != typeof(TextValueOperand))
            {
                return Expression.LessThan(fieldRef, value);
            }
            else if (this.valueOperand.GetType() == typeof(TextValueOperand))
            {
                var methodInfo = typeof(BaseFieldTypeWithOperators).GetMethod(ReflectionHelper.LessThanMethodName);

                var fieldRefExpr = this.fieldRefOperand.ToExpression();
                var convertedValue = Expression.Convert(value, typeof(BaseFieldType));
                return Expression.LessThan(fieldRefExpr, Expression.Convert(convertedValue, typeof(DataTypes.Text)), false, methodInfo);
            }
            else
            {
                var methodInfo = typeof(BaseFieldTypeWithOperators).GetMethod(ReflectionHelper.LessThanMethodName);
                return Expression.LessThan(fieldRef, value, false, methodInfo);
            }
        }
    }
}


