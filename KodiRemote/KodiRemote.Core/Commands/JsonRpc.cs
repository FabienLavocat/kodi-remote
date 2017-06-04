using System.Threading.Tasks;
using KodiRemote.Core.Model;
using KodiRemote.Core.Requests;
using KodiRemote.Core.Responses;

namespace KodiRemote.Core.Commands
{
    public sealed class JsonRpc : CommandCollection
    {
        private readonly Request _request;

        internal JsonRpc(Connection xbmc)
        {
            _request = new Request(xbmc);
        }

        /// <summary>Ping responder.</summary>
        public async Task<bool> PingAsync()
        {
            try
            {
                var result = await _request.SendRequestAsync<BasicResponseMessage<string>>("JSONRPC.Ping", 10);
                return result.Result.ToUpperInvariant() == "PONG";
            }
            catch
            {
                return false;
            }
        }

        /// <summary>Retrieve the JSON-RPC protocol version.</summary>
        public async Task<JsonRpcVersion> VersionAsync()
        {
            var result = await _request.SendRequestAsync<BasicResponseMessage<JsonRpcVersion>>("JSONRPC.Version");
            return result.Result;
        }
    }
}