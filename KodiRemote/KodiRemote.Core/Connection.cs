using System;
using KodiRemote.Core.Commands;
using Newtonsoft.Json;

namespace KodiRemote.Core
{
    [JsonObject]
    public sealed class Connection
    {
        public Connection(string address, string port, string login, string password)
            : this()
        {
            Address = address;
            Port = port;
            Login = login;
            Password = password;
        }

        public Connection()
        {
            Addons = new Addons(this);
            Application = new Application(this);
            AudioLibrary = new AudioLibrary(this);
            Files = new Files(this);
            Gui = new Gui(this);
            Input = new Input(this);
            JsonRpc = new JsonRpc(this);
            Player = new Player(this);
            Playlist = new Playlist(this);
            System = new Commands.System(this);
            VideoLibrary = new VideoLibrary(this);
            Xbmc = new Xbmc(this);
        }

        [JsonProperty]
        public string Address { get; set; }

        [JsonProperty]
        public string Port { get; set; }

        [JsonProperty]
        public string Login { get; set; }

        [JsonProperty]
        public string Password { get; set; }

        [JsonProperty]
        public string MacAddress { get; set; }

        public bool IsMocked
        {
            get { return Address == "123"; }
        }

        public Addons Addons { get; }

        public Application Application { get; }

        public AudioLibrary AudioLibrary { get; }

        public Files Files { get; }

        public Gui Gui { get; }

        public Input Input { get; }

        public JsonRpc JsonRpc { get; }

        public Player Player { get; }

        public Playlist Playlist { get; }

        public Commands.System System { get; }

        public VideoLibrary VideoLibrary { get; }

        public Xbmc Xbmc { get; }

        internal string BaseUrl
        {
            get { return string.Concat("http://", Address, ":", Port, "/jsonrpc"); }
        }

        public string GetFileUrl(string path)
        {
            return $"http://{Address}:{Port}/{path}";
        }

        public Uri GetFileUri(string path)
        {
            string url = GetFileUrl(path);
            return new Uri(url, UriKind.RelativeOrAbsolute);
        }

        public static Connection Default()
        {
            return new Connection("", "80", "kodi", "");
        }
    }
}