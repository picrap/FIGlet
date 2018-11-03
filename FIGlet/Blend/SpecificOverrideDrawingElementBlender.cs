// A FIGlet generation library - MIT license
// https://github.com/picrap/FIGlet

namespace FIGlet.Blend
{
    using System.Linq;
    using Drawing;

    /// <summary>
    /// Allows to override on specific glyphs
    /// </summary>
    /// <seealso cref="FIGlet.Blend.IDrawingElementBlender" />
    public class SpecificOverrideDrawingElementBlender : IDrawingElementBlender
    {
        private readonly bool _overrideEmpty;
        private readonly char[] _overridableGlyphs;
        private readonly char[] _overridingGlyphs;

        public SpecificOverrideDrawingElementBlender(bool overrideEmpty, char[] overridableGlyphs, char[] overridingGlyphs = null)
        {
            _overrideEmpty = overrideEmpty;
            _overridableGlyphs = overridableGlyphs;
            _overridingGlyphs = overridingGlyphs;
        }

        public DrawingElement TryBlend(DrawingElement under, DrawingElement over)
        {
            if (under is null)
            {
                if (_overrideEmpty)
                    return GetOverridingGlyph(over);
                return null;
            }
            if (_overridableGlyphs.Contains(under.Glyph))
                return GetOverridingGlyph(over);
            return null;
        }

        private DrawingElement GetOverridingGlyph(DrawingElement over)
        {
            if (_overridingGlyphs == null || over is null)
                return over;
            if (_overridingGlyphs.Contains(over.Glyph))
                return over;
            return null;
        }
    }
}
