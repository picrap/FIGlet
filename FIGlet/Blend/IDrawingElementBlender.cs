// A FIGlet generation library - MIT license
// https://github.com/picrap/FIGlet

namespace FIGlet.Blend
{
    using Drawing;

    /// <summary>
    /// All mechanisms are based upon this blender.
    /// It it used by layout to see if a character can be placed at a given position.
    /// The algorithm tries to blend all character, if it succeeds, the character can fit.
    /// The blender considers two <see cref="DrawingElement"/>:
    /// - under, the element being already on the <see cref="DrawingBoard"/>
    /// - over, the element that is tested for overwrite.
    /// </summary>
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
