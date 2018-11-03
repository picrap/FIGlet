// A FIGlet generation library - MIT license
// https://github.com/picrap/FIGlet

namespace FIGlet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class FIGcharacter
    {
        public char Code { get; }

        public IList<string> Lines { get; }

        public int Width { get; }
        public int Height => Lines.Count;

        public char this[int x, int y]
        {
            get
            {
                var line = Lines[y];
                if (x < line.Length)
                    return line[x];
                return '\0';
            }
        }

        public FIGcharacter(char code, IList<string> lines)
        {
            Code = code;
            Lines = lines;
            Width = lines.Aggregate(0, (w, l) => Math.Max(w, l.Length));
        }
    }
}