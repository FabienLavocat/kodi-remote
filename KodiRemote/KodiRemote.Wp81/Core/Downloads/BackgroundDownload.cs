using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace KodiRemote.Wp81.Core.Downloads
{
    public class BackgroundDownload : INotifyPropertyChanged
    {
        public string Id { get; set; }

        #region Filename

        private string _filename;

        public string Filename
        {
            get { return _filename; }
            set
            {
                _filename = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region BytesReceived

        private long _bytesReceived;

        public long BytesReceived
        {
            get { return _bytesReceived; }
            set
            {
                _bytesReceived = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region TotalBytesToReceive

        private long _totalBytesToReceive;

        public long TotalBytesToReceive
        {
            get { return _totalBytesToReceive; }
            set
            {
                _totalBytesToReceive = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region Status

        private string _status;

        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

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
