using System.Threading.Tasks;
using KodiRemote.Core.Model;
using KodiRemote.Core.Requests;
using KodiRemote.Core.Responses;

namespace KodiRemote.Core.Commands
{
    public sealed class Files : CommandCollection
    {
        private readonly Request _request;

        internal Files(Connection xbmc)
        {
            _request = new Request(xbmc);
        }

        /// <summary>Downloads the given file.</summary>
        public async Task<string> DownloadAsync(string path)
        {
            var method = new ParameteredMethodMessage<FilesDownloadParameters>
                             {
                                 Method = "Files.Download",
                                 Parameters = new FilesDownloadParameters {Path = path}
                             };

            var result = await _request.SendRequestAsync<BasicResponseMessage<string>>(method);
            return result.Result;
        }

        /// <summary>Provides a way to download a given file (e.g. providing an URL to the real file location).</summary>
        public async Task<PrepareDownload> PrepareDownloadAsync(string path)
        {
            var method = new ParameteredMethodMessage<FilesDownloadParameters>
                             {
                                 Method = "Files.PrepareDownload",
                                 Parameters = new FilesDownloadParameters {Path = path}
                             };

            var result = await _request.SendRequestAsync<SingleValueResponseMessage<PrepareDownload>>(method);
            return result.Result;
        }
    }
}