namespace CamlexNET
{
    internal static class Comparisons
    {
        internal static class Gt
        {
            public const string Name = Tags.Gt;
            public const string Symbol = ">";
            public const string Method = "op_GreaterThan";
        }

        internal static class Geq
        {
            public const string Name = Tags.Geq;
            public const string Symbol = ">=";
            public const string Method = "op_GreaterThanOrEqual";
        }

        internal static class Lt
        {
            public const string Name = Tags.Lt;
            public const string Symbol = "<";
            public const string Method = "op_LessThan";
        }

        internal static class Leq
        {
            public const string Name = Tags.Leq;
            public const string Symbol = "<=";
            public const string Method = "op_LessThanOrEqual";
        }
    }
}