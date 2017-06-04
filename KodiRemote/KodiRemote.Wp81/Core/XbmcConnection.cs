using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Newtonsoft.Json;
using KodiRemote.Core;
using KodiRemote.Wp81.Resources;

namespace KodiRemote.Wp81.Core
{
    [JsonObject]
    public class XbmcConnection : INotifyPropertyChanged
    {
        [JsonProperty]
        public string Id { get; set; }

        #region Name

        private string _name;

        [JsonProperty]
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value) return;
                _name = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region Version

        private string _version;

        [JsonProperty]
        public string Version
        {
            get { return _version; }
            set
            {
                if (_version == value) return;
                _version = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region IsOnline

        private bool _isOnline;

        public bool IsOnline
        {
            get { return _isOnline; }
            set
            {
                if (_isOnline == value) return;
                _isOnline = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region Status

        private ConnectionStatus _status;

        public ConnectionStatus Status
        {
            get { return _status; }
            set
            {
                if (_status == value) return;
                _status = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region IsDefault

        private bool _isDefault;

        [JsonProperty]
        public bool IsDefault
        {
            get { return _isDefault; }
            set
            {
                if (_isDefault == value) return;
                _isDefault = value;
                NotifyPropertyChanged();

                if (value)
                    StartTestingLoop();
                else
                    StopTestingLoop();
            }
        }

        #endregion

        #region Xbmc

        private Connection _xbmc;

        [JsonProperty]
        public Connection Xbmc
        {
            get { return _xbmc; }
            set
            {
                if (_xbmc == value) return;
                _xbmc = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        public XbmcConnection()
        {
            Id = Guid.NewGuid().ToString();
            Name = AppResources.Loading;
            Status = ConnectionStatus.Pending;
        }

        private bool _askStop;

        private async void StartTestingLoop()
        {
            if (Xbmc == null) return;

            if (Xbmc.IsMocked)
            {
                IsOnline = true;
                Status = ConnectionStatus.Online;
                Name = "Kodi server";
                Version = "Version 15";
                return;
            }

            do
            {
                Status = ConnectionStatus.Pending;
                try
                {
                    IsOnline = await Xbmc.JsonRpc.PingAsync();
                    Status = IsOnline ? ConnectionStatus.Online : ConnectionStatus.Offline;

                    await Task.Delay(4000);
                    continue;
                }
                catch { }

                await Task.Delay(4000);
                
            } while (!_askStop);
        }

        private void StopTestingLoop()
        {
            _askStop = true;
        }

        public async Task<bool> TestConnectionAsync()
        {
            if (Xbmc.IsMocked)
            {
                IsOnline = true;
                Status = ConnectionStatus.Online;
                Name = "Kodi server";
                return true;
            }

            Status = ConnectionStatus.Pending;

            const string labelFriendlyName = "System.FriendlyName";
            const string labelBuildVersion = "System.BuildVersion";

            IsOnline = await Xbmc.JsonRpc.PingAsync();
            Status = IsOnline ? ConnectionStatus.Online : ConnectionStatus.Offline;
            if (!IsOnline) return false;

            var labels = await Xbmc.Xbmc.GetInfoLabelsAsync(labelFriendlyName, labelBuildVersion);
            Name = labels[labelFriendlyName];
            Version = labels[labelBuildVersion];

            return true;
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