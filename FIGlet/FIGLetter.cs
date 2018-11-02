// A FIGlet generation library - MIT license
// https://github.com/picrap/FIGlet

namespace FIGlet
{
    using System.Collections.Generic;

    public class FIGLetter
    {
        public char Code { get; }
        public ICollection<string> Lines { get; }

        public FIGLetter(char code, ICollection<string> lines)
        {
            Code = code;
            Lines = lines;
        }
    }
}