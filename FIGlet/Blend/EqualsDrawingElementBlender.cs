// A FIGlet generation library - MIT license
// https://github.com/picrap/FIGlet

namespace FIGlet.Blend
{
    using System.Linq;
    using Drawing;

    /// <summary>
    /// Blends two characters when they are equal
    /// </summary>
    /// <seealso cref="FIGlet.Blend.IDrawingElementBlender" />
    public class EqualsDrawingElementBlender : IDrawingElementBlender
    {
        private readonly char[] _except;

        public EqualsDrawingElementBlender(char[] except)
        {
            _except = except;
        }

        public DrawingElement TryBlend(DrawingElement under, DrawingElement over)
        {
            if (_except != null && _except.Contains(over.Glyph))
                return null;
            if (under?.Glyph == over.Glyph)
                return over;
            return null;
        }
    }
}