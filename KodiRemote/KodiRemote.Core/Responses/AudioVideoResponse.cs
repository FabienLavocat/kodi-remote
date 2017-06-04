using Newtonsoft.Json;
using KodiRemote.Core.Model;

namespace KodiRemote.Core.Responses
{
    [JsonObject]
    public abstract class LimitsResponseBase
    {
        [JsonProperty(PropertyName = "limits")]
        public ListLimitsReturned Limits { get; set; }
    }

    [JsonObject]
    public sealed class AlbumsResponse : LimitsResponseBase
    {
        [JsonProperty(PropertyName = "albums")]
        public AudioDetailsAlbum[] Albums { get; set; }
    }

    [JsonObject]
    public sealed class ArtistsResponse : LimitsResponseBase
    {
        [JsonProperty(PropertyName = "artists")]
        public AudioDetailsArtist[] Artists { get; set; }
    }

    [JsonObject]
    public sealed class GenresResponse : LimitsResponseBase
    {
        [JsonProperty(PropertyName = "genres")]
        public LibraryDetailsGenre[] Genres { get; set; }
    }

    [JsonObject]
    public sealed class SongsResponse : LimitsResponseBase
    {
        [JsonProperty(PropertyName = "songs")]
        public AudioDetailsSong[] Songs { get; set; }
    }

    [JsonObject]
    public sealed class EpisodesResponse : LimitsResponseBase
    {
        [JsonProperty(PropertyName = "episodes")]
        public VideoDetailsEpisode[] Episodes { get; set; }
    }

    [JsonObject]
    public sealed class MovieSetsResponse : LimitsResponseBase
    {
        [JsonProperty(PropertyName = "sets")]
        public VideoDetailsMovieSet[] Sets { get; set; }
    }

    [JsonObject]
    public sealed class MoviesResponse : LimitsResponseBase
    {
        [JsonProperty(PropertyName = "movies")]
        public VideoDetailsMovie[] Movies { get; set; }
    }

    [JsonObject]
    public sealed class MusicVideoResponse : LimitsResponseBase
    {
        [JsonProperty(PropertyName = "musicvideos")]
        public VideoDetailsMusicVideo[] MusicVideos { get; set; }
    }

    [JsonObject]
    public sealed class SeasonsResponse : LimitsResponseBase
    {
        [JsonProperty(PropertyName = "seasons")]
        public VideoDetailsSeason[] Seasons { get; set; }
    }

    [JsonObject]
    public sealed class TvShowsResponse : LimitsResponseBase
    {
        [JsonProperty(PropertyName = "tvshows")]
        public VideoDetailsTvShow[] TvShows { get; set; }
    }

    [JsonObject]
    public sealed class VideoDetailsTvShowResponse : LimitsResponseBase
    {
        [JsonProperty(PropertyName = "tvshowdetails")]
        public VideoDetailsTvShow TvShowDetails { get; set; }
    }

    [JsonObject]
    public sealed class VideoDetailsEpisodeResponse : LimitsResponseBase
    {
        [JsonProperty(PropertyName = "episodedetails")]
        public VideoDetailsEpisode EpisodeDetails { get; set; }
    }
}