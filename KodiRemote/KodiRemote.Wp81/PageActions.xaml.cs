using System;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;
using KodiRemote.Wp81.Core;
using KodiRemote.Wp81.Resources;

namespace KodiRemote.Wp81
{
    public partial class PageActions
    {
        #region Connection

        public XbmcConnection Connection
        {
            get { return (XbmcConnection) GetValue(ConnectionProperty); }
            set { SetValue(ConnectionProperty, value); }
        }

        public static readonly DependencyProperty ConnectionProperty =
            DependencyProperty.Register("Connection", typeof (XbmcConnection), typeof (PageActions),
                                        new PropertyMetadata(null));

        #endregion

        private DispatcherTimer _timer;
        private const int TIMER_INTERVAL = 1000;

        public PageActions()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string url = e.Uri.ToString();
            int index = url.IndexOf('?');

            Connection = App.Context.Connections.FirstOrDefault(c => c.Id == url.Substring(index + 1));

            DataContext = this;
            
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(TIMER_INTERVAL);

            _timer.Tick += TimerTick;
            _timer.Start();
        }

        private async void TimerTick(object sender, EventArgs e)
        {
            _timer.Stop();

            if (Connection.Xbmc.IsMocked) return;

            await Connection.TestConnectionAsync();

            _timer.Start();
        }

        private async void BtShutdown(object sender, EventArgs e)
        {
            if (Connection == null || Connection.Xbmc.IsMocked) return;
            
            await Connection.Xbmc.System.ShutdownAsync();
        }

        private async void BtHibernate(object sender, EventArgs e)
        {
            if (Connection == null || Connection.Xbmc.IsMocked) return;
            
            await Connection.Xbmc.System.HibernateAsync();
        }

        private async void BtSuspend(object sender, EventArgs e)
        {
            if (Connection == null || Connection.Xbmc.IsMocked) return;
            
            await Connection.Xbmc.System.SuspendAsync();
        }

        private async void BtReboot(object sender, EventArgs e)
        {
            if (Connection == null || Connection.Xbmc.IsMocked) return;
            
            await Connection.Xbmc.System.RebootAsync();
        }

        private async void BtUpdateAudioClick(object sender, EventArgs e)
        {
            if (!App.Context.Connection.Xbmc.IsMocked)
            {
                await App.Context.Connection.Xbmc.AudioLibrary.ScanAsync();
            }

            MessageBox.Show(AppResources.Page_Actions_Update_Message_Audio_Message,
                            AppResources.Page_Actions_Update_Audio_Library,
                            MessageBoxButton.OK);
        }

        private async void BtUpdateVideoClick(object sender, EventArgs e)
        {
            if (!App.Context.Connection.Xbmc.IsMocked)
            {
                await App.Context.Connection.Xbmc.VideoLibrary.ScanAsync();
            }

            MessageBox.Show(AppResources.Page_Actions_Update_Message_Video_Message,
                            AppResources.Page_Actions_Update_Video_Library,
                            MessageBoxButton.OK);
        }
    }
}