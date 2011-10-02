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
using System.Xml;
using System.Xml.Linq;
using CamlexNET.Impl.Operands;
using CamlexNET.Impl.Operations.Array;
using CamlexNET.Impl.Operations.Eq;
using CamlexNET.Interfaces;
using CamlexNET.Interfaces.ReverseEngeneering;

namespace CamlexNET.Impl.ReverseEngeneering.Caml.Analyzers
{
    internal class ReEqAnalyzer : ReBaseAnalyzer
    {
        public ReEqAnalyzer(XElement el, IReOperandBuilder operandBuilder) :
            base(el, operandBuilder)
        {
        }

        public override bool IsValid()
        {
            if (!base.IsValid()) return false;

            // check presence of FieldRef tag with ID or Name attribute
            if (this.el.Descendants(Tags.FieldRef).Count() != 1) return false;
            var fieldRefElement = this.el.Descendants(Tags.FieldRef).First();
            var isIdOrNamePresent = fieldRefElement.Attributes()
                .Any(a => a.Name == Attributes.ID || a.Name == Attributes.Name);
            if (!isIdOrNamePresent) return false;

            // check presence of Value tag with Type attribute
            if (this.el.Descendants(Tags.Value).Count() != 1) return false;
            var valueElement = this.el.Descendants(Tags.Value).First();
            var typeAttribute = valueElement.Attributes()
                .Where(a => a.Name == Attributes.Type).FirstOrDefault();
            if (typeAttribute == null) return false;

            // check whether we support this value type
            if (typeAttribute.Value != typeof(DataTypes.Text).Name &&
                string.IsNullOrEmpty(valueElement.Value)) return false;
            var value = valueElement.Value;
            try
            {
                if (typeAttribute.Value == typeof(DataTypes.Boolean).Name) new BooleanValueOperand(value);
                else if (typeAttribute.Value == typeof(DataTypes.DateTime).Name) new DateTimeValueOperand(value, false);
                else if (typeAttribute.Value == typeof(DataTypes.Guid).Name) new GuidValueOperand(value);
                else if (typeAttribute.Value == typeof(DataTypes.Integer).Name) new IntegerValueOperand(value);
                else if (typeAttribute.Value == typeof(DataTypes.Lookup).Name) new LookupValueValueOperand(value);
                else if (typeAttribute.Value == typeof(DataTypes.Text).Name) { }
                else throw new InvalidValueForOperandTypeException(null, null);
            }
            catch (InvalidValueForOperandTypeException) { return false; }

            return true;
        }

        public override IOperation GetOperation()
        {
            if (!IsValid())
                throw new CamlAnalysisException(string.Format("Xml element '{0}' is not supported", el));

            var operand = this.operandBuilder.CreateFieldRefOperand(el);
            var value = this.operandBuilder.CreateValueOperand(el);
            return new EqOperation(null, operand, value);
        }
    }
}