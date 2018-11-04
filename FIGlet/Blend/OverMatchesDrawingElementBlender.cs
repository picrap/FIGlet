// A FIGlet generation library - MIT license
// https://github.com/picrap/FIGlet

namespace FIGlet.Blend
{
    using System.Linq;
    using Drawing;

    /// <summary>
    /// Blends if the over glyph matches one of given elements (in which case it overwrites the background)
    /// </summary>
    /// <seealso cref="FIGlet.Blend.IDrawingElementBlender" />
    public class OverMatchesDrawingElementBlender : IDrawingElementBlender
    {
        private readonly char[] _matches;

        /// <summary>
        /// Initializes a new instance of the <see cref="OverMatchesDrawingElementBlender"/> class.
        /// </summary>
        /// <param name="matches">The matches.</param>
        public OverMatchesDrawingElementBlender(params char[] matches)
        {
            _matches = matches;
        }

        /// <inheritdoc />
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