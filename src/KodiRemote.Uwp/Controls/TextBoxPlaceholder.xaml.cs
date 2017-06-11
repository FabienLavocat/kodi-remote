using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace KodiRemote.Uwp.Controls
{
    public sealed partial class TextBoxPlaceholder : UserControl
    {
        public event TextChangedEventHandler TextChanged;

        public TextBoxPlaceholder()
        {
            InitializeComponent();
            LayoutRoot.DataContext = this;

            PlaceholderVisibility = Visibility.Visible;
            Loaded += TextBoxPlaceholder_Loaded;
        }
        
        #region Placeholder

        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register(nameof(Placeholder),
            typeof(string), typeof(TextBoxPlaceholder), new PropertyMetadata(null));

        #endregion

        #region PlaceholderVisibility

        public Visibility PlaceholderVisibility
        {
            get { return (Visibility)GetValue(PlaceholderVisibilityProperty); }
            set { SetValue(PlaceholderVisibilityProperty, value); }
        }

        public static readonly DependencyProperty PlaceholderVisibilityProperty = DependencyProperty.Register(nameof(PlaceholderVisibility),
            typeof(Visibility), typeof(TextBoxPlaceholder), new PropertyMetadata(Visibility.Visible));

        #endregion

        #region Text

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text),
            typeof(string), typeof(TextBoxPlaceholder), new PropertyMetadata(null));

        #endregion

        #region InputScope

        public InputScope InputScope
        {
            get { return (InputScope)GetValue(InputScopeProperty); }
            set { SetValue(InputScopeProperty, value); }
        }

        public static readonly DependencyProperty InputScopeProperty = DependencyProperty.Register(nameof(InputScope),
            typeof(InputScope), typeof(TextBoxPlaceholder), new PropertyMetadata(null));

        #endregion

        #region MaxLength

        public int MaxLength
        {
            get { return (int)GetValue(MaxLengthProperty); }
            set { SetValue(MaxLengthProperty, value); }
        }

        public static readonly DependencyProperty MaxLengthProperty = DependencyProperty.Register(nameof(MaxLength),
            typeof(int), typeof(TextBoxPlaceholder), new PropertyMetadata(250));

        #endregion

        private void TextBoxPlaceholder_Loaded(object sender, RoutedEventArgs e)
        {
            PlaceholderVisibilityMustChange();
        }

        private bool _hasFocus = false;

        private void MainTextBoxGotFocus(object sender, RoutedEventArgs e)
        {
            _hasFocus = true;
            PlaceholderVisibilityMustChange();
        }

        private void MainTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            _hasFocus = false;
            PlaceholderVisibilityMustChange();
        }

        private void MainTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            TextChanged?.Invoke(sender, e);

            PlaceholderVisibilityMustChange();
        }

        private void PlaceholderVisibilityMustChange()
        {
            if (_hasFocus)
                PlaceholderVisibility = Visibility.Collapsed;
            else
                PlaceholderVisibility = string.IsNullOrWhiteSpace(MainTextBox.Text) ? Visibility.Visible : Visibility.Collapsed;

            SubText.Text = PlaceholderVisibility == Visibility.Visible ? string.Empty : Placeholder;
        }

        public void Focus()
        {
            MainTextBox.Focus(FocusState.Keyboard);
        }
    }
}
