// A FIGlet generation library - MIT license
// https://github.com/picrap/FIGlet

namespace FIGlet.Blend
{
    /// <summary>
    /// This class holds special instances of <see cref="IDrawingElementBlender"/> implementations
    /// </summary>
    public static class DrawingElementBlender
    {
        /// <summary>
        /// A blender that will write only on empty spaces
        /// </summary>
        public static readonly IDrawingElementBlender WriteOnEmptyOnly = new NoOverrideDrawingElementBlender();

        /// <summary>
        /// A blender that writes on spaces (and also empty spaces)
        /// </summary>
        public static readonly IDrawingElementBlender WriteOnSpace = new SpecificOverrideDrawingElementBlender(true, new[] { ' ' });

        /// <summary>
        /// Transforms a blender to a blender without priority (under=over).
        /// </summary>
        /// <param name="blender">The blender.</param>
        /// <returns></returns>
        public static IDrawingElementBlender WithoutPriority(this IDrawingElementBlender blender)
        {
            return new FlatDrawingElementBlender(blender);
        }

        /// <summary>
        /// Inverses the specified blender. Allows to blend if inner blender refuses to.
        /// </summary>
        /// <param name="blender">The blender.</param>
        /// <returns></returns>
        public static IDrawingElementBlender Inverse(this IDrawingElementBlender blender)
        {
            return new InverseDrawingElementBlender(blender);
        }

        /// <summary>
        /// Specific blend: merges to given arguments to a specified third.
        /// </summary>
        /// <param name="under">The under.</param>
        /// <param name="over">The over.</param>
        /// <param name="replace">The replace.</param>
        /// <returns></returns>
        public static IDrawingElementBlender Blend(char under, char over, char replace)
        {
            return new PairDrawingElementBlender(under, over, replace);
        }

        /// <summary>
        /// Applies one or another blender (the first that works)
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns></returns>
        public static IDrawingElementBlender Or(this IDrawingElementBlender a, IDrawingElementBlender b)
        {
            // TODO: optimize (if a or b is already a MultiDrawingElementBlender)
            return new MultiDrawingElementBlender(a, b);
        }

        /// <summary>
        /// Smushing #1
        /// </summary>
        public static readonly IDrawingElementBlender EqualCharacterSmushing = new EqualsDrawingElementBlender(new[] { FIGdriver.HardBlank });

        /// <summary>
        /// Smushing #2
        /// </summary>
        public static readonly IDrawingElementBlender UnderscoreSmushing =
            new SpecificOverrideDrawingElementBlender(false, new[] { '_' }, new[] { '|', '/', '\\', '[', ']', '{', '}', '(', ')', '<', '>' });

        /// <summary>
        /// Smushing #3
        /// </summary>
        public static readonly IDrawingElementBlender HierarchySmushing =
            new HierarchySmushingDrawingElementBlender(new[]
                {new[] {'|'}, new[] {'/', '\\'}, new[] {'[', ']'}, new[] {'{', '}'}, new[] {'(', ')'}, new[] {'<', '>'}}).WithoutPriority();

        /// <summary>
        /// Smushing #4
        /// </summary>
        public static readonly IDrawingElementBlender OppositePairSmushing =
            Blend('[', ']', '|').WithoutPriority().Or(Blend('{', '}', '|').WithoutPriority()).Or(Blend('(', ')', '|').WithoutPriority());

        /// <summary>
        /// Smushing #5
        /// </summary>
        public static readonly IDrawingElementBlender BigXSmushing = Blend('/', '\\', '|').Or(Blend('\\', '/', 'Y')).Or(Blend('>', '<', 'X'));

        /// <summary>
        /// Smushing #6
        /// </summary>
        public static readonly IDrawingElementBlender HardBlankSmushing = Blend(FIGdriver.HardBlank, FIGdriver.HardBlank, FIGdriver.HardBlank);
    }
}
