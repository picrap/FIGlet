// A FIGlet generation library - MIT license
// https://github.com/picrap/FIGlet

namespace FIGlet.Blend
{
    using Drawing;

    public class PairDrawingElementBlender : IDrawingElementBlender
    {
        private readonly char _under;
        private readonly char _over;
        private readonly char _replace;

        public PairDrawingElementBlender(char under, char over, char replace)
        {
            _under = under;
            _over = over;
            _replace = replace;
        }

        public DrawingElement TryBlend(DrawingElement under, DrawingElement over)
        {
            if (under is null)
                return null;

            if (under.Glyph == _under && over.Glyph == _over)
                return over.ReplaceGlyph(_replace);

            return null;
        }
    }
}