// A FIGlet generation library - MIT license
// https://github.com/picrap/FIGlet

namespace FIGlet
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// A reference to a fig font... Somewhere
    /// </summary>
    public abstract class FIGfontReference
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; }

        /// <summary>
        /// Loads the font.
        /// </summary>
        /// <returns></returns>
        public abstract FIGfont LoadFont();

        protected FIGfontReference(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Parses an assembly for fonts related to a sibling type.
        /// Type and embedded resources must be in the same project folder for this to work.
        /// </summary>
        /// <param name="siblingType">Type of the sibling.</param>
        /// <returns></returns>
        public static IEnumerable<FIGfontReference> Parse(Type siblingType)
        {
            var prefix = siblingType.Namespace + ".";
            foreach (var resourcePath in siblingType.Assembly.GetManifestResourceNames())
            {
                if (!resourcePath.StartsWith(prefix))
                    continue;

                var resourceName = resourcePath.Substring(prefix.Length);
                var extension = Path.GetExtension(resourceName);
                if (!extension.Equals(".zip", StringComparison.InvariantCultureIgnoreCase)
                    && !extension.Equals(".flf", StringComparison.InvariantCultureIgnoreCase))
                    continue;

                yield return new EmbeddedFIGfontReference(resourceName, siblingType, Path.GetFileNameWithoutExtension(resourceName));
            }
        }
    }
}