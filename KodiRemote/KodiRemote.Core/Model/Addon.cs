using KodiRemote.Core.Responses;
using Newtonsoft.Json;

namespace KodiRemote.Core.Model
{
    [JsonObject]
    public sealed class AddonsResponse : LimitsResponseBase
    {
        [JsonProperty(PropertyName = "addons")]
        public AddonDetailsBase[] Addons { get; set; }
    }

    [JsonObject]
    public sealed class AddonDetails : LimitsResponseBase
    {
        [JsonProperty(PropertyName = "albumdetails")]
        public AddonDetailsBase Details { get; set; }
    }

    [JsonObject]
    public sealed class AddonBase
    {
        [JsonProperty(PropertyName = "addon")]
        public AddonDetailsBase Addon { get; set; }
    }

    [JsonObject]
    public class AddonDetailsBase : ItemDetailsBase
    {
        [JsonProperty(PropertyName = "addonid")]
        public string AddonId { get; set; }

        [JsonProperty(PropertyName = "disclaimer")]
        public string Disclaimer { get; set; }

        [JsonProperty(PropertyName = "fanart")]
        public string Fanart { get; set; }

        // TODO: Change the property type
        [JsonProperty(PropertyName = "broken")]
        public string Broken { get; set; }

        [JsonProperty(PropertyName = "author")]
        public string Author { get; set; }

        [JsonProperty(PropertyName = "enabled")]
        public bool Enabled { get; set; }

        // TODO: Change the property type
        //[JsonProperty(PropertyName = "extrainfo")]
        //public string[] ExtraInfo { get; set; }

        [JsonProperty(PropertyName = "thumbnail")]
        public string Thumbnail { get; set; }

        [JsonProperty(PropertyName = "path")]
        public string Path { get; set; }

        // TODO: Change the property type
        //[JsonProperty(PropertyName = "dependencies")]
        //public string[] Dependencies { get; set; }

        // TODO: Change the property type
        //[JsonProperty(PropertyName = "type")]
        //public string[] Type { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "version")]
        public string Version { get; set; }

        [JsonProperty(PropertyName = "summary")]
        public string Summary { get; set; }

        [JsonProperty(PropertyName = "rating")]
        public int Rating { get; set; }
    }
}
