using Newtonsoft.Json;

namespace KodiRemote.Core.Model
{
    [JsonObject]
    public sealed class VideoCast
    {
        [JsonProperty(PropertyName = "thumbnail")]
        public string Thumbnail { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "role")]
        public string Role { get; set; }
    }

    [JsonObject]
    public class VideoDetailsBase : MediaDetailsBase
    {
        [JsonProperty(PropertyName = "art")]
        public MediaArtwork Art { get; set; }

        [JsonProperty(PropertyName = "playcount")]
        public int PlayCount { get; set; }

        public bool IsWatched { get { return PlayCount > 0; } }
    }

    [JsonObject]
    public sealed class VideoDetailsEpisode : VideoDetailsFile
    {
        [JsonProperty(PropertyName = "cast")]
        public VideoCast[] Cast { get; set; }

        [JsonProperty(PropertyName = "productioncode")]
        public string ProductionCode { get; set; }

        [JsonProperty(PropertyName = "rating")]
        public double Rating { get; set; }

        [JsonProperty(PropertyName = "votes")]
        public string Votes { get; set; }

        [JsonProperty(PropertyName = "episode")]
        public int Episode { get; set; }

        [JsonProperty(PropertyName = "showtitle")]
        public string ShowTitle { get; set; }

        [JsonProperty(PropertyName = "episodeid")]
        public int EpisodeId { get; set; }

        [JsonProperty(PropertyName = "tvshowid")]
        public int TvShowId { get; set; }

        [JsonProperty(PropertyName = "season")]
        public int Season { get; set; }

        [JsonProperty(PropertyName = "firstaired")]
        public string FirstAired { get; set; }

        [JsonProperty(PropertyName = "uniqueid")]
        public object UniqueId { get; set; }

        [JsonProperty(PropertyName = "writer")]
        public string[] Writer { get; set; }

        [JsonProperty(PropertyName = "originaltitle")]
        public string OriginalTitle { get; set; }
    }

    [JsonObject]
    public class VideoDetailsFile : VideoDetailsItem
    {
        [JsonProperty(PropertyName = "streamdetails")]
        public VideoStreams StreamDetails { get; set; }

        [JsonProperty(PropertyName = "director")]
        public string[] Director { get; set; }

        [JsonProperty(PropertyName = "resume")]
        public VideoResume Resume { get; set; }

        [JsonProperty(PropertyName = "runtime")]
        public int Runtime { get; set; }
    }

    [JsonObject]
    public class VideoDetailsItem : VideoDetailsMedia
    {
        [JsonProperty(PropertyName = "dateadded")]
        public string DateAdded { get; set; }

        [JsonProperty(PropertyName = "file")]
        public string File { get; set; }

        [JsonProperty(PropertyName = "lastplayed")]
        public string LastPlayed { get; set; }

        [JsonProperty(PropertyName = "plot")]
        public string Plot { get; set; }
    }

    [JsonObject]
    public class VideoDetailsMedia : VideoDetailsBase
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
    }

    [JsonObject]
    public sealed class VideoDetailsMovieDetails
    {
        [JsonProperty(PropertyName = "moviedetails")]
        public VideoDetailsMovie DetailsMovie { get; set; }
    }

    [JsonObject]
    public sealed class VideoDetailsMovie : VideoDetailsFile
    {
        [JsonProperty(PropertyName = "plotoutline")]
        public string PlotOutline { get; set; }

        [JsonProperty(PropertyName = "sorttitle")]
        public string SortTitle { get; set; }

        [JsonProperty(PropertyName = "movieid")]
        public int MovieId { get; set; }

        [JsonProperty(PropertyName = "cast")]
        public VideoCast[] Cast { get; set; }

        [JsonProperty(PropertyName = "votes")]
        public string Votes { get; set; }

        [JsonProperty(PropertyName = "showlink")]
        public string[] ShowLink { get; set; }

        [JsonProperty(PropertyName = "top250")]
        public int Top250 { get; set; }

        [JsonProperty(PropertyName = "trailer")]
        public string Trailer { get; set; }

        [JsonProperty(PropertyName = "year")]
        public int Year { get; set; }

        [JsonProperty(PropertyName = "country")]
        public string[] Country { get; set; }

        [JsonProperty(PropertyName = "studio")]
        public string[] Studio { get; set; }

        [JsonProperty(PropertyName = "set")]
        public string Set { get; set; }

        [JsonProperty(PropertyName = "genre")]
        public string[] Genre { get; set; }

        [JsonProperty(PropertyName = "mpaa")]
        public string Mpaa { get; set; }

        [JsonProperty(PropertyName = "setid")]
        public int SetId { get; set; }

        [JsonProperty(PropertyName = "rating")]
        public double Rating { get; set; }

        [JsonProperty(PropertyName = "tag")]
        public string[] Tag { get; set; }

        [JsonProperty(PropertyName = "TagLine")]
        public string TagLine { get; set; }

        [JsonProperty(PropertyName = "writer")]
        public string[] Writer { get; set; }

        [JsonProperty(PropertyName = "originaltitle")]
        public string OriginalTitle { get; set; }

        [JsonProperty(PropertyName = "imdbnumber")]
        public string ImdbNumber { get; set; }
    }

    [JsonObject]
    public class VideoDetailsMovieSet : VideoDetailsMedia
    {
        [JsonProperty(PropertyName = "setid")]
        public int SetId { get; set; }
    }

    [JsonObject]
    public sealed class VideoDetailsMovieSetExtended
    {
        [JsonProperty(PropertyName = "setdetails")]
        public SetDetails SetDetails { get; set; }
    }

    [JsonObject]
    public sealed class SetDetails : VideoDetailsMovieSet
    {
        [JsonProperty(PropertyName = "limits")]
        public ListLimitsReturned Limits { get; set; }

        [JsonProperty(PropertyName = "movies")]
        public VideoDetailsMovie[] Movies { get; set; }
    }

    [JsonObject]
    public class VideoDetailsMusicVideo : VideoDetailsFile
    {
        [JsonProperty(PropertyName = "genre")]
        public string[] Genre { get; set; }

        [JsonProperty(PropertyName = "artist")]
        public string[] Artist { get; set; }

        [JsonProperty(PropertyName = "musicvideoid")]
        public int MusicVideoId { get; set; }

        [JsonProperty(PropertyName = "tag")]
        public string[] Tag { get; set; }

        [JsonProperty(PropertyName = "album")]
        public string Album { get; set; }

        [JsonProperty(PropertyName = "track")]
        public int Track { get; set; }

        [JsonProperty(PropertyName = "studio")]
        public string[] Studio { get; set; }

        [JsonProperty(PropertyName = "year")]
        public int Year { get; set; }
    }

    [JsonObject]
    public class VideoDetailsSeason : VideoDetailsBase
    {
        [JsonProperty(PropertyName = "showtitle")]
        public string ShowTitle { get; set; }

        [JsonProperty(PropertyName = "watchedepisodes")]
        public int WatchedEpisodes { get; set; }

        [JsonProperty(PropertyName = "tvshowid")]
        public int TvShowId { get; set; }

        [JsonProperty(PropertyName = "episode")]
        public int Episode { get; set; }

        [JsonProperty(PropertyName = "season")]
        public int Season { get; set; }
    }

    [JsonObject]
    public class VideoDetailsTvShow : VideoDetailsItem
    {
        [JsonProperty(PropertyName = "sorttitle")]
        public string SortTitle { get; set; }

        [JsonProperty(PropertyName = "mpaa")]
        public string Mpaa { get; set; }

        [JsonProperty(PropertyName = "premiered")]
        public string Premiered{ get; set; }

        [JsonProperty(PropertyName = "year")]
        public int Year { get; set; }

        [JsonProperty(PropertyName = "episode")]
        public int Episode { get; set; }

        [JsonProperty(PropertyName = "watchedepisodes")]
        public int WatchedEpisodes { get; set; }

        public int RemainingEpisodes { get { return Episode - WatchedEpisodes; } }

        [JsonProperty(PropertyName = "votes")]
        public string Votes { get; set; }

        [JsonProperty(PropertyName = "rating")]
        public double Rating { get; set; }

        [JsonProperty(PropertyName = "tvshowid")]
        public int TvShowId { get; set; }

        [JsonProperty(PropertyName = "studio")]
        public string[] Studio { get; set; }

        [JsonProperty(PropertyName = "season")]
        public int Season { get; set; }

        [JsonProperty(PropertyName = "genre")]
        public string[] Genre { get; set; }

        [JsonProperty(PropertyName = "cast")]
        public VideoCast[] Cast { get; set; }

        [JsonProperty(PropertyName = "episodeguide")]
        public string EpisodeGuide { get; set; }

        [JsonProperty(PropertyName = "tag")]
        public string[] Tag { get; set; }

        [JsonProperty(PropertyName = "originaltitle")]
        public string OriginalTitle { get; set; }

        [JsonProperty(PropertyName = "imdbtitle")]
        public string ImdbNumber { get; set; }
    }

    [JsonObject]
    public sealed class VideoResume
    {
        [JsonProperty(PropertyName = "position")]
        public double Position { get; set; }

        [JsonProperty(PropertyName = "total")]
        public double Total { get; set; }
    }

    [JsonObject]
    public sealed class VideoStreams
    {
        [JsonProperty(PropertyName = "video")]
        public StreamVideo[] Video { get; set; }

        [JsonProperty(PropertyName = "audio")]
        public StreamAudio[] Audio { get; set; }

        [JsonProperty(PropertyName = "subtitle")]
        public StreamSubtitle[] SubTitle { get; set; }
    }

    [JsonObject]
    public sealed class StreamVideo
    {
        [JsonProperty(PropertyName = "height")]
        public int Height { get; set; }

        [JsonProperty(PropertyName = "width")]
        public int Width { get; set; }

        [JsonProperty(PropertyName = "aspect")]
        public double Aspect { get; set; }

        [JsonProperty(PropertyName = "codec")]
        public string Codec { get; set; }

        [JsonProperty(PropertyName = "duration")]
        public int Duration { get; set; }
    }

    [JsonObject]
    public sealed class StreamAudio
    {
        [JsonProperty(PropertyName = "channels")]
        public int Channels { get; set; }

        [JsonProperty(PropertyName = "language")]
        public string Language { get; set; }

        [JsonProperty(PropertyName = "codec")]
        public string Codec { get; set; }
    }

    [JsonObject]
    public sealed class StreamSubtitle
    {
        [JsonProperty(PropertyName = "language")]
        public string Language { get; set; }
    }
}