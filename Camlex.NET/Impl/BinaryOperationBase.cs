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
using System.Text;
using System.Xml.Linq;
using CamlexNET.Impl.Helpers;
using CamlexNET.Impl.Operands;
using CamlexNET.Interfaces;

namespace CamlexNET.Impl
{
    internal abstract class BinaryOperationBase : OperationBase
    {
        protected IOperand fieldRefOperand;
        protected IOperand valueOperand;

        protected BinaryOperationBase(IOperationResultBuilder operationResultBuilder,
            IOperand fieldRefOperand, IOperand valueOperand) :
            base(operationResultBuilder)
        {
            this.fieldRefOperand = fieldRefOperand;
            this.valueOperand = valueOperand;
        }

        protected virtual Expression getFieldRefOperandExpression()
        {
            if (this.fieldRefOperand == null)
            {
                throw new NullReferenceException("fieldRefOperand");
            }
            if (this.valueOperand == null)
            {
                throw new NullReferenceException("valueOperand");
            }
            var valueOperandType = this.getValueOperandType();
            return this.getFieldRefOperandExpression(valueOperandType);
        }

        // Here we know both FieldRef operand and Value operand. So in those cases when we need knowledge from FieldRef for Value,
        // or from Value to FieldRef (e.g. for native syntax we need to know the type of the Value operand in order to perform casting for FieldRef)
        protected virtual Expression getFieldRefOperandExpression(Type valueOperandType)
        {
            if (this.fieldRefOperand == null)
            {
                throw new NullReferenceException("fieldRefOperand");
            }
            if (this.valueOperand == null)
            {
                throw new NullReferenceException("valueOperand");
            }

            var fieldRefOperandExpr = this.fieldRefOperand.ToExpression();
            if (valueOperandType == null)
            {
                return fieldRefOperandExpr;
            }
            return Expression.Convert(fieldRefOperandExpr, valueOperandType);
        }

        protected virtual Type getValueOperandType()
        {
            var valueOperandExpr = this.valueOperand.ToExpression();
            if (valueOperandExpr is ConstantExpression)
            {
                return ((ConstantExpression)valueOperandExpr).Value.GetType();
            }
            else if (valueOperandExpr is NewExpression)
            {
                return valueOperandExpr.Type;
            }
            else if (valueOperandExpr is MethodCallExpression)
            {
                // special case for DateTimeValueOperand - we should cast left value to the DateTime only if rvalue is native
                if (valueOperand is DateTimeValueOperand && ((DateTimeValueOperand)valueOperand).Mode == DateTimeValueOperand.DateTimeValueMode.Native)
                {
                    return ((MethodCallExpression)valueOperandExpr).Method.ReturnType;
                }
            }
            return null;
        }

        protected virtual Expression getValueOperandExpression()
        {
            if (this.fieldRefOperand == null)
            {
                throw new NullReferenceException("fieldRefOperand");
            }
            if (this.valueOperand == null)
            {
                throw new NullReferenceException("valueOperand");
            }

            return this.valueOperand.ToExpression();
        }
    }
}
