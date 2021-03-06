﻿// A FIGlet generation library - MIT license
// https://github.com/picrap/FIGlet

namespace FIGlet
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Text;
    using Utility;

    /// <summary>
    /// Represents the font
    /// </summary>
    public class FIGfont
    {
        /// <summary>
        /// The FLF signature
        /// </summary>
        public const string FlfSignature = "flf2";

        /// <summary>
        /// Gets the hard blank character.
        /// </summary>
        /// <value>
        /// The hard blank.
        /// </value>
        public char HardBlank { get; private set; }
        /// <summary>
        /// Gets the font height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public int Height { get; private set; }
        /// <summary>
        /// Gets the font baseline.
        /// </summary>
        /// <value>
        /// The baseline.
        /// </value>
        public int Baseline { get; private set; }
        /// <summary>
        /// Gets the maximum length.
        /// </summary>
        /// <value>
        /// The maximum length.
        /// </value>
        public int MaxLength { get; private set; }
        /// <summary>
        /// Gets the old layout flags.
        /// </summary>
        /// <value>
        /// The old layout.
        /// </value>
        public int OldLayout { get; private set; }
        /// <summary>
        /// Gets the comment lines count.
        /// </summary>
        /// <value>
        /// The comment lines count.
        /// </value>
        public int CommentLinesCount { get; private set; }

        /// <summary>
        /// Gets the print direction.
        /// </summary>
        /// <value>
        /// The print direction.
        /// </value>
        public int? PrintDirection { get; private set; }
        /// <summary>
        /// Gets the full layout flags.
        /// </summary>
        /// <value>
        /// The full layout.
        /// </value>
        public int? FullLayout { get; private set; }
        /// <summary>
        /// Gets the codetag count.
        /// </summary>
        /// <value>
        /// The codetag count.
        /// </value>
        public int? CodetagCount { get; private set; }

        /// <summary>
        /// Gets the comment lines.
        /// </summary>
        /// <value>
        /// The comment lines.
        /// </value>
        public IList<string> CommentLines { get; private set; }

        /// <summary>
        /// Gets the characters.
        /// </summary>
        /// <value>
        /// The characters.
        /// </value>
        public IDictionary<UnicodeChar, FIGcharacter> Characters { get; private set; }

        /// <summary>
        /// Gets the default layout rule.
        /// </summary>
        /// <value>
        /// The layout rule.
        /// </value>
        public LayoutRule DefaultLayoutRule
        {
            get
            {
                if (OldLayout == -1)
                    return LayoutRule.FullSize;
                if (OldLayout == 0)
                    return LayoutRule.Fitting;
                return LayoutRule.Smushing;
            }
        }

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
            return FromStream(resourceStream);
        }

        /// <summary>
        /// Reads a <see cref="FIGfont"/> from an open <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public static FIGfont FromStream(Stream stream)
        {
            var figFont = new FIGfont();
            figFont.Read(stream);
            return figFont;
        }

        /// <summary>
        /// Reads a <see cref="FIGfont"/> from a file
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static FIGfont FromFile(string path)
        {
            using (var fileStream = File.OpenRead(path))
                return FromStream(fileStream);
        }

        /// <summary>
        /// Reads from specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <exception cref="NotSupportedException">Unknown format (WTF?)</exception>
        public void Read(Stream stream)
        {
            var signatureBytes = new byte[4];
            stream.Read(signatureBytes, 0, signatureBytes.Length);
            var flfSignatureBytes = Encoding.ASCII.GetBytes(FlfSignature);
            if (signatureBytes.SequenceEqual(flfSignatureBytes))
            {
                using (var textReader = new StreamReader(stream))
                    ReadContent(textReader);
                return;
            }

            var pkzipBytes = new byte[] { (byte)'P', (byte)'K', 3, 4 };
            if (signatureBytes.SequenceEqual(pkzipBytes))
            {
                ReadZipContent(stream);
                return;
            }

            throw new NotSupportedException("Unknown format (WTF?)");
        }

        private void ReadZipContent(Stream stream)
        {
            stream.ReadShort(); // version
            stream.ReadShort(); // flag
            var compressionMethod = stream.ReadShort();
            stream.ReadShort(); // modification time
            stream.ReadShort(); // modification date
            stream.ReadInt(); // CRC-32
            stream.ReadInt(); // compressed size
            var uncompressedSize = stream.ReadInt(); // uncompressed size
            var fileNameLength = stream.ReadShort();
            var extraFieldLength = stream.ReadShort();
            var fileNameBytes = stream.ReadBytes(fileNameLength);
            var extraField = stream.ReadBytes(extraFieldLength);

            var memoryStream = new MemoryStream();
            using (var decompressionStream = CreateDecompressionStream(stream, compressionMethod))
                decompressionStream.Copy(memoryStream, uncompressedSize);

            memoryStream.Seek(0, SeekOrigin.Begin);
            Read(memoryStream);
        }

        private Stream CreateDecompressionStream(Stream innerStream, int algorithm)
        {
            switch (algorithm)
            {
                case 8:
                    return new DeflateStream(innerStream, CompressionMode.Decompress, true);
                default:
                    throw new NotSupportedException("Right, not many compression formats are supported...");
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
            ReadContent(textReader);
        }

        private void ReadContent(TextReader textReader)
        {
            textReader.Read(); // that one is skipped
            var information = textReader.ReadLine()?.TrimEnd();
            if (information is null)
                throw new NotSupportedException("The format is unknown (again)");
            var informationParts = information.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            ReadMandatoryInformation(informationParts);
            ReadOptionalInformation(informationParts);

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
                var code = ReadCharacterCode(textReader, out _);
                if (!code.HasValue)
                    break;
                AddCharacter(ReadCharacter(code.Value, textReader));
            }
        }

        private void AddCharacter(FIGcharacter character)
        {
            if (!(character is null) && character.Code > 0)
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
            // we guess the end mark (I need to read the doc twice).
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

        private void ReadMandatoryInformation(IList<string> informationParts)
        {
            HardBlank = informationParts[0][0];
            Height = ParseInt(informationParts[1]);
            Baseline = ParseInt(informationParts[2]);
            MaxLength = ParseInt(informationParts[3]);
            OldLayout = ParseInt(informationParts[4]);
            CommentLinesCount = ParseInt(informationParts[5]);
        }

        private void ReadOptionalInformation(IList<string> informationParts)
        {
            if (informationParts.Count > 6)
                PrintDirection = ParseInt(informationParts[6]);
            if (informationParts.Count > 7)
                FullLayout = ParseInt(informationParts[7]);
            if (informationParts.Count > 8)
                CodetagCount = ParseInt(informationParts[8]);
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
            var signatureBuffer = new char[4];
            textReader.Read(signatureBuffer, 0, signatureBuffer.Length);
            var signature = new string(signatureBuffer);
            var b = signature == FlfSignature;
            return b;
        }
    }
}
