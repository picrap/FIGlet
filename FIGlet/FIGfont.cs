// A FIGlet generation library - MIT license
// https://github.com/picrap/FIGlet

namespace FIGlet
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Utility;

    /// <summary>
    /// Represents the font
    /// </summary>
    public class FIGfont
    {
        public char HardBlank { get; private set; }
        public int Height { get; private set; }
        public int Baseline { get; private set; }
        public int MaxLength { get; private set; }
        public int OldLayout { get; private set; }
        public int CommentLinesCount { get; private set; }

        public int? PrintDirection { get; private set; }
        public int? FullLayout { get; private set; }
        public int? CodetagCount { get; private set; }

        public IList<string> CommentLines { get; private set; }

        public IDictionary<UnicodeChar, FIGcharacter> Characters { get; private set; }

        /// <summary>
        /// Loads a <see cref="FIGfont"/> from an embedded resource.
        /// </summary>
        /// <param name="resourceName">Full name of the resource.</param>
        /// <param name="siblingType">A type in the same project folder as resource (a sibling type, hence the name).</param>
        /// <returns></returns>
        public static FIGfont FromEmbeddedResource(string resourceName, Type siblingType)
        {
            var resourceStream = siblingType.Assembly.GetManifestResourceStream(siblingType, resourceName);
            if (resourceStream == null)
                return null;
            using (var streamReader = new StreamReader(resourceStream))
            {
                var figFont = new FIGfont();
                figFont.Read(streamReader);
                return figFont;
            }
        }

        /// <summary>
        /// Reads from  the specified <see cref="TextReader"/>.
        /// </summary>
        /// <param name="textReader">The text reader.</param>
        /// <exception cref="NotSupportedException">
        /// The format is unknown
        /// or
        /// Invalid headers
        /// </exception>
        public void Read(TextReader textReader)
        {
            if (!IsValid(textReader))
                throw new NotSupportedException("The format is unknown");
            var informations = textReader.ReadLine().TrimEnd();
            var informationParts = informations.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            // 6 parts are mandatory, 3 are optional
            switch (informationParts.Length)
            {
                case 6:
                    ReadMandatoryInformation(informationParts);
                    break;
                case 9:
                    ReadMandatoryInformation(informationParts);
                    ReadOptionalInformation(informationParts);
                    break;
                default:
                    throw new NotSupportedException("Invalid headers");
            }

            // comment is kept (not sure this helps)
            var comment = new List<string>();
            for (int commentLineIndex = 0; commentLineIndex < CommentLinesCount; commentLineIndex++)
                comment.Add(textReader.ReadLine());
            CommentLines = comment.AsReadOnly();

            // -- now load letters
            Characters = new Dictionary<UnicodeChar, FIGcharacter>();
            // all ASCII letters
            for (int i = 32; i <= 126; i++)
                AddCharacter(ReadCharacter((UnicodeChar)i, textReader));
            // then some extended (but implied) extra letters
            foreach (var s in new[] { 196, 214, 220, 228, 246, 252, 223 })
                AddCharacter(ReadCharacter((UnicodeChar)s, textReader));

            // then, it's free bar. Any letter can be added
            for (; ; )
            {
                var code = ReadCharacterCode(textReader, out var description);
                if (!code.HasValue)
                    break;
                AddCharacter(ReadCharacter(code.Value, textReader));
            }
        }

        private void AddCharacter(FIGcharacter character)
        {
            Characters[character.Code] = character;
        }

        /// <summary>
        /// Reads the letter code.
        /// </summary>
        /// <param name="textReader">The text reader.</param>
        /// <param name="description">The char description (if any).</param>
        /// <returns>A <see cref="char"/> representing the code</returns>
        private UnicodeChar? ReadCharacterCode(TextReader textReader, out string description)
        {
            var line = textReader.ReadLine();
            if (line == null)
            {
                description = null;
                return null;
            }

            var splitIndex = line.IndexOf(' ');
            if (splitIndex == -1)
            {
                description = null;
                return (UnicodeChar)ParseInt(line);
            }

            var literalCode = line.Substring(0, splitIndex);
            description = line.Substring(splitIndex + 1).Trim();
            return (UnicodeChar)ParseInt(literalCode);
        }

        /// <summary>
        /// Reads the letter.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="textReader">The text reader.</param>
        /// <returns></returns>
        private FIGcharacter ReadCharacter(UnicodeChar code, TextReader textReader)
        {
            var characterLines = new List<string>();
            // we guess the endmark (I need to read the doc twice).
            char? endMark = null;
            for (; ; )
            {
                var rawCharacterLine = textReader.ReadLine();
                if (rawCharacterLine == null)
                    return null;
                if (!endMark.HasValue)
                    endMark = rawCharacterLine.Last();
                var endMarksCount = CountTrailing(rawCharacterLine, endMark.Value);
                var characterLine = rawCharacterLine.Substring(0, rawCharacterLine.Length - endMarksCount);
                characterLines.Add(characterLine);
                if (endMarksCount == 2)
                    break;
            }
            return new FIGcharacter(code, characterLines);
        }

        /// <summary>
        /// Counts the trailing character.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="c">The c.</param>
        /// <returns></returns>
        private int CountTrailing(string s, char c)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (s[s.Length - i - 1] != c)
                    return i;
            }

            return s.Length;
        }

        /// <summary>
        /// Counts the heading character.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <param name="c">The c.</param>
        /// <returns></returns>
        private int CountHeading(string s, char c)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] != c)
                    return i;
            }

            return s.Length;
        }

        private void ReadMandatoryInformation(IList<string> informationParts)
        {
            HardBlank = informationParts[0][0];
            Height = ParseInt(informationParts[1]);
            Baseline = ParseInt(informationParts[2]);
            MaxLength = ParseInt(informationParts[3]);
            OldLayout = ParseInt(informationParts[4]);
            CommentLinesCount = ParseInt(informationParts[5]);
        }

        private void ReadOptionalInformation(IList<string> informationsParts)
        {
            PrintDirection = ParseInt(informationsParts[6]);
            FullLayout = ParseInt(informationsParts[7]);
            CodetagCount = ParseInt(informationsParts[8]);
        }

        /// <summary>
        /// Parses the specified literal. Throws exceptions when angry
        /// </summary>
        /// <param name="literal">The literal.</param>
        /// <returns></returns>
        /// <exception cref="FormatException"></exception>
        private int ParseInt(string literal)
        {
            var v = IntParser.TryParse(literal);
            if (!v.HasValue)
                throw new FormatException();
            return v.Value;
        }

        /// <summary>
        /// Indicates whether the file is valid.
        /// </summary>
        /// <param name="textReader">The text reader.</param>
        /// <returns>
        ///   <c>true</c> if the specified text reader is valid; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsValid(TextReader textReader)
        {
            var signatureBuffer = new char[5];
            textReader.Read(signatureBuffer, 0, signatureBuffer.Length);
            var signature = new string(signatureBuffer);
            var b = signature == "flf2a";
            return b;
        }
    }
}
