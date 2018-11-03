// A FIGlet generation library - MIT license
// https://github.com/picrap/FIGlet

namespace FIGlet.Blend
{
    using System.Linq;
    using Drawing;

    public class OverMatchesDrawingElementBlender : IDrawingElementBlender
    {
        private readonly char[] _matches;

        public OverMatchesDrawingElementBlender(params char[] matches)
        {
            _matches = matches;
        }

        public DrawingElement TryBlend(DrawingElement under, DrawingElement over)
        {
            if (over is null)
                return null;
            if (_matches.Contains(over.Glyph))
                return over;
            return null;
        }
    }
}