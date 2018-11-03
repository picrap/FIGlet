// A FIGlet generation library - MIT license
// https://github.com/picrap/FIGlet

namespace FIGlet
{
    using System;

    public class EmbeddedFIGfontReference : FIGfontReference
    {
        private readonly Type _siblingType;
        private readonly string _resourceName;

        public EmbeddedFIGfontReference(string resourceName, Type siblingType, string name)
            : base(name)
        {
            _siblingType = siblingType;
            _resourceName = resourceName;
        }

        public override FIGfont LoadFont()
        {
            return FIGfont.FromEmbeddedResource(_resourceName, _siblingType);
        }
    }
}