using Newtonsoft.Json;

namespace KodiRemote.Core.Model
{
    [JsonObject]
    public sealed class AlbumDetails
    {
        [JsonProperty(PropertyName = "albumdetails")]
        public AudioDetailsAlbum Details { get; set; }
    }

    [JsonObject]
    public sealed class ArtistDetails
    {
        [JsonProperty(PropertyName = "artistdetails")]
        public AudioDetailsArtist Details { get; set; }
    }

    [JsonObject]
    public sealed class AudioDetailsAlbum : AudioDetailsMedia
    {
        [JsonProperty(PropertyName = "theme")]
        public string[] Theme { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "style")]
        public string[] Style { get; set; }

        [JsonProperty(PropertyName = "albumid")]
        public int AlbumId { get; set; }

        [JsonProperty(PropertyName = "playcount")]
        public int PlayCount { get; set; }

        [JsonProperty(PropertyName = "albumlabel")]
        public string AlbumLabel { get; set; }

        //[JsonProperty(PropertyName = "mood")]
        //public string[] Mood { get; set; }
    }

    [JsonObject]
    public sealed class AudioDetailsArtist : AudioDetailsBase
    {
        [JsonProperty(PropertyName = "born")]
        public string Born { get; set; }

        [JsonProperty(PropertyName = "formed")]
        public string Formed { get; set; }

        [JsonProperty(PropertyName = "died")]
        public string Died { get; set; }

        [JsonProperty(PropertyName = "style")]
        public string[] Style { get; set; }

        [JsonProperty(PropertyName = "yearsactive")]
        public string[] YearsActive { get; set; }

        [JsonProperty(PropertyName = "mood")]
        public string[] Mood { get; set; }

        //[JsonProperty(PropertyName = "musicbrainzartistid")]
        //public string MusicBrainzArtistId { get; set; }

        [JsonProperty(PropertyName = "disbanded")]
        public string Disbanded { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "artist")]
        public string ArtistName { get; set; }

        [JsonProperty(PropertyName = "instrument")]
        public string[] Instrument { get; set; }

        [JsonProperty(PropertyName = "artistid")]
        public int ArtistId { get; set; }
    }

    [JsonObject]
    public class AudioDetailsBase : MediaDetailsBase
    {
        [JsonProperty(PropertyName = "genre")]
        public string[] Genre { get; set; }
    }

    [JsonObject]
    public class AudioDetailsMedia : AudioDetailsBase
    {
        [JsonProperty(PropertyName = "displayartist")]
        public string DisplayArtist { get; set; }

        [JsonProperty(PropertyName = "artist")]
        public string[] Artist { get; set; }

        [JsonProperty(PropertyName = "genreid")]
        public int[] GenreId { get; set; }

        [JsonProperty(PropertyName = "musicbrainzalbumartistid")]
        public string MusicBrainzAlbumArtistId { get; set; }

        [JsonProperty(PropertyName = "year")]
        public int Year { get; set; }

        [JsonProperty(PropertyName = "rating")]
        public decimal Rating { get; set; }

        [JsonProperty(PropertyName = "artistid")]
        public int[] ArtistId { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "musicbrainzalbumid")]
        public string MusicBrainzAlbumId { get; set; }
    }

    [JsonObject]
    public sealed class AudioDetailsSong : AudioDetailsMedia
    {
        [JsonProperty(PropertyName = "lyrics")]
        public string Lyrics { get; set; }

        [JsonProperty(PropertyName = "songid")]
        public int SongId { get; set; }

        [JsonProperty(PropertyName = "albumartistid")]
        public int[] AlbumArtistId { get; set; }

        [JsonProperty(PropertyName = "disc")]
        public int Disc { get; set; }

        [JsonProperty(PropertyName = "comment")]
        public string Comment { get; set; }

        [JsonProperty(PropertyName = "playcount")]
        public int PlayCount { get; set; }

        [JsonProperty(PropertyName = "album")]
        public string Album { get; set; }

        [JsonProperty(PropertyName = "file")]
        public string File { get; set; }

        [JsonProperty(PropertyName = "lastplayed")]
        public string LastPlayed { get; set; }

        [JsonProperty(PropertyName = "albumid")]
        public int AlbumId { get; set; }

        //[JsonProperty(PropertyName = "musicbrainzartistid")]
        //public string MusicBrainzArtistId { get; set; }

        [JsonProperty(PropertyName = "albumartist")]
        public string[] AlbumArtist { get; set; }

        [JsonProperty(PropertyName = "duration")]
        public int Duration { get; set; }

        [JsonProperty(PropertyName = "musicbrainztrackid")]
        public string MusicBrainzTrackId { get; set; }

        [JsonProperty(PropertyName = "track")]
        public int Track { get; set; }
    }
}