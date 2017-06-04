using Newtonsoft.Json;
using KodiRemote.Core.Base;
using KodiRemote.Core.Model;

namespace KodiRemote.Core.Requests
{
    [JsonObject]
    public class PropertiesParameters : Parameters
    {
        [JsonProperty(PropertyName = "properties")]
        public string[] Properties { get; set; }
    }

    [JsonObject]
    public sealed class ItemsParameters : Parameters
    {
        [JsonProperty(PropertyName = "items")]
        public string[] Items { get; set; }
    }

    [JsonObject]
    public class PlayerParameters : Parameters
    {
        [JsonProperty(PropertyName = "playerid")]
        public int PlayerId { get; set; }
    }

    [JsonObject]
    public sealed class PlayerGetItemParameters : PlayerParameters
    {
        [JsonProperty(PropertyName = "properties")]
        public string[] Properties { get; set; }
    }

    [JsonObject]
    public class PlaylistParameters : PropertiesParameters
    {
        [JsonProperty(PropertyName = "playlistid")]
        public int PlayListId { get; set; }
    }

    [JsonObject]
    public sealed class GoToParameters : PlayerParameters
    {
        [JsonProperty(PropertyName = "to")]
        public string To { get; set; }
    }

    #region Open Parameters

    [JsonObject]
    public sealed class OpenParameters<T> : Parameters
    {
        [JsonProperty(PropertyName = "item")]
        public T Item { get; set; }
        
        [JsonProperty(PropertyName = "options")]
        public OpenOptions Options { get; set; }
    }

    [JsonObject]
    public sealed class OpenOptions
    {
        [JsonProperty(PropertyName = "shuffled",
        DefaultValueHandling = DefaultValueHandling.Ignore,
        NullValueHandling = NullValueHandling.Ignore)]
        public bool? Shuffled { get; set; }

        [JsonProperty(PropertyName = "repeat",
        DefaultValueHandling = DefaultValueHandling.Ignore,
        NullValueHandling = NullValueHandling.Ignore)]
        public string Repeat { get; set; }

        [JsonProperty(PropertyName = "resume",
        DefaultValueHandling = DefaultValueHandling.Ignore,
        NullValueHandling = NullValueHandling.Ignore)]
        public bool? Resume { get; set; }
    }

    #endregion

