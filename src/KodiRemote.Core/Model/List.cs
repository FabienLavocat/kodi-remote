using Newtonsoft.Json;

namespace KodiRemote.Core.Model
{
    [JsonObject]
    public class ListItemAll : ListItemBase
    {
        //TODO: Create PVRChannelType
        //[JsonProperty(PropertyName = "channeltype")]
        //public PVRChannelType ChannelType { get; set; }

        [JsonProperty(PropertyName = "channel")]
        public string Channel { get; set; }

        [JsonProperty(PropertyName = "starttime")]
        public string StartTime { get; set; }

        [JsonProperty(PropertyName = "endtime")]
        public string EndTime { get; set; }

        [JsonProperty(PropertyName = "channelnumber")]
        public int ChannelNumber { get; set; }

        [JsonProperty(PropertyName = "hidden")]
        public bool Hidden { get; set; }

        [JsonProperty(PropertyName = "locked")]
        public bool Locked { get; set; }
    }

    [JsonObject]
    public class ListItemBase : AudioDetailsMedia
    {
        //TODO: Find a better way to do that
        #region VideoDetailsFile

        [JsonProperty(PropertyName = "streamdetails")]
        public VideoStreams StreamDetails { get; set; }

        [JsonProperty(PropertyName = "director")]
        public string[] Director { get; set; }

        [JsonProperty(PropertyName = "resume")]
        public VideoResume Resume { get; set; }

        [JsonProperty(PropertyName = "runtime")]
        public int Runtime { get; set; }

        #region VideoDetailsItem

        [JsonProperty(PropertyName = "dateadded")]
        public string DateAdded { get; set; }

        [JsonProperty(PropertyName = "file")]
        public string File { get; set; }

        [JsonProperty(PropertyName = "lastplayed")]
        public string LastPlayed { get; set; }

        [JsonProperty(PropertyName = "plot")]
        public string Plot { get; set; }

        #region VideoDetailsItem

        #region VideoDetailsBase

        [JsonProperty(PropertyName = "art")]
        public MediaArtwork Art { get; set; }

        [JsonProperty(PropertyName = "playcount")]
        public int PlayCount { get; set; }

        #endregion

        #endregion

        #endregion

        #endregion

        [JsonProperty(PropertyName = "sorttitle")]
        public string SortTitle { get; set; }

        [JsonProperty(PropertyName = "productioncode")]
        public string ProductionCode { get; set; }

        [JsonProperty(PropertyName = "cast")]
        public VideoCast[] Cast { get; set; }

        [JsonProperty(PropertyName = "votes")]
        public string Votes { get; set; }

        [JsonProperty(PropertyName = "duration")]
        public int Duration { get; set; }

        [JsonProperty(PropertyName = "trailer")]
        public string Trailer { get; set; }

        [JsonProperty(PropertyName = "albumid")]
        public int AlbumId { get; set; }

        //[JsonProperty(PropertyName = "musicbrainzartistid")]
        //public string MusicBrainzArtistId { get; set; }

        [JsonProperty(PropertyName = "mpaa")]
        public string Mpaa { get; set; }

        [JsonProperty(PropertyName = "albumlabel")]
        public string AlbumLabel { get; set; }

        [JsonProperty(PropertyName = "originaltitle")]
        public string OriginalTitle { get; set; }

        [JsonProperty(PropertyName = "writer")]
        public string[] Writer { get; set; }

        [JsonProperty(PropertyName = "albumartistid")]
        public int[] AlbumArtistId { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "episode")]
        public int Episode { get; set; }

        [JsonProperty(PropertyName = "firstaired")]
        public string FirstAired { get; set; }

        [JsonProperty(PropertyName = "showtitle")]
        public string ShowTitle { get; set; }

        [JsonProperty(PropertyName = "country")]
        public string[] Country { get; set; }

        [JsonProperty(PropertyName = "mood")]
        public string[] Mood { get; set; }

        [JsonProperty(PropertyName = "set")]
        public string Set { get; set; }

        [JsonProperty(PropertyName = "musicbrainztrackid")]
        public string MusicBrainzTrackId { get; set; }

        [JsonProperty(PropertyName = "tag")]
        public string[] Tag { get; set; }

        [JsonProperty(PropertyName = "lyrics")]
        public string Lyrics { get; set; }

        [JsonProperty(PropertyName = "top250")]
        public int Top250 { get; set; }

        [JsonProperty(PropertyName = "comment")]
        public string Comment { get; set; }

        [JsonProperty(PropertyName = "premiered")]
        public string Premiered { get; set; }

        [JsonProperty(PropertyName = "showlink")]
        public string[] ShowLink { get; set; }

        [JsonProperty(PropertyName = "style")]
        public string[] Style { get; set; }

        [JsonProperty(PropertyName = "album")]
        public string Album { get; set; }

        [JsonProperty(PropertyName = "tvshowid")]
        public int TvShowId { get; set; }

        [JsonProperty(PropertyName = "season")]
        public int Season { get; set; }

        [JsonProperty(PropertyName = "theme")]
        public string[] Theme { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "setid")]
        public int SetId { get; set; }

        [JsonProperty(PropertyName = "track")]
        public int Track { get; set; }

        [JsonProperty(PropertyName = "tagline")]
        public string TagLine { get; set; }

        [JsonProperty(PropertyName = "plotoutline")]
        public string PlotOutline { get; set; }

        [JsonProperty(PropertyName = "watchedepisodes")]
        public int WatchedEpisodes { get; set; }

        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "disc")]
        public int Disc { get; set; }

        [JsonProperty(PropertyName = "albumartist")]
        public string[] AlbumArtist { get; set; }

        [JsonProperty(PropertyName = "studio")]
        public string[] Studio { get; set; }

        [JsonProperty(PropertyName = "uniqueid")]
        public object UniqueId { get; set; }

        [JsonProperty(PropertyName = "episodeguide")]
        public string EpisodeGuide { get; set; }

        [JsonProperty(PropertyName = "imdbnumber")]
        public string ImdbNumber { get; set; }
    }

    [JsonObject]
    public sealed class ListItemFile : ListItemBase
    {
        [JsonProperty(PropertyName = "filetype")]
        public string FileType { get; set; }

        [JsonProperty(PropertyName = "size")]
        public int Size { get; set; }

        [JsonProperty(PropertyName = "mimetype")]
        public string MimeType { get; set; }

        [JsonProperty(PropertyName = "lastmodified")]
        public string LastModified { get; set; }
    }

    [JsonObject]
    public sealed class ListLimits
    {
        [JsonProperty(PropertyName = "start")]
        public int Start { get; set; }

        [JsonProperty(PropertyName = "end")]
        public int End { get; set; }
    }

    [JsonObject]
    public sealed class ListLimitsReturned
    {
        [JsonProperty(PropertyName = "start")]
        public int Start { get; set; }

        [JsonProperty(PropertyName = "end")]
        public int End { get; set; }

        [JsonProperty(PropertyName = "total")]
        public int Total { get; set; }
    }

    [JsonObject]
    public sealed class ListSort
    {
        [JsonProperty(PropertyName = "order")]
        public string Order { get; set; }

        [JsonProperty(PropertyName = "ignorearticle")]
        public bool IgnoreArticle { get; set; }

        [JsonProperty(PropertyName = "method")]
        public string Method { get; set; }
    }
}