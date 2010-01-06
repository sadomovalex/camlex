using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Xml.Linq;
using Camlex.NET.Impl;
using Camlex.NET.Impl.Factories;
using Camlex.NET.Interfaces;
using Microsoft.SharePoint;

namespace Camlex.NET
{
    public class Camlex
    {
        #region OrderBy functionality
        /// <summary>Marker class representing ASC order direction for "OrderBy" functionality</summary>
        public class OrderDirection
        {
            public static OrderDirection Default { get { return new Asc(); } }
            public static OrderDirection Convert(Type type)
            {
                return type == typeof (Asc) ? (OrderDirection) new Asc() : new Desc();
            }
        }
        /// <summary>Marker class representing ASC order direction for "OrderBy" functionality</summary>
        public class Asc : OrderDirection { }
        /// <summary>Marker class representing DESC order direction for "OrderBy" functionality</summary>
        public class Desc : OrderDirection { }
        #endregion

        private static ITranslatorFactory translatorFactory;

        static Camlex()
        {
            // factories setup
            var operandBuilder = new OperandBuilder();
            var operationResultBuilder = new OperationResultBuilder();
            var analyzerFactory = new AnalyzerFactory(operandBuilder, operationResultBuilder);
            translatorFactory = new TranslatorFactory(analyzerFactory);
        }

        public static IQuery Query()
        {
            return new Query(translatorFactory);
        }
    }
}
