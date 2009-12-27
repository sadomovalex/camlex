using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Camlex.NET.Impl;
using Camlex.NET.Interfaces;
using Microsoft.SharePoint;


namespace Camlex.NET
{
    public static class Camlex
    {
        public static ITranslatorFactory TranslatorFactory = new TranslatorFactory();

        public static string Where(Expression<Func<SPItem, bool>> expr)
        {
            var translator = TranslatorFactory.Create(expr.Body.NodeType);
            return translator.Translate(expr);
        }
    }
}
