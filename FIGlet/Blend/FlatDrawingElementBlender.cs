// A FIGlet generation library - MIT license
// https://github.com/picrap/FIGlet

namespace FIGlet.Blend
{
    using Drawing;

    /// <summary>
    ///     An implementation of <see cref="IDrawingElementBlender" /> which does not consider over/under and tries both
    /// </summary>
    /// <seealso cref="FIGlet.Blend.IDrawingElementBlender" />
    public class FlatDrawingElementBlender : IDrawingElementBlender
    {
        private readonly IDrawingElementBlender _innerDrawingElementBlender;

        public FlatDrawingElementBlender(IDrawingElementBlender innerDrawingElementBlender)
        {
            _innerDrawingElementBlender = innerDrawingElementBlender;
        }

        public DrawingElement TryBlend(DrawingElement under, DrawingElement over)
        {
            return _innerDrawingElementBlender.TryBlend(under, over) ?? _innerDrawingElementBlender.TryBlend(over, under);
        }
    }
}