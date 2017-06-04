using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Shell;
using KodiRemote.Core.Commands;
using KodiRemote.Core.Model;
using KodiRemote.Wp81.Resources;

namespace KodiRemote.Wp81
{
    public class Playlist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ObservableCollection<ListItemAll> Items { get; private set; }

        public void SetItems(IEnumerable<ListItemAll> items)
        {
            Items = new ObservableCollection<ListItemAll>();

            foreach (var item in items)
            {
                if (string.IsNullOrWhiteSpace(item.Title))
                    item.Title = item.Label;

                Items.Add(item);
            }
        }
    }

    public partial class PagePlaylists
    {
        #region IsLoading

        public bool IsLoading
        {
            get { return (bool) GetValue(IsLoadingProperty); }
            private set { SetValue(IsLoadingProperty, value); }
        }

        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register("IsLoading", typeof(bool), typeof(PagePlaylists), new PropertyMetadata(false));

        #endregion

        #region Playlists

        public ObservableCollection<Playlist> Playlists
        {
            get { return (ObservableCollection<Playlist>) GetValue(PlaylistsProperty); }
            private set { SetValue(PlaylistsProperty, value); }
        }

        public static readonly DependencyProperty PlaylistsProperty =
            DependencyProperty.Register("Playlists", typeof(ObservableCollection<Playlist>), typeof(PagePlaylists), new PropertyMetadata(null));

        #endregion

        public PagePlaylists()
        {
            InitializeComponent();
            DataContext = this;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            AddApplicationBar();

            Playlists = new ObservableCollection<Playlist>();
            LoadPlaylists();
        }

        private async void LoadPlaylists()
        {
            IsLoading = true;
            Playlists.Clear();

            try
            {
                var playlists = await App.Context.Connection.Xbmc.Playlist.GetPlaylistsAsync();
                if (playlists != null)
                {
                    foreach (var p in playlists)
                    {
                        var items = await App.Context.Connection.Xbmc.Playlist.GetItemsAsync(p.PlaylistId, ListFieldsAll.title);
                        if (items == null || items.Items == null)
                            continue;

                        var playlist = new Playlist
                        {
                            Id = p.PlaylistId,
                            Name = p.Type.ToString()
                        };
                        playlist.SetItems(items.Items);

                        Playlists.Add(playlist);
                    }
                }
            }
            catch (Exception ex)
            {
                App.TrackException(ex);
                MessageBox.Show(AppResources.Global_Error_Message, AppResources.ApplicationTitle, MessageBoxButton.OK);
                if (NavigationService.CanGoBack)
                    NavigationService.GoBack();
            }
            finally
            {
                IsLoading = false;
            }

            if (!Playlists.Any())
            {
                MessageBox.Show(AppResources.Page_Playlists_No_Playlist, AppResources.Page_Playlists_Title, MessageBoxButton.OK);

                if (NavigationService.CanGoBack)
                    NavigationService.GoBack();
            }
        }

        private void AddApplicationBar()
        {
            ApplicationBar.Buttons.Clear();

            var appBarButtonReview =
                new ApplicationBarIconButton(new Uri("/Assets/appbar.layer.delete.png", UriKind.Relative))
                {
                    Text = AppResources.Page_Playlists_Clear
                };
            appBarButtonReview.Click += ClearPlaylist;
            ApplicationBar.Buttons.Add(appBarButtonReview);
        }

        private async void ClearPlaylist(object sender, EventArgs e)
        {
            if (Pivot.SelectedIndex < 0) return;

            IsLoading = true;

            try
            {
                Playlist playlist = Playlists[Pivot.SelectedIndex];
                await App.Context.Connection.Xbmc.Playlist.ClearAsync(playlist.Id);

                LoadPlaylists();
            }
            catch { }

            IsLoading = false;
        }
    }
}