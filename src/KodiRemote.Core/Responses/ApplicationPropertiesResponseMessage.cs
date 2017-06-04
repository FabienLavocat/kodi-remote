using Newtonsoft.Json;

namespace KodiRemote.Core.Responses
{
    [JsonObject]
    public sealed class ApplicationPropertiesResponseMessage
    {
        [JsonProperty(PropertyName = "version")]
        public Model.Version Version { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "volume")]
        public int Volume { get; set; }

        [JsonProperty(PropertyName = "muted")]
        public bool Muted { get; set; }
    }
}