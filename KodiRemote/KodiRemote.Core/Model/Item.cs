using Newtonsoft.Json;

namespace KodiRemote.Core.Model
{
    [JsonObject]
    public class ItemDetailsBase
    {
        [JsonProperty(PropertyName = "label")]
        public string Label { get; set; }
    }
}