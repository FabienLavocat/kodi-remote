using Newtonsoft.Json;

namespace KodiRemote.Core.Model
{
    [JsonObject]
    public sealed class Version
    {
        [JsonProperty(PropertyName = "major")]
        public int Major { get; set; }

        [JsonProperty(PropertyName = "minor")]
        public int Minor { get; set; }

        [JsonProperty(PropertyName = "patch")]
        public int Patch { get; set; }

        [JsonProperty(PropertyName = "tag")]
        public string Tag { get; set; }

        [JsonProperty(PropertyName = "revision")]
        public string Revision { get; set; }
    }

    [JsonObject]
    public sealed class JsonRpcVersion
    {
        [JsonProperty(PropertyName = "major")]
        public int Major { get; set; }

        [JsonProperty(PropertyName = "minor")]
        public int Minor { get; set; }

        [JsonProperty(PropertyName = "patch")]
        public int Patch { get; set; }
    }
}