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
        private IEnumerable<IOperand> values;
        public ValuesValueOperand(IEnumerable<IOperand> values)
            : base(typeof(IEnumerable<IOperand>), values)
        {
            if (values == null || !values.Any())
            {
                throw new CantCreateValuesValueOperandException("Can't create Values operand: list of values is null or empty");
            }
            this.values = values;
        }

        public override XElement ToCaml()
        {
            var el = new XElement(Tags.Values);
            foreach (var operand in this.values)
            {
                el.Add(operand.ToCaml());
            }
            return el;
        }

        public override Expression ToExpression()
        {
            throw new NotImplementedException();
        }
    }
}
