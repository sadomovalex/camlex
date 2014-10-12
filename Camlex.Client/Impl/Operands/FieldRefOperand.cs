#region Copyright(c) Alexey Sadomov, Vladimir Timashkov, Stef Heyenrath. All Rights Reserved.
// -----------------------------------------------------------------------------
// Copyright(c) 2010 Alexey Sadomov, Vladimir Timashkov, Stef Heyenrath. All Rights Reserved.
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
using System.Linq.Expressions;
using System.Xml.Linq;
using CamlexNET.Interfaces;
using Microsoft.SharePoint.Client;

namespace CamlexNET.Impl.Operands
{
	internal class FieldRefOperand : IOperand
	{
		protected string fieldName;
		protected Guid? id;
		private readonly List<KeyValuePair<string, string>> attributes;
        private string tagName;

		public string FieldName
		{
			get
			{
				return this.fieldName;
			}
		}

		public Guid? FieldId
		{
			get
			{
				return this.id;
			}
		}

		public List<KeyValuePair<string, string>> Attributes
		{
			get
			{
				return this.attributes;
			}
            set
            {
                this.attributes = value;
            }
        }

        public string TagName
        {
            get
            {
                return tagName;
            }
            set
            {
                tagName = value;
            }
		}

		protected FieldRefOperand(List<KeyValuePair<string, string>> attributes)
		{
			this.attributes = attributes;
		}

		public FieldRefOperand(string fieldName)
		{
			this.initialize(fieldName);
		}

		public FieldRefOperand(string fieldName, List<KeyValuePair<string, string>> attributes) :
			this(attributes)
		{
			this.initialize(fieldName);
		}

		public FieldRefOperand(Guid id)
		{
			this.initialize(id);
		}

		public FieldRefOperand(Guid id, List<KeyValuePair<string, string>> attributes) :
			this(attributes)
		{
			this.initialize(id);
		}

		protected void initialize(Guid id)
		{
			this.id = id;
		}

		protected void initialize(string fieldName)
		{
			this.fieldName = fieldName;
		}

		public virtual XElement ToCaml()
		{
			XElement element;
            string tag = this.tagName ?? Tags.FieldRef;
			if (this.id != null)
			{
                element = new XElement(tag, new XAttribute(CamlexNET.Attributes.ID, this.id.Value));
			}
			else if (!string.IsNullOrEmpty(this.fieldName))
			{
                element = new XElement(tag, new XAttribute(CamlexNET.Attributes.Name, this.fieldName));
			}
			else
			{
				throw new FieldRefOperandShouldContainNameOrIdException();
			}

			if (this.attributes != null)
			{
				foreach (var attr in this.attributes)
				{
					// should not specify id or name twice
					if (string.Compare(attr.Key, CamlexNET.Attributes.ID, true) == 0 ||
						string.Compare(attr.Key, CamlexNET.Attributes.Name, true) == 0)
					{
						continue;
					}
					element.Add(new XAttribute(attr.Key, attr.Value));
				}
			}
			return element;
		}

		public virtual Expression ToExpression()
		{
			if (this.id != null)
			{
				var mi = typeof(ListItem).GetProperty(ReflectionHelper.Item, typeof(object), new[] { typeof(Guid) }, null).GetGetMethod();
				var guidConstructor = typeof(Guid).GetConstructor(new[] { typeof(string) });
				return
					Expression.Call(
						Expression.Parameter(typeof(ListItem), ReflectionHelper.CommonParameterName),
						mi, new[] { Expression.New(guidConstructor, Expression.Constant(this.id.Value.ToString())) });
			}
			else if (!string.IsNullOrEmpty(this.fieldName))
			{
				var mi = typeof(ListItem).GetProperty(ReflectionHelper.Item, typeof(object), new[] { typeof(string) }, null).GetGetMethod();
				return
					Expression.Call(
						Expression.Parameter(typeof(ListItem), ReflectionHelper.CommonParameterName),
						mi, new[] { Expression.Constant(this.fieldName) });
			}
			else
			{
				throw new FieldRefOperandShouldContainNameOrIdException();
			}
		}
	}
}


