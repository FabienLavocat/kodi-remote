using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using KodiRemote.Core;
using Newtonsoft.Json;
using Windows.ApplicationModel.Resources;

namespace KodiRemote.Uwp.Core
{
    [JsonObject]
    public class KodiConnection : INotifyPropertyChanged
    {
        public KodiConnection()
        {
            Id = Guid.NewGuid().ToString();

            ResourceLoader resourceLoader = ResourceLoader.GetForCurrentView();
            Name = resourceLoader.GetString("Loading");
            Status = ConnectionStatus.Pending;
        }

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

        #region Kodi

        private Connection _kodi;

        [JsonProperty]
        public Connection Kodi
        {
            get { return _kodi; }
            set
            {
                if (_kodi == value) return;
                _kodi = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        private bool _askStop;

        private async void StartTestingLoop()
        {
            if (Kodi == null) return;

            if (Kodi.IsMocked)
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
                    IsOnline = await Kodi.JsonRpc.PingAsync();
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
            if (Kodi.IsMocked)
            {
                IsOnline = true;
                Status = ConnectionStatus.Online;
                Name = "Kodi server";
                return true;
            }

            Status = ConnectionStatus.Pending;

            const string labelFriendlyName = "System.FriendlyName";
            const string labelBuildVersion = "System.BuildVersion";

            IsOnline = await Kodi.JsonRpc.PingAsync();
            Status = IsOnline ? ConnectionStatus.Online : ConnectionStatus.Offline;
            if (!IsOnline) return false;

            var labels = await Kodi.Server.GetInfoLabelsAsync(labelFriendlyName, labelBuildVersion);
            Name = labels[labelFriendlyName];
            Version = labels[labelBuildVersion];

            return true;
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
