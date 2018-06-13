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

using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Linq;
using CamlexNET.Impl.Operands;
using CamlexNET.Interfaces;

namespace CamlexNET.Impl.Operations.In
{
    internal class InOperation : BinaryOperationBase
    {
        public InOperation(IOperationResultBuilder operationResultBuilder,
            IOperand fieldRefOperand, IOperand valueOperand)
            : base(operationResultBuilder, fieldRefOperand, valueOperand)
        {
        }

        public override IOperationResult ToResult()
        {
            var result = new XElement(Tags.In,
                             this.fieldRefOperand.ToCaml(),
                             this.valueOperand.ToCaml());
            return this.operationResultBuilder.CreateResult(result);
        }

        public override Expression ToExpression()
        {
            if (!(this.fieldRefOperand is FieldRefOperand))
            {
                throw new OperationShouldContainFieldRefOperandException();
            }
            if (!(this.valueOperand is ValuesValueOperand))
            {
                throw new OperationShouldContainValuesValueOperandException();
            }
            var valueOperandType = ((ValuesValueOperand)this.valueOperand).GetOperandsType();
            var fieldRefExpr = this.getFieldRefOperandExpression(valueOperandType);
            var valueExpr = this.getValueOperandExpression();
            //var mi = valueOperandType.MakeArrayType().GetMethod(ReflectionHelper.ContainsMethodName, BindingFlags.Public | BindingFlags.Static);
            var mi = typeof(System.Linq.Enumerable).GetMethods(BindingFlags.Static | BindingFlags.Public)
                .Where(m => m.Name == ReflectionHelper.ContainsMethodName && m.GetParameters().Count() == 2).First();
            return Expression.Call(null, mi.MakeGenericMethod(valueOperandType), valueExpr, fieldRefExpr);
        }
    }
}


