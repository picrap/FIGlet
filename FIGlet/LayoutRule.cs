// A FIGlet generation library - MIT license
// https://github.com/picrap/FIGlet

namespace FIGlet
{
    /// <summary>
    /// The layout rule.
    /// It is predefined, and may be deprecated.
    /// Internally it uses blenders
    /// </summary>
    public enum LayoutRule
    {
        /// <summary>
        /// Full size width
        /// </summary>
        FullSize,
        /// <summary>
        /// Fits on spaces
        /// </summary>
        Fitting,
        /// <summary>
        /// All wonderful smushing rules
        /// </summary>
        Smushing,

        /// <summary>
        /// Anything else that was defined directly
        /// </summary>
        Custom,
    }
}