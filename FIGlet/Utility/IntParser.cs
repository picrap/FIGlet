// A FIGlet generation library - MIT license
// https://github.com/picrap/FIGlet

namespace FIGlet.Utility
{
    using System;

    /// <summary>
    /// Simple methods to parse simple integers
    /// </summary>
    public static class IntParser
    {
        /// <summary>
        /// Tries to parse a literal number, which can be hexadecimal (starting with 0x), octal (starting with 0) or decimal.
        /// </summary>
        /// <param name="literal">The literal.</param>
        /// <returns></returns>
        public static int? TryParse(string literal)
        {
            if (string.IsNullOrEmpty(literal))
                return null;

            bool negate = false;
            if (literal.StartsWith("-"))
            {
                literal = literal.Substring(1);
                negate = true;
            }

            if (literal.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase))
                return Sign(TryParse(literal.Substring(2), 16), negate);
            if (literal.StartsWith("0"))
                return Sign(TryParse(literal.Substring(1), 8), negate);
            return Sign(TryParse(literal, 10), negate);
        }

        private static int? Sign(int? v, bool negate)
        {
            if (!v.HasValue)
                return null;
            if (negate)
                return -v;
            return v;
        }

        /// <summary>
        /// Gets a digit from a char, regardless the base.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <returns></returns>
        private static int? GetDigit(char c)
        {
            if (c >= '0' && c <= '9')
                return c - '0';
            if (c >= 'a' && c <= 'f')
                return c - 'a' + 10;
            if (c >= 'A' && c <= 'F')
                return c - 'A' + 10;
            return null;
        }

        /// <summary>
        /// Gets a digit, regarding the requested base.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <param name="decimalBase">The decimal base.</param>
        /// <returns></returns>
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