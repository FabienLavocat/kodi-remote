using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Windows.Storage;

namespace KodiRemote.Uwp.Core
{
    public class Context : INotifyPropertyChanged
    {
        #region Connections

        private ObservableCollection<KodiConnection> _connections;

        public ObservableCollection<KodiConnection> Connections
        {
            get { return _connections; }
            private set
            {
                if (_connections == value) return;
                _connections = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(Connection));
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
                {
                    if (!_settings.Values.ContainsKey(nameof(DownloadFanArt)))
                        _settings.Values[nameof(DownloadFanArt)] = true;

                    _downloadFanArt = (bool)_settings.Values[nameof(DownloadFanArt)];
                }

                return _downloadFanArt.Value;
            }
            set
            {
                _settings.Values[nameof(DownloadFanArt)] = value;
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
                {
                    if (!_settings.Values.ContainsKey(nameof(DownloadThumbnails)))
                        _settings.Values[nameof(DownloadThumbnails)] = true;

                    _downloadThumbnails = (bool)_settings.Values[nameof(DownloadThumbnails)];
                }

                return _downloadThumbnails.Value;
            }
            set
            {
                _settings.Values[nameof(DownloadThumbnails)] = value;
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
                {
                    if (!_settings.Values.ContainsKey(nameof(AllowVibrate)))
                        _settings.Values[nameof(AllowVibrate)] = true;

                    _allowVibrate = (bool)_settings.Values[nameof(AllowVibrate)];
                }

                return _allowVibrate.Value;
            }
            set
            {
                _settings.Values[nameof(AllowVibrate)] = value;
                _allowVibrate = value;
            }
        }

        #endregion

        public KodiConnection Connection
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

        public void SetDefaultConnection(KodiConnection connection)
        {
            Connection = connection;
        }

        private readonly ApplicationDataContainer _settings;

        public Context()
        {
            _settings = ApplicationData.Current.RoamingSettings;

            try
            {
                if (_settings.Values.ContainsKey(nameof(Connections)))
                {
                    string serialization = _settings.Values[nameof(Connections)] as string;
                    var array = JsonConvert.DeserializeObject<KodiConnection[]>(serialization);
                    Connections = new ObservableCollection<KodiConnection>(array);
                }
            }
            catch (Exception) { }

            if (Connections == null)
                Connections = new ObservableCollection<KodiConnection>();

#if DEBUG
            if (!Connections.Any())
            {
                var cnx = new KodiConnection
                {
                    IsDefault = true,
                    Kodi = new KodiRemote.Core.Connection("123", "80", "kodi", "")
                };
                Connections.Add(cnx);
            }
#endif
        }

        public void Save()
        {
            string serialization = JsonConvert.SerializeObject(Connections.ToArray(), Formatting.None);
            _settings.Values[nameof(Connections)] = serialization;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}