using Newtonsoft.Json;

namespace KodiRemote.Core.Model
{
    [JsonObject]
    public sealed class LibraryDetailsGenre
    {
        [JsonProperty(PropertyName = "thumbnail")]
        public string Thumbnail { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "genreid")]
        public int GenreId { get; set; }
    }
}