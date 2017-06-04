using Newtonsoft.Json;
using KodiRemote.Core.Model;

namespace KodiRemote.Core.Responses
{
    [JsonObject]
    public sealed class GetItemResponse
    {
        [JsonProperty(PropertyName = "item")]
        public ListItemAll Item { get; set; }
    }
}