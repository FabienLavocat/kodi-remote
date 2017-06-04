using KodiRemote.Core.Base;
using KodiRemote.Core.Commands;
using Newtonsoft.Json;

namespace KodiRemote.Core.Model
{
    [JsonObject]
    public sealed class Playlist
    {
        [JsonProperty(PropertyName = "playlistid")]
        public int PlaylistId { get; set; }

        [JsonProperty(PropertyName = "type")]
        public PlaylistType Type { get; set; }
    }

    [JsonObject]
    public sealed class PlaylistAdd : Parameters
    {
        [JsonProperty(PropertyName = "playlistid")]
        public int PlaylistId { get; set; }

        [JsonProperty(PropertyName = "item")]
        public PlaylistItem Item { get; set; }
    }

    [JsonObject]
    public sealed class PlaylistRemove : Parameters
    {
        [JsonProperty(PropertyName = "playlistid")]
        public int PlaylistId { get; set; }

        [JsonProperty(PropertyName = "position")]
        public int Position { get; set; }
    }

    [JsonObject]
    public sealed class PlaylistGetItems
    {
        [JsonProperty(PropertyName = "limits")]
        public ListLimitsReturned Limits { get; set; }

        [JsonProperty(PropertyName = "items")]
        public ListItemAll[] Items { get; set; }
    }

    [JsonObject]
    public sealed class PlaylistItem
    {
        /// <summary>Path to a file (not a directory) to be added to the playlist.</summary>
        [JsonProperty(PropertyName = "file",
        DefaultValueHandling = DefaultValueHandling.Ignore,
        NullValueHandling = NullValueHandling.Ignore)]
        public string File { get; set; }

        [JsonProperty(PropertyName = "directory",
        DefaultValueHandling = DefaultValueHandling.Ignore,
        NullValueHandling = NullValueHandling.Ignore)]
        public string Directory { get; set; }

        [JsonProperty(PropertyName = "movieid",
        DefaultValueHandling = DefaultValueHandling.Ignore,
        NullValueHandling = NullValueHandling.Ignore)]
        public int? MovieId { get; set; }

        [JsonProperty(PropertyName = "episodeid",
        DefaultValueHandling = DefaultValueHandling.Ignore,
        NullValueHandling = NullValueHandling.Ignore)]
        public int? EpisodeId { get; set; }

        [JsonProperty(PropertyName = "musicvideoid",
        DefaultValueHandling = DefaultValueHandling.Ignore,
        NullValueHandling = NullValueHandling.Ignore)]
        public int? MusicVideoid { get; set; }

        [JsonProperty(PropertyName = "artistid",
        DefaultValueHandling = DefaultValueHandling.Ignore,
        NullValueHandling = NullValueHandling.Ignore)]
        public int? ArtistId { get; set; }

        [JsonProperty(PropertyName = "albumid",
        DefaultValueHandling = DefaultValueHandling.Ignore,
        NullValueHandling = NullValueHandling.Ignore)]
        public int? AlbumId { get; set; }

        [JsonProperty(PropertyName = "songid",
        DefaultValueHandling = DefaultValueHandling.Ignore,
        NullValueHandling = NullValueHandling.Ignore)]
        public int? SongId { get; set; }

        /// <summary>Identification of a genre from the AudioLibrary.</summary>
        [JsonProperty(PropertyName = "genreid",
        DefaultValueHandling = DefaultValueHandling.Ignore,
        NullValueHandling = NullValueHandling.Ignore)]
        public int? GenreId { get; set; }
    }
}