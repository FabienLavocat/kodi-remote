namespace KodiRemote.Core
{
    public static class Settings
    {
        public static string Address { get; private set; }
        public static string Port { get; private set; }
        public static string Login { get; private set; }
        internal static string Password { get; private set; }

        internal static string BaseUrl
        {
            get { return IsInitialized ? string.Concat("http://", Address, ":", Port, "/jsonrpc") : string.Empty;}
        }

        public static void Initialize(string address, string port, string login, string password)
        {
            Address = address;
            Port = port;
            Login = login;
            Password = password;
        }

        public static bool IsInitialized
        {
            get
            {
                return !string.IsNullOrEmpty(Address)
                       && !string.IsNullOrEmpty(Port)
                       && !string.IsNullOrEmpty(Login);
            }
        }

        public static string GetImageUrl(string path)
        {
            return IsInitialized
                       ? string.Concat("http://", Address, ":", Port, "/", path)
                       : string.Empty;
        }
    }
}