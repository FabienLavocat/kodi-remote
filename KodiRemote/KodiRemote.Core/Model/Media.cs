using Newtonsoft.Json;

namespace KodiRemote.Core.Model
{
    [JsonObject]
    public sealed class MediaArtwork
    {
        [JsonProperty(PropertyName = "banner")]
        public string Banner { get; set; }

        [JsonProperty(PropertyName = "poster")]
        public string Poster { get; set; }

        [JsonProperty(PropertyName = "fanart")]
        public string FanArt { get; set; }

        [JsonProperty(PropertyName = "thumb")]
        public string Thumb { get; set; }
    }

    [JsonObject]
    public class MediaDetailsBase : ItemDetailsBase
    {
        [JsonProperty(PropertyName = "fanart")]
        public string FanArt { get; set; }

        [JsonProperty(PropertyName = "thumbnail")]
        public string Thumbnail { get; set; }
    }
}