using System;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace KodiRemote.Uwp.Core
{
    /// <summary>
    /// Data to represent an item in the nav menu.
    /// </summary>
    public class NavMenuItem : INotifyPropertyChanged
    {
        public string Label { get; set; }

        public Symbol Symbol { get; set; }

        public char SymbolAsChar
        {
            get
            {
                return (char)Symbol;
            }
        }

        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                SelectedVis = value ? Visibility.Visible : Visibility.Collapsed;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        private Visibility _selectedVis = Visibility.Collapsed;

        public Visibility SelectedVis
        {
            get { return _selectedVis; }
            set
            {
                _selectedVis = value;
                OnPropertyChanged(nameof(SelectedVis));
            }
        }

        public Type DestPage { get; set; }

        public object Arguments { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            // Raise the PropertyChanged event, passing the name of the property whose value has changed.
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}