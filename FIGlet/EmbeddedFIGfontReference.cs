// A FIGlet generation library - MIT license
// https://github.com/picrap/FIGlet

namespace FIGlet
{
    using System;

    /// <summary>
    /// Implementation of <see cref="FIGfontReference"/> for embedded resources
    /// </summary>
    /// <seealso cref="FIGlet.FIGfontReference" />
    public class EmbeddedFIGfontReference : FIGfontReference
    {
        private readonly Type _siblingType;
        private readonly string _resourceName;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmbeddedFIGfontReference"/> class.
        /// </summary>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="siblingType">Type of the sibling.</param>
        /// <param name="name">The name.</param>
        public EmbeddedFIGfontReference(string resourceName, Type siblingType, string name)
            : base(name)
        {
            _siblingType = siblingType;
            _resourceName = resourceName;
        }

        /// <inheritdoc />
        public override FIGfont LoadFont()
        {
            return FIGfont.FromEmbeddedResource(_resourceName, _siblingType);
        }
    }
}