#region Copyright(c) Alexey Sadomov, Vladimir Timashkov. All Rights Reserved.
// -----------------------------------------------------------------------------
// Copyright(c) 2007 Alexey Sadomov, Vladimir Timashkov. All Rights Reserved.
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
using System.Xml.Linq;
using CamlexNET.Interfaces;

namespace CamlexNET.Impl.Operands
{
    internal class BooleanValueOperand : ValueOperand<bool>
    {
        public BooleanValueOperand(bool value) :
            base(typeof(DataTypes.Boolean), value)
        {
        }

        public BooleanValueOperand(string value) :
            base(typeof(DataTypes.Boolean), false)
        {
            if (!bool.TryParse(value, out this.value))
            {
                // boolean operand can have 1 and 0 as parameter
                if (!this.tryConvertViaInteger(value, out this.value))
                {
                    throw new InvalidValueForOperandTypeException(value, this.Type);
                }
            }
        }

        private bool tryConvertViaInteger(string value, out bool result)
        {
            result = false;
            try
            {
                int val = Convert.ToInt32(value);
                if (val != 0 && val != 1)
                {
                    return false;
                }
                result = Convert.ToBoolean(val);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public override XElement ToCaml()
        {
            // 0 and 1 should be used instead True and False
            return
                new XElement(Tags.Value, new XAttribute(Attributes.Type, this.TypeName),
                    new XText((Convert.ToInt32(this.Value)).ToString()));
        }
    }
}


