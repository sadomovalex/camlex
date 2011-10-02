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
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace CamlexNET.Impl.Operands
{
    internal class DateTimeValueOperand : ValueOperand<DateTime>
    {
        internal enum DateTimeValueMode
        {
            Native, Now, Today, Week, Month, Year
        }

        public DateTimeValueMode Mode { get; set; }
        public bool IncludeTimeValue { get; set; }

        public DateTimeValueOperand(DateTime value, bool includeTimeValue) :
            base(typeof(DataTypes.DateTime), value)
        {
            IncludeTimeValue = includeTimeValue;
        }

        public DateTimeValueOperand(string value, bool includeTimeValue) :
            this(value, includeTimeValue, false)
        {
        }

        public DateTimeValueOperand(string value, bool includeTimeValue, bool parseExact) :
            base(typeof(DataTypes.DateTime), DateTime.MinValue)
        {
            IncludeTimeValue = includeTimeValue;

            if (value == Camlex.Now) Mode = DateTimeValueMode.Now;
            else if (value == Camlex.Today) Mode = DateTimeValueMode.Today;
            else if (value == Camlex.Week) Mode = DateTimeValueMode.Week;
            else if (value == Camlex.Month) Mode = DateTimeValueMode.Month;
            else if (value == Camlex.Year) Mode = DateTimeValueMode.Year;
            else if (!parseExact && DateTime.TryParse(value, out this.value))
            {
                Mode = DateTimeValueMode.Native;
            }
            // from re value come in sortable format ("s"), so we need to use ParseExact instead of Parse
            else if (parseExact)
            {
                if (!string.IsNullOrEmpty(value) && value.EndsWith("Z"))
                {
                    value = value.Substring(0, value.Length - 1);
                }

                if (DateTime.TryParseExact(value, "s", null, DateTimeStyles.None, out this.value))
                {
                    Mode = DateTimeValueMode.Native;
                }
                else throw new InvalidValueForOperandTypeException(value, Type);
            }
            else throw new InvalidValueForOperandTypeException(value, Type);
        }


        public override XElement ToCaml()
        {
            object dateTime;
            if (Mode == DateTimeValueMode.Native)
                dateTime = new XText(this.getStringFromDateTime(this.Value));
            else dateTime = new XElement(Mode.ToString());
            
            if (IncludeTimeValue)
            {
                return new XElement(Tags.Value,
                                    new XAttribute(Attributes.Type, TypeName),
                                    new XAttribute(Attributes.IncludeTimeValue, true.ToString()),
                                    dateTime);
            }
            return new XElement(Tags.Value,
                                new XAttribute(Attributes.Type, TypeName),
                                dateTime);
        }

        private string getStringFromDateTime(DateTime dt)
        {
            return (dt.ToString("s") + "Z");
        }

        public override Expression ToExpression()
        {
            if (this.Mode == DateTimeValueMode.Native)
            {
                var expr = Expression.Constant(this.Value);
                if (this.IncludeTimeValue)
                {
                    var mi =
                        ReflectionHelper.GetExtensionMethods(typeof(Camlex).Assembly, typeof(DateTime)).FirstOrDefault(
                            m => m.Name == ReflectionHelper.IncludeTimeValue);
                    return Expression.Call(mi, expr);
                }
                else
                {
                    return expr;
                }
            }
            else
            {
                string val = getValueByMode(this.Mode);
                var expr = Expression.Convert(Expression.Convert(Expression.Constant(val), typeof(BaseFieldType)),
                                          typeof(DataTypes.DateTime));
                if (this.IncludeTimeValue)
                {
                    var mi =
                        typeof(DataTypes.DateTime).GetMethod(ReflectionHelper.IncludeTimeValue);
                    return Expression.Call(expr, mi);
                }
                else
                {
                    return expr;
                }
            }
        }

        private string getValueByMode(DateTimeValueMode mode)
        {
            switch (mode)
            {
                case DateTimeValueMode.Native:
                    return this.getStringFromDateTime(this.Value);
                case DateTimeValueMode.Now:
                    return Camlex.Now;
                case DateTimeValueMode.Today:
                    return Camlex.Today;
                case DateTimeValueMode.Week:
                    return Camlex.Week;
                case DateTimeValueMode.Month:
                    return Camlex.Month;
                case DateTimeValueMode.Year:
                    return Camlex.Year;
                default:
                    throw new DateTimeOperandModeNotSupportedException(mode);
            }
        }
    }
}
