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
using System.Text;
using CamlexNET;
using CamlexNET.Impl.Operands;
using CamlexNET.UnitTests.Helpers;
using NUnit.Framework;

namespace CamlexNET.UnitTests.Operands
{
    [TestFixture]
    public class DateTimeValueOperandTests
    {
        [Test]
        public void test_THAT_datetime_value_with_includetimevalue_IS_rendered_to_caml_properly()
        {
            var dateTime = DateTime.Now;
            var operand = new DateTimeValueOperand(dateTime, true);
            var caml = operand.ToCaml().ToString();
            Assert.That(caml, Is.EqualTo("<Value Type=\"DateTime\" IncludeTimeValue=\"True\">" + dateTime.ToString("s") + "Z</Value>").Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_datetime_value_without_includetimevalue_IS_rendered_to_caml_properly()
        {
            var dateTime = DateTime.Now;
            var operand = new DateTimeValueOperand(dateTime, false);
            var caml = operand.ToCaml().ToString();
            Assert.That(caml, Is.EqualTo("<Value Type=\"DateTime\">" + dateTime.ToString("s") + "Z</Value>").Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_datetime_value_with_storagetz_IS_rendered_to_caml_properly()
        {
            var dateTime = DateTime.Now;
            var operand = new DateTimeValueOperand(dateTime, false, true);
            var caml = operand.ToCaml().ToString();
            Assert.That(caml, Is.EqualTo("<Value Type=\"DateTime\" StorageTZ=\"True\">" + dateTime.ToString("s") + "Z</Value>").Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_datetime_value_without_storagetz_IS_rendered_to_caml_properly()
        {
            var dateTime = DateTime.Now;
            var operand = new DateTimeValueOperand(dateTime, false, false);
            var caml = operand.ToCaml().ToString();
            Assert.That(caml, Is.EqualTo("<Value Type=\"DateTime\">" + dateTime.ToString("s") + "Z</Value>").Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_datetime_value_with_includetimevalue_and_storagetz_IS_rendered_to_caml_properly()
        {
            var dateTime = DateTime.Now;
            var operand = new DateTimeValueOperand(dateTime, true, true);
            var caml = operand.ToCaml().ToString();
            Assert.That(caml, Is.EqualTo("<Value Type=\"DateTime\" IncludeTimeValue=\"True\" StorageTZ=\"True\">" + dateTime.ToString("s") + "Z</Value>").Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_datetime_value_IS_successfully_created_from_valid_string()
        {
            var operand = new DateTimeValueOperand("02.01.2010 03:04:05", true);
            var expected = DateTime.Parse("02.01.2010 03:04:05");
            Assert.That(operand.Value, Is.EqualTo(expected));
        }

        [Test]
        public void test_THAT_datetime_value_of_now_IS_rendered_to_caml_properly()
        {
            var operand = new DateTimeValueOperand(Camlex.Now, false);
            var caml = operand.ToCaml().ToString();
            Assert.That(caml, Is.EqualTo("<Value Type=\"DateTime\"><Now /></Value>").Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_datetime_value_of_today_IS_rendered_to_caml_properly()
        {
            var operand = new DateTimeValueOperand(Camlex.Today, false);
            var caml = operand.ToCaml().ToString();
            Assert.That(caml, Is.EqualTo("<Value Type=\"DateTime\"><Today /></Value>").Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_datetime_value_of_today_and_offsetdays_IS_rendered_to_caml_properly()
        {
            int offsetDays = 4;
            var operand = new DateTimeValueOperand(Camlex.Today, false, offsetDays);
            var caml = operand.ToCaml().ToString();
            Assert.That(caml, Is.EqualTo("<Value Type=\"DateTime\"><Today OffsetDays=\"" + offsetDays + "\" /></Value>").Using(new CamlComparer()));
        }

    [Test]
        public void test_THAT_datetime_value_of_week_IS_rendered_to_caml_properly()
        {
            var operand = new DateTimeValueOperand(Camlex.Week, false);
            var caml = operand.ToCaml().ToString();
            Assert.That(caml, Is.EqualTo("<Value Type=\"DateTime\"><Week /></Value>").Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_datetime_value_of_month_IS_rendered_to_caml_properly()
        {
            var operand = new DateTimeValueOperand(Camlex.Month, false);
            var caml = operand.ToCaml().ToString();
            Assert.That(caml, Is.EqualTo("<Value Type=\"DateTime\"><Month /></Value>").Using(new CamlComparer()));
        }

        [Test]
        public void test_THAT_datetime_value_of_year_IS_rendered_to_caml_properly()
        {
            var operand = new DateTimeValueOperand(Camlex.Year, false);
            var caml = operand.ToCaml().ToString();
            Assert.That(caml, Is.EqualTo("<Value Type=\"DateTime\"><Year /></Value>").Using(new CamlComparer()));
        }

        [Test]
        [ExpectedException(typeof(InvalidValueForOperandTypeException))]
        public void test_WHEN_datetime_value_is_not_valid_datetime_THEN_exception_is_thrown()
        {
            var operand = new DateTimeValueOperand("abc", true);
        }
    }
}
