// A FIGlet generation library - MIT license
// https://github.com/picrap/FIGlet

namespace FIGlet.Blend
{
    using Drawing;

    /// <summary>
    /// Specific composition over + under -> replace
    /// </summary>
    /// <seealso cref="FIGlet.Blend.IDrawingElementBlender" />
    public class PairDrawingElementBlender : IDrawingElementBlender
    {
        private readonly char _under;
        private readonly char _over;
        private readonly char _replace;

        /// <summary>
        /// Initializes a new instance of the <see cref="PairDrawingElementBlender"/> class.
        /// </summary>
        /// <param name="under">The under.</param>
        /// <param name="over">The over.</param>
        /// <param name="replace">The replace.</param>
        public PairDrawingElementBlender(char under, char over, char replace)
        {
            _under = under;
            _over = over;
            _replace = replace;
        }

        /// <inheritdoc />
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