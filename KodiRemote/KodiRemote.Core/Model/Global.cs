using Newtonsoft.Json;

namespace KodiRemote.Core.Model
{
    [JsonObject]
    public class GlobalTime
    {
        [JsonProperty(PropertyName = "hours")]
        public int Hours { get; set; }

        [JsonProperty(PropertyName = "minutes")]
        public int Minutes { get; set; }

        [JsonProperty(PropertyName = "seconds")]
        public int Seconds { get; set; }

        [JsonProperty(PropertyName = "milliseconds")]
        public int Milliseconds { get; set; }

        public int TotalMilliseconds()
        {
            return Hours*60*60*1000
                   + Minutes*60*1000
                   + Seconds*1000
                   + Milliseconds;
        }
    }
}