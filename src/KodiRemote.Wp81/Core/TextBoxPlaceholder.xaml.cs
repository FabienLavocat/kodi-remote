using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace KodiRemote.Wp81.Core
{
    public partial class TextBoxPlaceholder : UserControl
    {
        #region Placeholder

        public string Placeholder
        {
            get { return (string) GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register("Placeholder",
            typeof(string), typeof(TextBoxPlaceholder), new PropertyMetadata(null));

        #endregion

        #region PlaceholderVisibility

        public Visibility PlaceholderVisibility
        {
            get { return (Visibility) GetValue(PlaceholderVisibilityProperty); }
            set { SetValue(PlaceholderVisibilityProperty, value); }
        }

        public static readonly DependencyProperty PlaceholderVisibilityProperty = DependencyProperty.Register("PlaceholderVisibility",
            typeof(Visibility), typeof(TextBoxPlaceholder), new PropertyMetadata(Visibility.Visible));

        #endregion

        #region Text

        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text",
            typeof(string), typeof(TextBoxPlaceholder), new PropertyMetadata(null));

        #endregion

        #region InputScope

        public InputScope InputScope
        {
            get { return (InputScope) GetValue(InputScopeProperty); }
            set { SetValue(InputScopeProperty, value); }
        }

        public static readonly DependencyProperty InputScopeProperty = DependencyProperty.Register("InputScope",
            typeof(InputScope), typeof(TextBoxPlaceholder), new PropertyMetadata(null));

        #endregion

        public event TextChangedEventHandler TextChanged;

        public TextBoxPlaceholder()
        {
            InitializeComponent();

            PlaceholderVisibility = Visibility.Visible;

            MainTextBox.TextChanged += MainTextBoxTextChanged;
            MainTextBox.LostFocus += MainTextBoxLostFocus;
            MainTextBox.GotFocus += MainTextBoxGotFocus;
            
            LayoutRoot.DataContext = this;

            Loaded += TextBoxPlaceholder_Loaded;
        }

        private void TextBoxPlaceholder_Loaded(object sender, RoutedEventArgs e)
        {
            PlaceholderVisibilityMustChange();
            MainTextBox.InputScope = InputScope;
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
            if (TextChanged != null)
                TextChanged(sender, e);

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

        public new void Focus()
        {
            MainTextBox.Focus();
        }
    }
}
