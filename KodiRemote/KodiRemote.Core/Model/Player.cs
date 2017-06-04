using Newtonsoft.Json;

namespace KodiRemote.Core.Model
{
    [JsonObject]
    public class PlayerAudioStream
    {
        [JsonProperty(PropertyName = "index")]
        public int Index { get; set; }

        [JsonProperty(PropertyName = "language")]
        public string Language { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }

    [JsonObject]
    public sealed class PlayerAudioStreamExtended : PlayerAudioStream
    {
        [JsonProperty(PropertyName = "channels")]
        public int Channels { get; set; }

        [JsonProperty(PropertyName = "codec")]
        public string Codec { get; set; }

        [JsonProperty(PropertyName = "bitrate")]
        public int Bitrate { get; set; }
    }

    [JsonObject]
    public sealed class PlayerPositionTime : GlobalTime
    {
    }

    [JsonObject]
    public sealed class PlayerPropertyValue
    {
        [JsonProperty(PropertyName = "canrepeat")]
        public bool CanRepeat { get; set; }

        [JsonProperty(PropertyName = "canmove")]
        public bool CanMove { get; set; }

        [JsonProperty(PropertyName = "canshuffle")]
        public bool CanShuffle { get; set; }

        [JsonProperty(PropertyName = "speed")]
        public int Speed { get; set; }

        [JsonProperty(PropertyName = "percentage")]
        public double Percentage { get; set; }

        [JsonProperty(PropertyName = "playlistid")]
        public int PlaylistId { get; set; }

        [JsonProperty(PropertyName = "repeat")]
        public string Repeat { get; set; }

        [JsonProperty(PropertyName = "currentsubtitle")]
        public PlayerSubtitle CurrentSubTitle { get; set; }

        [JsonProperty(PropertyName = "canrotate")]
        public bool CanRotate { get; set; }

        [JsonProperty(PropertyName = "canzoom")]
        public bool CanZoom { get; set; }

        [JsonProperty(PropertyName = "canchangespeed")]
        public bool CanChangeSpeed { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "partymode")]
        public bool PartyMode { get; set; }

        [JsonProperty(PropertyName = "subtitles")]
        public PlayerSubtitle[] SubTitles { get; set; }

        [JsonProperty(PropertyName = "canseek")]
        public bool CanSeek { get; set; }

        [JsonProperty(PropertyName = "time")]
        public GlobalTime Time { get; set; }

        [JsonProperty(PropertyName = "totaltime")]
        public GlobalTime TotalTime { get; set; }

        [JsonProperty(PropertyName = "currentaudiostream")]
        public PlayerAudioStreamExtended CurrentAudioStream { get; set; }

        [JsonProperty(PropertyName = "live")]
        public bool Live { get; set; }

        [JsonProperty(PropertyName = "subtitleenabled")]
        public bool SubtitleEnabled { get; set; }
    }

    [JsonObject]
    public sealed class PlayerSubtitle
    {
        [JsonProperty(PropertyName = "index")]
        public int Index { get; set; }

        [JsonProperty(PropertyName = "language")]
        public string Language { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }

    [JsonObject]
    public sealed class Player
    {
        [JsonProperty(PropertyName = "playerid")]
        public int PlayerId { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
    }
}