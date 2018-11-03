namespace FIGlet.Blend
{
    using System.Linq;
    using Drawing;
    using Utility;

    public class HierarchySmushingDrawingElementBlender : IDrawingElementBlender
    {
        private readonly char[][] _hierarchies;

        public HierarchySmushingDrawingElementBlender(char[][] hierarchies)
        {
            _hierarchies = hierarchies;
        }

        public DrawingElement TryBlend(DrawingElement under, DrawingElement over)
        {
            if (under is null)
                return null;

            var underIndex = _hierarchies.IndexOf(h => h.Contains(under.Glyph));
            if (!underIndex.HasValue)
                return null;

            var overIndex = _hierarchies.IndexOf(h => h.Contains(over.Glyph));
            if (!overIndex.HasValue)
                return null;

            if (overIndex.Value > underIndex.Value)
                return over;

            return null;
        }
    }
}