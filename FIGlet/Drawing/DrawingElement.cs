// A FIGlet generation library - MIT license
// https://github.com/picrap/FIGlet

namespace FIGlet.Drawing
{
    /// <summary>
    /// Default drawing element to be inserted in <see cref="DrawingBoard"/>
    /// It is created by <see cref="DrawingBoard"/> or by <see cref="ReplaceGlyph"/> method here.
    /// Overriding both allows to use derived instances of <see cref="DrawingElement"/> with may embed metadata (such as color)
    /// that can be retrieved at render
    /// </summary>
    public class DrawingElement
    {
        /// <summary>
        /// The glyph is the character (basic ASCII) to be shown.
        /// </summary>
        /// <value>
        /// The glyph.
        /// </value>
        public char Glyph { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is a blank glyph.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is blank; otherwise, <c>false</c>.
        /// </value>
        public bool IsBlank => Glyph == ' ' || Glyph == FIGdriver.HardBlank;

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawingElement"/> class.
        /// </summary>
        /// <param name="glyph">The glyph.</param>
        public DrawingElement(char glyph)
        {
            Glyph = glyph;
        }

        /// <summary>
        /// Replaces the glyph (and returns a new <see cref="DrawingElement"/>).
        /// </summary>
        /// <param name="newGlyph">The new glyph.</param>
        /// <returns></returns>
        public virtual DrawingElement ReplaceGlyph(char newGlyph)
        {
            return new DrawingElement(newGlyph);
        }
    }
}