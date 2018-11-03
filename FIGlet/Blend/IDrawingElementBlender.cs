// A FIGlet generation library - MIT license
// https://github.com/picrap/FIGlet

namespace FIGlet.Blend
{
    using Drawing;

    public interface IDrawingElementBlender
    {
        /// <summary>
        /// Tries to blend the two <see cref="DrawingElement"/> instances.
        /// </summary>
        /// <param name="under">The under.</param>
        /// <param name="over">The over.</param>
        /// <returns>A <see cref="DrawingElement"/> if blend succeeded, null otherwise</returns>
        DrawingElement TryBlend(DrawingElement under, DrawingElement over);
    }
}