    [JsonObject]
    public sealed class GetAddonsParameters : LimitsPropertiesParameters
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "content")]
        public string Content { get; set; }

        [JsonProperty(PropertyName = "enabled")]
        public string Enabled { get; set; }
    }

    [JsonObject]
    public sealed class AddonDetailsParameters : PropertiesParameters
    {
        [JsonProperty(PropertyName = "addonid")]
        public string AddonId { get; set; }
    }

    [JsonObject]
    public sealed class SetAddonEnabledParameters : Parameters
    {
        [JsonProperty(PropertyName = "addonid")]
        public string AddonId { get; set; }

        [JsonProperty(PropertyName = "enabled")]
        public bool Enabled { get; set; }
    }

    [JsonObject]
    public class LimitsPropertiesParameters : PropertiesParameters
    {
        [JsonProperty(PropertyName = "limits")]
        public ListLimits Limits { get; set; }
    }

    [JsonObject]
    public class LimitsSortPropertiesParameters : LimitsPropertiesParameters
    {
        [JsonProperty(PropertyName = "sort")]
        public ListSort Sort { get; set; }
    }

    [JsonObject]
    public class FilteredPropertiesParameters : LimitsSortPropertiesParameters
    {
        [JsonProperty(PropertyName = "filter",
        DefaultValueHandling = DefaultValueHandling.Ignore,
        NullValueHandling = NullValueHandling.Ignore)]
        public Filter Filter { get; set; }
    }

    [JsonObject]
    public class Filter
    {
        //TODO: Add Filtering

        [JsonProperty(PropertyName = "artistid",
        DefaultValueHandling = DefaultValueHandling.Ignore,
        NullValueHandling = NullValueHandling.Ignore)]
        public int? ArtistId { get; set; }

        [JsonProperty(PropertyName = "artist",
        DefaultValueHandling = DefaultValueHandling.Ignore,
        NullValueHandling = NullValueHandling.Ignore)]
        public string Artist { get; set; }

        [JsonProperty(PropertyName = "albumid",
        DefaultValueHandling = DefaultValueHandling.Ignore,
        NullValueHandling = NullValueHandling.Ignore)]
        public int? AlbumId { get; set; }

        [JsonProperty(PropertyName = "album",
        DefaultValueHandling = DefaultValueHandling.Ignore,
        NullValueHandling = NullValueHandling.Ignore)]
        public string Album { get; set; }

        [JsonProperty(PropertyName = "genreid",
        DefaultValueHandling = DefaultValueHandling.Ignore,
        NullValueHandling = NullValueHandling.Ignore)]
        public int? GenreId { get; set; }
    }

    #region GetAlbums / GetSongs

    [JsonObject]
    public sealed class AlbumFieldsParameters : PropertiesParameters
    {
        [JsonProperty(PropertyName = "albumid")]
        public int AlbumId { get; set; }
    }

    [JsonObject]
    public sealed class ArtistFieldsParameters : PropertiesParameters
    {
        [JsonProperty(PropertyName = "artistid")]
        public int ArtistId { get; set; }
    }

    [JsonObject]
    public sealed class ArtistsParameters : FilteredPropertiesParameters
    {
        [JsonProperty(PropertyName = "albumartistsonly")]
        public bool? AlbumArtistsOnly { get; set; }
    }

    #endregion

    [JsonObject]
    public class ScanParameters : Parameters
    {
        [JsonProperty(PropertyName = "directory")]
        public string Directory { get; set; }
    }

    #region VideoLibrary

    [JsonObject]
    public sealed class GetEpidodeParameters : PropertiesParameters
    {
        [JsonProperty(PropertyName = "episodeid")]
        public int EpisodeId { get; set; }
    }

    [JsonObject]
    public sealed class GetMovieSetParameters : PropertiesParameters
    {
        [JsonProperty(PropertyName = "setid")]
        public int SetId { get; set; }
    }

    [JsonObject]
    public sealed class GetMovieParameters : PropertiesParameters
    {
        [JsonProperty(PropertyName = "movieid")]
        public int MovieId { get; set; }
    }

    [JsonObject]
    public sealed class GetMusicVideoParameters : PropertiesParameters
    {
        [JsonProperty(PropertyName = "musicvideoid")]
        public int MusicVideoId { get; set; }
    }

    [JsonObject]
    public sealed class GetTVShowParameters : PropertiesParameters
    {
        [JsonProperty(PropertyName = "tvshowid")]
        public int TvShowId { get; set; }
    }

    [JsonObject]
    public class GetEpidodesParameters : GetSeasonsParameters
    {
        [JsonProperty(PropertyName = "season")]
        public int Season { get; set; }
    }

    [JsonObject]
    public class GetSeasonsParameters : FilteredPropertiesParameters
    {
        [JsonProperty(PropertyName = "tvshowid")]
        public int TvShowId { get; set; }
    }

    [JsonObject]
    public class VideoGenreParameters : LimitsSortPropertiesParameters
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
    }

    #endregion

    #region PlayerPosition

    [JsonObject]
    public abstract class PlayerPositionParameters : PlayerParameters
    {

    }

    [JsonObject]
    public sealed class PlayerPositionPercentageParameters : PlayerPositionParameters
    {
        [JsonProperty(PropertyName = "value")]
        public double Value { get; set; }
    }

    [JsonObject]
    public sealed class PlayerPositionTimeParameters : PlayerPositionParameters
    {
        [JsonProperty(PropertyName = "value")]
        public GlobalTime Value { get; set; }
    }

    [JsonObject]
    public sealed class PlayerPositionEnumParameters : PlayerPositionParameters
    {
        /// <remarks>enum {smallforward, smallbackward, bigforward, bigbackward}</remarks>
        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }
    }

    #endregion

    [JsonObject]
    public sealed class PlaylistIdParameters : Parameters
    {
        [JsonProperty(PropertyName = "playlistid")]
        public int PlaylistId { get; set; }
    }

    [JsonObject]
    public sealed class InputActionParameters : Parameters
    {
        [JsonProperty(PropertyName = "action")]
        public string Action { get; set; }
    }

    [JsonObject]
    public sealed class GuiWindowParameters : Parameters
    {
        [JsonProperty(PropertyName = "window")]
        public string Window { get; set; }
    }

    [JsonObject]
    public sealed class FilesDownloadParameters : Parameters
    {
        [JsonProperty(PropertyName = "path")]
        public string Path { get; set; }
    }

    [JsonObject]
    public sealed class InputSendTextParameters : Parameters
    {
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "done")]
        public bool Done { get; set; }
    }

    [JsonObject]
    public sealed class XbmcLabelsParameters : Parameters
    {
        [JsonProperty(PropertyName = "labels")]
        public string[] Labels { get; set; }
    }

    #region Volume Parameters

    [JsonObject]
    public sealed class MuteParameters : Parameters
    {
        [JsonProperty(PropertyName = "mute")]
        public bool Mute { get; set; }
    }

    [JsonObject]
    public abstract class VolumeParameters : Parameters
    {

    }

    [JsonObject]
    public sealed class VolumeValueParameters : VolumeParameters
    {
        [JsonProperty(PropertyName = "volume")]
        public int Volume { get; set; }
    }

    [JsonObject]
    public sealed class VolumeDirectionParameters : VolumeParameters
    {
        [JsonProperty(PropertyName = "volume")]
        public string Volume { get; set; }
    }

    #endregion
}