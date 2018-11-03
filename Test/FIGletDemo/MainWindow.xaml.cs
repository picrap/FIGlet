// A FIGlet generation library - MIT license
// https://github.com/picrap/FIGlet

namespace FIGletDemo
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using FIGlet;
    using Fonts;

    public partial class MainWindow
    {
        private readonly FIGfont _font;

        private CharacterSpacing CharacterSpacing => (CharacterSpacing)((FrameworkElement)Spacing.SelectedItem).Tag;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            _font = FIGfont.FromEmbeddedResource("small.flf", typeof(FontsRoot));
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            FocusManager.SetFocusedElement(Input, Input);
            Input.TextChanged += delegate { RenderAll(); };
            Spacing.SelectionChanged += delegate { RenderAll(); };
        }

        private void RenderAll()
        {
            var figDriver = new FIGdriver { Font = _font, CharacterSpacing = CharacterSpacing };
            figDriver.Write(Input.Text);
            Render.Text = figDriver.ToString();
        }
    }
}
