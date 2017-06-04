using System;
using Microsoft.Phone.BackgroundTransfer;

namespace KodiRemote.Wp81.Core.Downloads
{
    public delegate void DownloadStateEventHandler(BackgroundTransferRequest request, EventArgsState e);

    public sealed class EventArgsState : EventArgs
    {
        public DownloadRequestState State { get; }

        public EventArgsState(DownloadRequestState state)
        {
            State = state;
        }
    }
}
