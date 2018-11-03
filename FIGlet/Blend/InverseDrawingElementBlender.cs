namespace FIGlet.Blend
{
    using Drawing;

    public class InverseDrawingElementBlender : IDrawingElementBlender
    {
        private readonly IDrawingElementBlender _innerDrawingElementBlender;

        public InverseDrawingElementBlender(IDrawingElementBlender innerDrawingElementBlender)
        {
            _innerDrawingElementBlender = innerDrawingElementBlender;
        }

        public DrawingElement TryBlend(DrawingElement under, DrawingElement over)
        {
            if (_innerDrawingElementBlender.TryBlend(under, over) is null)
                return over;
            return null;
        }
    }
}