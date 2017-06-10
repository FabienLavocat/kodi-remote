using System.Collections.Generic;
using System.Threading.Tasks;
using KodiRemote.Core.Requests;
using KodiRemote.Core.Responses;

namespace KodiRemote.Core.Commands
{
    public sealed class Kodi : CommandCollection
    {
        private readonly Request _request;

        internal Kodi(Connection kodi)
        {
            _request = new Request(kodi);
        }

        /// <summary>Retrieve info labels about Kodi and the system.</summary>
        public async Task<Dictionary<string, string>> GetInfoLabelsAsync(string label, params string[] labels)
        {
            var lbls = new List<string>(labels);
            lbls.Insert(0, label);

            var method = new ParameteredMethodMessage<XbmcLabelsParameters>
                             {
                                 Method = "XBMC.GetInfoLabels",
                                 Parameters = new XbmcLabelsParameters { Labels = lbls.ToArray() }
                             };

            var result = await _request.SendRequestAsync<BasicResponseMessage<Dictionary<string, string>>>(method);
            return result.Result;
        }
    }
}