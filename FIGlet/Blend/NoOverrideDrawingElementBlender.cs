// A FIGlet generation library - MIT license
// https://github.com/picrap/FIGlet

namespace FIGlet.Blend
{
    using Drawing;

    /// <summary>
    /// This implementation of <see cref="IDrawingElementBlender"/> won't allow any override
    /// </summary>
    /// <seealso cref="FIGlet.Blend.IDrawingElementBlender" />
    public class NoOverrideDrawingElementBlender : IDrawingElementBlender
    {
        public DrawingElement TryBlend(DrawingElement under, DrawingElement over)
        {
            if (!(under is null))
                return null;
            return over;
        }
    }
}