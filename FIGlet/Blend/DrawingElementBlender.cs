// A FIGlet generation library - MIT license
// https://github.com/picrap/FIGlet

namespace FIGlet.Blend
{
    public static class DrawingElementBlender
    {
        public static readonly IDrawingElementBlender WriteOnEmptyOnly = new NoOverrideDrawingElementBlender();

        public static readonly IDrawingElementBlender WriteOnSpace = new SpecificOverrideDrawingElementBlender(true, new[] { ' ' });

        public static IDrawingElementBlender WithoutPriority(this IDrawingElementBlender blender)
        {
            return new FlatDrawingElementBlender(blender);
        }

        public static IDrawingElementBlender Inverse(this IDrawingElementBlender blender)
        {
            return new InverseDrawingElementBlender(blender);
        }

        public static IDrawingElementBlender Blend(char under, char over, char replace)
        {
            return new PairDrawingElementBlender(under, over, replace);
        }

        public static IDrawingElementBlender Or(this IDrawingElementBlender a, IDrawingElementBlender b)
        {
            // TODO: optimize
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
