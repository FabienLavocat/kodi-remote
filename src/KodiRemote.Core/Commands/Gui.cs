using System.Threading.Tasks;
using KodiRemote.Core.Requests;
using KodiRemote.Core.Responses;

namespace KodiRemote.Core.Commands
{
    public sealed class Gui : CommandCollection
    {
        private readonly Request _request;

        internal Gui(Connection xbmc)
        {
            _request = new Request(xbmc);
        }

        /// <summary>Activates the given window.</summary>
        public async Task ActivateWindowAsync(GuiWindow guiWindow)
        {
            var method = new ParameteredMethodMessage<GuiWindowParameters>
                {
                    Method = "GUI.ActivateWindow",
                    Parameters = new GuiWindowParameters { Window = guiWindow.ToString() }
                };

            await _request.SendRequestAsync<BasicResponseMessage<string>>(method);
        }
    }
}