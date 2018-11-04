namespace FIGlet.Blend
{
    using System.Linq;
    using Drawing;
    using Utility;

    /// <summary>
    /// Hierarchy smushing... Complicated. See doc :)
    /// </summary>
    /// <seealso cref="FIGlet.Blend.IDrawingElementBlender" />
    public class HierarchySmushingDrawingElementBlender : IDrawingElementBlender
    {
        private readonly char[][] _hierarchies;

        /// <summary>
        /// Initializes a new instance of the <see cref="HierarchySmushingDrawingElementBlender"/> class.
        /// </summary>
        /// <param name="hierarchies">The hierarchies.</param>
        public HierarchySmushingDrawingElementBlender(char[][] hierarchies)
        {
            _hierarchies = hierarchies;
        }

        /// <inheritdoc />
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