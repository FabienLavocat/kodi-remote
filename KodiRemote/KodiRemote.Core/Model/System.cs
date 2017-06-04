using Newtonsoft.Json;

namespace KodiRemote.Core.Model
{
    [JsonObject]
    public sealed class SystemPropertyValue
    {
        [JsonProperty(PropertyName = "canshutdown")]
        public int CanShutdown { get; set; }

        [JsonProperty(PropertyName = "canhibernate")]
        public int CanHibernate { get; set; }

        [JsonProperty(PropertyName = "cansuspend")]
        public int CanSuspend { get; set; }

        [JsonProperty(PropertyName = "canreboot")]
        public int CanReboot { get; set; }
    }
}