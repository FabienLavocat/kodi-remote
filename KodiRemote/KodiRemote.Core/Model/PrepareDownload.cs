using Newtonsoft.Json;

namespace KodiRemote.Core.Model
{
    [JsonObject]
    public sealed class PrepareDownload
    {
        [JsonProperty(PropertyName = "mode")]
        public string Mode { get; set; }

        [JsonProperty(PropertyName = "protocol")]
        public string Protocol { get; set; }

        [JsonProperty(PropertyName = "details")]
        public PrepareDownloadDetails Details { get; set; }
    }

    [JsonObject]
    public sealed class PrepareDownloadDetails
    {
        [JsonProperty(PropertyName = "path")]
        public string Path { get; set; }
    }
}