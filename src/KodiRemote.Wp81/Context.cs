using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using KodiRemote.Wp81.Core;

namespace KodiRemote.Wp81
{
    public class Context : INotifyPropertyChanged
    {
        #region Connections

        private ObservableCollection<XbmcConnection> _connections;

        public ObservableCollection<XbmcConnection> Connections
        {
            get { return _connections; }
            private set
            {
                if (_connections == value) return;
                _connections = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("Connection");
            }
        }

        #endregion

        #region DownloadFanArt

        private bool? _downloadFanArt;

        public bool DownloadFanArt
        {
            get
            {
                if (!_downloadFanArt.HasValue)
                    _downloadFanArt = IsolatedStorage.ReadFromIsolatedStorage("DownloadFanArt", true);

                return _downloadFanArt.Value;
            }
            set
            {
                IsolatedStorage.SaveToIsolatedStorage("DownloadFanArt", value);
                _downloadFanArt = value;
            }
        }

        #endregion

        #region DownloadThumbnails

        private bool? _downloadThumbnails;

        public bool DownloadThumbnails
        {
            get
            {
                if (!_downloadThumbnails.HasValue)
                    _downloadThumbnails = IsolatedStorage.ReadFromIsolatedStorage("DownloadThumbnails", true);

                return _downloadThumbnails.Value;
            }
            set
            {
                IsolatedStorage.SaveToIsolatedStorage("DownloadThumbnails", value);
                _downloadThumbnails = value;
            }
        }

        #endregion

        #region AllowVibrate

        private bool? _allowVibrate;

        public bool AllowVibrate
        {
            get
            {
                if (!_allowVibrate.HasValue)
                    _allowVibrate = IsolatedStorage.ReadFromIsolatedStorage("AllowVibrate", true);

                return _allowVibrate.Value;
            }
            set
            {
                IsolatedStorage.SaveToIsolatedStorage("AllowVibrate", value);
                _allowVibrate = value;
            }
        }

        #endregion

        public XbmcConnection Connection
        {
            get
            {
                var cnx = Connections.FirstOrDefault(c => c.IsDefault);
                if (cnx == null)
                {
                    cnx = Connections.FirstOrDefault();
                    SetDefaultConnection(cnx);
                }

                return cnx;
            }
            private set
            {
                foreach (var xbmcConnection in Connections)
                    xbmcConnection.IsDefault = false;

                if (value != null)
                    value.IsDefault = true;

                NotifyPropertyChanged();
            }
        }

        public void SetDefaultConnection(XbmcConnection connection)
        {
            Connection = connection;
        }

        public Context()
        {
            try
            {
                var serialization = IsolatedStorage.ReadFromIsolatedStorage("Connections", string.Empty);
                var array = JsonConvert.DeserializeObject<XbmcConnection[]>(serialization);
                Connections = new ObservableCollection<XbmcConnection>(array);
            }
            catch (Exception)
            {
                Connections = new ObservableCollection<XbmcConnection>();
            }

#if DEBUG
            if (!Connections.Any())
            {
                var cnx = new XbmcConnection
                    {
                        IsDefault = true,
                        Xbmc = new KodiRemote.Core.Connection("123", "80", "xbmc", "")
                    };
                Connections.Add(cnx);
            }
#endif
        }

        public void Save()
        {
            var serialization = JsonConvert.SerializeObject(Connections.ToArray(), Formatting.None);

            IsolatedStorage.SaveToIsolatedStorage("Connections", serialization);
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}