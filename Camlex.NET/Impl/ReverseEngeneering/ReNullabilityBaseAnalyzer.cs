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
using System.Xml.Linq;
using CamlexNET.Impl.Operands;
using CamlexNET.Interfaces;
using CamlexNET.Interfaces.ReverseEngeneering;

namespace CamlexNET.Impl.ReverseEngeneering
{
    internal abstract class ReNullabilityBaseAnalyzer : ReBinaryExpressionBaseAnalyzer
    {
        protected ReNullabilityBaseAnalyzer(XElement el, IReOperandBuilder operandBuilder)
            : base(el, operandBuilder)
        {
        }

        public override bool IsValid()
        {
            if (this.el == null)
            {
                return false;
            }

            if (el.Elements().Count() != 1)
            {
                return false;
            }

            if (!this.hasValidFieldRefElement())
            {
                return false;
            }
            return true;
        }

        protected IOperation getNullabilityOperation<T>(Func<IOperationResultBuilder, IOperand, T> constructor)
            where T : IOperation
        {
            if (!IsValid())
                throw new CamlAnalysisException(string.Format(
                    "Can't create {0} from the following xml: {1}", typeof(T).Name, el));

            var fieldRefElement = this.el.Elements(Tags.FieldRef).First();
            var fieldRefOperand = this.operandBuilder.CreateFieldRefOperand(fieldRefElement);
            return constructor(null, fieldRefOperand);
        }
    }
}


