namespace FIGlet.Blend
{
    using Drawing;

    /// <summary>
    /// Implementation of <see cref="IDrawingElementBlender"/> that inverses blend:
    /// - if inner blender wants to blend, it returns null
    /// - if inner blender does not want to blend, it returns the over
    /// </summary>
    /// <seealso cref="FIGlet.Blend.IDrawingElementBlender" />
    public class InverseDrawingElementBlender : IDrawingElementBlender
    {
        private readonly IDrawingElementBlender _innerDrawingElementBlender;

        /// <summary>
        /// Initializes a new instance of the <see cref="InverseDrawingElementBlender"/> class.
        /// </summary>
        /// <param name="innerDrawingElementBlender">The inner drawing element blender.</param>
        public InverseDrawingElementBlender(IDrawingElementBlender innerDrawingElementBlender)
        {
            _innerDrawingElementBlender = innerDrawingElementBlender;
        }

        /// <inheritdoc />
        public DrawingElement TryBlend(DrawingElement under, DrawingElement over)
        {
            if (_innerDrawingElementBlender.TryBlend(under, over) is null)
                return over;
            return null;
        }
    }
}