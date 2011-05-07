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
using System.Reflection;
using System.Xml.Linq;
using CamlexNET.Impl.Operands;
using CamlexNET.Interfaces;

namespace CamlexNET.Impl.Operations.DateRangesOverlap
{
    internal class DateRangesOverlapOperation : OperationBase
    {
        protected IOperand startFieldRefOperand;
        protected IOperand stopFieldRefOperand;
        protected IOperand recurrenceFieldRefOperand;
        protected IOperand dateTimeOperand;

        public DateRangesOverlapOperation(IOperationResultBuilder operationResultBuilder,
            IOperand startFieldRefOperand, IOperand stopFieldRefOperand, IOperand recurrenceFieldRefOperand, IOperand dateTimeOperand) :
            base(operationResultBuilder)
        {
            this.startFieldRefOperand = startFieldRefOperand;
            this.stopFieldRefOperand = stopFieldRefOperand;
            this.recurrenceFieldRefOperand = recurrenceFieldRefOperand;
            this.dateTimeOperand = dateTimeOperand;
        }

        public override IOperationResult ToResult()
        {
            var result = new XElement(Tags.DateRangesOverlap,
                             startFieldRefOperand.ToCaml(),
                             stopFieldRefOperand.ToCaml(),
                             recurrenceFieldRefOperand.ToCaml(),
                             dateTimeOperand.ToCaml());
            return operationResultBuilder.CreateResult(result);
        }

        public override Expression ToExpression()
        {
            if (this.startFieldRefOperand == null)
            {
                throw new NullReferenceException("startFieldRefOperand");
            }
            if (this.stopFieldRefOperand == null)
            {
                throw new NullReferenceException("stopFieldRefOperand");
            }
            if (this.recurrenceFieldRefOperand == null)
            {
                throw new NullReferenceException("recurrenceFieldRefOperand");
            }
            if (this.dateTimeOperand == null)
            {
                throw new NullReferenceException("dateTimeOperand");
            }
            if (!(this.dateTimeOperand is DateTimeValueOperand))
            {
                throw new DateTimeValueOperandExpectedException();
            }

            var dt = this.dateTimeOperand as DateTimeValueOperand;

            MethodInfo mi = null;

            if (dt.Mode == DateTimeValueOperand.DateTimeValueMode.Native)
            {
                mi = typeof (Camlex).GetMethod(ReflectionHelper.DateRangesOverlap,
                                               new[]
                                                   {
                                                       typeof (object), typeof (object), typeof (object), typeof (DateTime)
                                                   });
            }
            else
            {
                mi = typeof(Camlex).GetMethod(ReflectionHelper.DateRangesOverlap,
                                               new[]
                                                   {
                                                       typeof (object), typeof (object), typeof (object), typeof (DataTypes.DateTime)
                                                   });
            }
            return Expression.Call(mi, this.startFieldRefOperand.ToExpression(), this.stopFieldRefOperand.ToExpression(),
                                   this.recurrenceFieldRefOperand.ToExpression(), this.dateTimeOperand.ToExpression());
        }
    }
}

