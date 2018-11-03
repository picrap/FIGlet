// A FIGlet generation library - MIT license
// https://github.com/picrap/FIGlet

namespace FIGlet.Blend
{
    public static class DrawingElementBlender
    {
        public static IDrawingElementBlender WriteOnEmptyOnly = new NoOverrideDrawingElementBlender();

        public static IDrawingElementBlender WriteOnSpace = new SpecificOverrideDrawingElementBlender(true, ' ');

        public static IDrawingElementBlender WithoutPriority(this IDrawingElementBlender blender)
        {
            return new FlatDrawingElementBlender(blender);
        }
    }
}