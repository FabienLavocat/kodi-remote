namespace KodiRemote.Core.Requests
{
    public sealed class TimeoutException : RequestException
    {
        public TimeoutException(string methodName)
            : base(methodName, "Request timeout")
        {
            
        }
    }
}
