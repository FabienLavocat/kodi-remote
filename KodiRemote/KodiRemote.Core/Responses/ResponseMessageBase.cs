using Newtonsoft.Json;

namespace KodiRemote.Core.Responses
{
    [JsonObject]
    public abstract class ResponseMessageBase
    {
        [JsonProperty(PropertyName = "jsonrpc")]
        public string JsonRpc { get; set; }

        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "error")]
        public ErrorMessage Error { get; set; }
    }

    [JsonObject]
    public sealed class ErrorMessage
    {
        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        public override string ToString()
        {
            return string.Concat(Code, ":", Message);
        }
    }
}