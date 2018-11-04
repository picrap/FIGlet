// A FIGlet generation library - MIT license
// https://github.com/picrap/FIGlet

namespace FIGletDemo
{
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using FIGlet;
    using Fonts;

    public partial class MainWindow
    {
        private CharacterSpacing CharacterSpacing => (CharacterSpacing)((FrameworkElement)Spacing.SelectedItem).Tag;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            FocusManager.SetFocusedElement(Input, Input);
            Input.TextChanged += delegate { RenderAll(); };
            Spacing.SelectionChanged += delegate { RenderAll(); };
            Font.SelectionChanged += delegate { RenderAll(); };
            var fontRefs = FIGfontReference.Parse(typeof(FontsRoot));
            foreach (var fontRef in fontRefs.OrderBy(f => f.Name))
                Font.Items.Add(new ComboBoxItem { Content = fontRef.Name, Tag = fontRef });
            Font.SelectedIndex = 0;
        }

        private string _currentFontName;
        private FIGfont _currentFont;

        private FIGfont LoadFont(FIGfontReference reference)
        {
            if (reference.Name != _currentFontName)
            {
                _currentFont = reference.LoadFont();
                _currentFontName = reference.Name;

                CharacterSpacing spacing;
                if (_currentFont.OldLayout == -1)
                    spacing = CharacterSpacing.FullSize;
                else if (_currentFont.OldLayout == 0)
                    spacing = CharacterSpacing.Fitting;
                else
                    spacing = CharacterSpacing.Smushing;
                Spacing.SelectedItem = Spacing.Items.Cast<FrameworkElement>().FirstOrDefault(e => (CharacterSpacing)e.Tag == spacing);
            }
            return _currentFont;
        }

        private void RenderAll()
        {
            var font = LoadFont((FIGfontReference)((FrameworkElement)Font.SelectedItem).Tag);
            var figDriver = new FIGdriver { Font = font, CharacterSpacing = CharacterSpacing };
            figDriver.Write(Input.Text);
            Render.Text = figDriver.ToString();
        }
    }
}
