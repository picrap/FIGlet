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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnInputTextChanged(object sender, TextChangedEventArgs e)
        {
            var figDriver = new FIGdriver { Font = FIGfont.FromEmbeddedResource("small.flf", typeof(FontsRoot)), CharacterSpacing = CharacterSpacing.FullSize };
            figDriver.Write(Input.Text);
            Render.Text = string.Join(Environment.NewLine, figDriver.DrawingBoard.Render());
        }
    }
}
