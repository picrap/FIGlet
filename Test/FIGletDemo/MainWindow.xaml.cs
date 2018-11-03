// A FIGlet generation library - MIT license
// https://github.com/picrap/FIGlet

namespace FIGletDemo
{
    using System;
    using System.Windows.Controls;
    using FIGlet;
    using Fonts;

    public partial class MainWindow
    {
        private readonly FIGfont _font;

        public MainWindow()
        {
            InitializeComponent();

            _font = FIGfont.FromEmbeddedResource("small.flf", typeof(FontsRoot));
        }

        private void OnInputTextChanged(object sender, TextChangedEventArgs e)
        {
            var figDriver = new FIGdriver { Font = _font, CharacterSpacing = CharacterSpacing.FullSize };
            figDriver.Write(Input.Text);
            Render.Text = figDriver.ToString();
        }
    }
}
