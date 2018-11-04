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

        /// <summary>
        /// Initializes a new instance of the <see cref="FlatDrawingElementBlender"/> class.
        /// </summary>
        /// <param name="innerDrawingElementBlender">The inner drawing element blender.</param>
        public FlatDrawingElementBlender(IDrawingElementBlender innerDrawingElementBlender)
        {
            _innerDrawingElementBlender = innerDrawingElementBlender;
        }

        /// <inheritdoc />
        public DrawingElement TryBlend(DrawingElement under, DrawingElement over)
        {
            return _innerDrawingElementBlender.TryBlend(under, over) ?? _innerDrawingElementBlender.TryBlend(over, under);
        }
    }
}