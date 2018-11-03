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

        public SpecificOverrideDrawingElementBlender(bool overrideEmpty, params char[] overridableGlyphs)
        {
            _overrideEmpty = overrideEmpty;
            _overridableGlyphs = overridableGlyphs;
        }

        public SpecificOverrideDrawingElementBlender()
            : this(true, ' ')
        {
        }

        public DrawingElement TryBlend(DrawingElement under, DrawingElement over)
        {
            if (_overrideEmpty && under is null)
                return over;
            if (_overridableGlyphs.Contains(under.Character))
                return over;
            return null;
        }
    }
}
