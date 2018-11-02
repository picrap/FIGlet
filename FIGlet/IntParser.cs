// A FIGlet generation library - MIT license
// https://github.com/picrap/FIGlet

namespace FIGlet
{
    using System;
    using System.Runtime.InteropServices;

    public static class IntParser
    {
        public static int? TryParse(string literal)
        {
            if (string.IsNullOrEmpty(literal))
                return null;

            if (literal.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase))
                return TryParse(literal.Substring(2), 16);
            if (literal.StartsWith("0"))
                return TryParse(literal.Substring(1), 8);
            return TryParse(literal, 10);
        }

        private static int? GetDigit(char c)
        {
            if (c >= '0' && c < '9')
                return c - '0';
            if (c >= 'a' && c <= 'f')
                return c - 'a' + 10;
            if (c >= 'A' && c <= 'F')
                return c - 'A' + 10;
            return null;
        }

        private static int? GetDigit(char c, int decimalBase)
        {
            var v = GetDigit(c);
            if (!v.HasValue)
                return null;
            if (v < decimalBase)
                return v;
            return null;
        }

        private static int? TryParse(string literal, int decimalBase)
        {
            var r = 0;
            foreach (var c in literal)
            {
                var v = GetDigit(c, decimalBase);
                if (!v.HasValue)
                    return null;
                r = r * decimalBase + v.Value;
            }

            return r;
        }
    }
}