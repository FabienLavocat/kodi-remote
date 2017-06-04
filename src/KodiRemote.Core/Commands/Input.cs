using System.Threading.Tasks;
using KodiRemote.Core.Requests;
using KodiRemote.Core.Responses;

namespace KodiRemote.Core.Commands
{
    public sealed class Input : CommandCollection
    {
        private readonly Request _request;

        internal Input(Connection xbmc)
        {
            _request = new Request(xbmc);
        }

        /// <summary>Goes back in GUI.</summary>
        public async Task BackAsync()
        {
            await _request.SendRequestAsync<BasicResponseMessage<string>>("Input.Back");
        }

        /// <summary>Shows the context menu.</summary>
        public async Task ContextMenuAsync()
        {
            await _request.SendRequestAsync<BasicResponseMessage<string>>("Input.ContextMenu");
        }

        /// <summary>Navigate down in GUI.</summary>
        public async Task DownAsync()
        {
            await _request.SendRequestAsync<BasicResponseMessage<string>>("Input.Down");
        }

        /// <summary>Execute a specific action.</summary>
        public async Task<string> ExecuteActionAsync(InputActions inputAction)
        {
            var method = new ParameteredMethodMessage<InputActionParameters>
                             {
                                 Method = "Input.ExecuteAction",
                                 Parameters = new InputActionParameters {Action = inputAction.ToString()}
                             };

            var result = await _request.SendRequestAsync<BasicResponseMessage<string>>(method);
            return result.Result;
        }

        /// <summary>Goes to home window in GUI.</summary>
        public async Task HomeAsync()
        {
            await _request.SendRequestAsync<BasicResponseMessage<string>>("Input.Home");
        }

        /// <summary>Shows the information dialog.</summary>
        public async Task InfoAsync()
        {
            await _request.SendRequestAsync<BasicResponseMessage<string>>("Input.Info");
        }

        /// <summary>Navigate left in GUI.</summary>
        public async Task LeftAsync()
        {
            await _request.SendRequestAsync<BasicResponseMessage<string>>("Input.Left");
        }

        /// <summary>Navigate right in GUI.</summary>
        public async Task RightAsync()
        {
            await _request.SendRequestAsync<BasicResponseMessage<string>>("Input.Right");
        }

        /// <summary>Select current item in GUI.</summary>
        public async Task SelectAsync()
        {
            await _request.SendRequestAsync<BasicResponseMessage<string>>("Input.Select");
        }

        /// <summary>Send a generic (unicode) text.</summary>
        /// <param name="text">Text to send.</param>
        /// <param name="done">Whether this is the whole input or not (closes an open input dialog if true).</param>
        public async Task SendTextAsync(string text, bool done = true)
        {
            var method = new ParameteredMethodMessage<InputSendTextParameters>
                             {
                                 Method = "Input.SendText",
                                 Parameters = new InputSendTextParameters
                                                  {
                                                      Text = text,
                                                      Done = done
                                                  }
                             };

            await _request.SendRequestAsync<BasicResponseMessage<string>>(method);
        }

        /// <summary>Show codec information of the playing item.</summary>
        public async Task ShowCodecAsync()
        {
            await _request.SendRequestAsync<BasicResponseMessage<string>>("Input.ShowCodec");
        }

        /// <summary>Show the on-screen display for the current player.</summary>
        public async Task ShowOSDAsync()
        {
            await _request.SendRequestAsync<BasicResponseMessage<string>>("Input.ShowOSD");
        }

        /// <summary>Navigate up in GUI.</summary>
        public async Task UpAsync()
        {
            await _request.SendRequestAsync<BasicResponseMessage<string>>("Input.Up");
        }
    }
}