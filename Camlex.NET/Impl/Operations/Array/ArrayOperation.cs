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
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Xml.Linq;
using CamlexNET.Impl.Factories;
using CamlexNET.Impl.Operands;
using CamlexNET.Interfaces;

namespace CamlexNET.Impl.Operations.Array
{
    internal class ArrayOperation : OperationBase
    {
        private readonly IOperand[] fieldRefOperands;

        public int OperandsCount
        {
            get { return this.fieldRefOperands == null ? 0 : this.fieldRefOperands.Length; }
        }

        public ArrayOperation(IOperationResultBuilder operationResultBuilder,
            params IOperand[] fieldRefOperands) :
            base(operationResultBuilder)
        {
            this.fieldRefOperands = fieldRefOperands;
        }

        public override IOperationResult ToResult()
        {
            var results = new List<XElement>();
            System.Array.ForEach(this.fieldRefOperands, x => results.Add(x.ToCaml()));
            return this.operationResultBuilder.CreateResult(results.ToArray());
        }

        public override Expression ToExpression()
        {
            if (this.fieldRefOperands == null)
            {
                throw new NullReferenceException("fieldRefOperands");
            }
            if (this.fieldRefOperands.Any(x => x == null))
            {
                throw new NullReferenceException("fieldRefOperand");
            }
            if (this.fieldRefOperands.Any(x => !(x is FieldRefOperand || x is FieldRefOperandWithOrdering)))
            {
                throw new ArrayOperationShouldContainOnlyFieldRefOperandsException();
            }

            // if there is only 1 field ref operand - return single expression (not array)
            if (this.fieldRefOperands.Count() == 1)
            {
                return this.fieldRefOperands.First().ToExpression();
            }

            return Expression.NewArrayInit(typeof(object), this.fieldRefOperands.Select(o => o.ToExpression()));
        }
    }
}


