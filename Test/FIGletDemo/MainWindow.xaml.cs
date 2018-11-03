// A FIGlet generation library - MIT license
// https://github.com/picrap/FIGlet

namespace FIGletDemo
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using FIGlet;
    using Fonts;

    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnInputTextChanged(object sender, TextChangedEventArgs e)
        {
            var figWriter = new FIGwriter { Font = FIGfont.FromEmbeddedResource("small.flf", typeof(FontsRoot)), CharacterSpacing = CharacterSpacing.FullSize };
            figWriter.Write(Input.Text);
            Render.Text = string.Join(Environment.NewLine, figWriter.DrawingBoard.Render());
        }
    }
}