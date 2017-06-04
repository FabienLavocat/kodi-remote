using System;

namespace KodiRemote.Core.Requests
{
    public class RequestException : Exception
    {
        public string MethodName { get; private set; }

        public RequestException(string methodName, string message)
            : base(message)
        {
            MethodName = methodName;
        }
    }
}