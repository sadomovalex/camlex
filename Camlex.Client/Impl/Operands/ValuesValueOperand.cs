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
using CamlexNET.Interfaces;

namespace CamlexNET.Impl.Operands
{
    // Values operand is used for In operator
    internal class ValuesValueOperand : ValueOperand<IEnumerable<IOperand>>
    {
        public ValuesValueOperand(IEnumerable<IOperand> values)
            : base(typeof(IEnumerable<IOperand>), values)
        {
            if (values == null || !values.Any())
            {
                throw new CantCreateValuesValueOperandException("Can't create Values operand: list of values is null or empty");
            }
        }

        public override XElement ToCaml()
        {
            var el = new XElement(Tags.Values);
            foreach (var operand in this.Value)
            {
                el.Add(operand.ToCaml());
            }
            return el;
        }

        public override Expression ToExpression()
        {
            var type = this.GetOperandsType();
            return Expression.NewArrayInit(type, this.Value.Select(v => v.ToExpression()));
        }

        public Type GetOperandsType()
        {
            var types = this.Value.Select(getOperandsType).ToList();
            if (types == null || !types.Any())
            {
                throw new CantDetermineValueTypeException("Can't determine type of values: array of values is null or empty");
            }
            if (types.Distinct().Count() != 1)
            {
                throw new CantDetermineValueTypeException("Can't determine type of values: all values should have the same type, while in provided array they have different types");
            }
            return types[0];
        }

        private Type getOperandsType(IOperand operand)
        {
            //var operand = this.Value.ElementAt(0);
            if (typeof (ValueOperand<>).IsAssignableFrom(operand.GetType()))
            {
                throw new CantCreateExpressionForValuesValueOperandException();
            }
            var baseType = operand.GetType().BaseType;
            if (baseType == null || !baseType.IsGenericType)
            {
                throw new CantCreateExpressionForValuesValueOperandException();
            }

            var genericParams = baseType.GetGenericArguments();
            if (genericParams.Length != 1)
            {
                throw new CantCreateExpressionForValuesValueOperandException();
            }

            return genericParams[0];
        }
    }
}
