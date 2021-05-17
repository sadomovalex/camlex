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
        public bool StorageTZ { get; set; }
        public int OffsetDays { get; set; }

        public DateTimeValueOperand(DateTime value, bool includeTimeValue) :
            base(typeof(DataTypes.DateTime), value)
        {
            IncludeTimeValue = includeTimeValue;
        }

        public DateTimeValueOperand(DateTime value, bool includeTimeValue, bool storageTZ) :
            base(typeof(DataTypes.DateTime), value)
        {
            IncludeTimeValue = includeTimeValue;
            StorageTZ = storageTZ;
        }

        public DateTimeValueOperand(string value, bool includeTimeValue) :
            this(value, includeTimeValue, false, 0)
        {
        }

        public DateTimeValueOperand(string value, bool includeTimeValue, bool storageTZ) :
            this(value, includeTimeValue, false, 0, storageTZ)
        {
        }

        public DateTimeValueOperand(string value, bool includeTimeValue, int offsetDays) :
            this(value, includeTimeValue, false, offsetDays)
        {
        }

        public DateTimeValueOperand(string value, bool includeTimeValue, int offsetDays, bool storageTZ) :
            this(value, includeTimeValue, false, offsetDays, storageTZ)
        {
        }

        public DateTimeValueOperand(string value, bool includeTimeValue, bool parseExact, int offsetDays) :
            this(value, includeTimeValue, parseExact, offsetDays, false)
        {

        }

        public DateTimeValueOperand(string value, bool includeTimeValue, bool parseExact, int offsetDays, bool storageTZ) :
            base(typeof(DataTypes.DateTime), DateTime.MinValue)
        {
            IncludeTimeValue = includeTimeValue;
            StorageTZ = storageTZ;
            OffsetDays = offsetDays;

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
            {
                dateTime = new XText(this.getStringFromDateTime(this.Value));
            }
            else if (Mode == DateTimeValueMode.Today && OffsetDays != 0)
            {
                dateTime = new XElement(Mode.ToString(),
                    new XAttribute(Attributes.OffsetDays, OffsetDays.ToString()));
            }
            else
            {
                dateTime = new XElement(Mode.ToString());
            }

            var attributes = new List<XAttribute>();
            attributes.Add(new XAttribute(Attributes.Type, TypeName));

            if (IncludeTimeValue)
            {
                attributes.Add(new XAttribute(Attributes.IncludeTimeValue, true.ToString()));
            }

            if (StorageTZ)
            {
                attributes.Add(new XAttribute(Attributes.StorageTZ, true.ToString()));
            }

            return new XElement(Tags.Value,
                attributes,
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
                    if (StorageTZ)
                    {
                        var mi =
                            ReflectionHelper.GetExtensionMethods(typeof(Camlex).Assembly, typeof(DateTime)).FirstOrDefault(
                                m => m.Name == ReflectionHelper.IncludeTimeValue && m.GetParameters().Count() == 2);

                        return Expression.Call(mi, expr, Expression.Constant(StorageTZ));
                    }
                    else
                    {
                        var mi =
                            ReflectionHelper.GetExtensionMethods(typeof(Camlex).Assembly, typeof(DateTime)).FirstOrDefault(
                                m => m.Name == ReflectionHelper.IncludeTimeValue && m.GetParameters().Count() == 1);

                        return Expression.Call(mi, expr);
                    }
                }
                else
                {
                    return expr;
                }
            }
            else
            {
                var val = this.getExpressionByMode(this.Mode);
                var expr = Expression.Convert(Expression.Convert(val, typeof(BaseFieldType)),
                    typeof(DataTypes.DateTime));
                if (Mode == DateTimeValueMode.Today && OffsetDays != 0)
                {
                    var mi =
                        typeof(DataTypes.DateTime).GetMethod(ReflectionHelper.OffsetDays);
                    return Expression.Call(expr, mi, Expression.Constant(OffsetDays));
                }
                else if (this.IncludeTimeValue)
                {
                    if (StorageTZ)
                    {
                        var mi =
                            typeof(DataTypes.DateTime).GetMethods().FirstOrDefault(
                                m => m.Name == ReflectionHelper.IncludeTimeValue && m.GetParameters().Count() == 1);

                        return Expression.Call(expr, mi, Expression.Constant(StorageTZ));
                    }
                    else
                    {
                        var mi =
                            typeof(DataTypes.DateTime).GetMethods().FirstOrDefault(
                                m => m.Name == ReflectionHelper.IncludeTimeValue && m.GetParameters().Count() == 0);

                        return Expression.Call(expr, mi);
                    }
                }
                return expr;
            }
        }

        private Expression getExpressionByMode(DateTimeValueMode mode)
        {
            switch (mode)
            {
                case DateTimeValueMode.Native:
                    return Expression.Constant(this.Value);
                case DateTimeValueMode.Now:
                    return this.getExpression(Camlex.Now);
                case DateTimeValueMode.Today:
                    return this.getExpression(Camlex.Today);
                case DateTimeValueMode.Week:
                    return this.getExpression(Camlex.Week);
                case DateTimeValueMode.Month:
                    return this.getExpression(Camlex.Month);
                case DateTimeValueMode.Year:
                    return this.getExpression(Camlex.Year);
                default:
                    throw new DateTimeOperandModeNotSupportedException(mode);
            }
        }

        private Expression getExpression(string name)
        {
            var mi = typeof (Camlex).GetMember(name)[0];
            return Expression.MakeMemberAccess(null, mi);
        }
    }
}
