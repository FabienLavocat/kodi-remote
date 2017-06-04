using System;
using System.Linq;
using System.Threading.Tasks;
using KodiRemote.Core.Requests;
using KodiRemote.Core.Responses;

namespace KodiRemote.Core.Commands
{
    public sealed class Application : CommandCollection
    {
        private readonly Request _request;

        internal Application(Connection xbmc)
        {
            _request = new Request(xbmc);
        }

        /// <summary>Quit application.</summary>
        public async Task<ApplicationPropertiesResponseMessage> GetPropertiesAsync(params ApplicationPropertyName[] names)
        {
            string[] properties = names.Select(p => p.ToString().ToLowerInvariant()).ToArray();

            if (!properties.Any())
                properties = Enum.GetNames(typeof(ApplicationPropertyName));

            var method = new ParameteredMethodMessage<PropertiesParameters>
                             {
                                 Method = "Application.GetProperties",
                                 Parameters = new PropertiesParameters { Properties = properties }
                             };

            var item = await _request.SendRequestAsync<BasicResponseMessage<ApplicationPropertiesResponseMessage>>(method);
            return item.Result;
        }

        /// <summary>Quit application.</summary>
        public async Task QuitAsync()
        {
            await _request.SendRequestAsync<BasicResponseMessage<string>>("Application.Quit");
        }

        /// <summary>Downloads the given file.</summary>
        public async Task<bool> SetMuteAsync(bool mute)
        {
            var method = new ParameteredMethodMessage<MuteParameters>
                             {
                                 Method = "Application.SetMute",
                                 Parameters = new MuteParameters { Mute = mute }
                             };

            var result = await _request.SendRequestAsync<BasicResponseMessage<bool>>(method);
            return result.Result;
        }

        // TODO: use Global.Toggle [ enum { toggle } ]

        /// <summary>Set the current volume.</summary>
        public async Task<int> SetVolumeAsync(int volume)
        {
            var method = new ParameteredMethodMessage<VolumeParameters>
                             {
                                 Method = "Application.SetVolume",
                                 Parameters = new VolumeValueParameters { Volume = volume }
                             };

            var result = await _request.SendRequestAsync<BasicResponseMessage<int>>(method);
            return result.Result;
        }

        private const string VOLUME_INCREMENT = "increment";
        private const string VOLUME_DECREMENT = "decrement";

        /// <summary>Increment the current volume.</summary>
        public async Task<int> IncrementVolumeAsync()
        {
            return await SetVolumeAsync(VOLUME_INCREMENT);
        }

        /// <summary>Decrement the current volume.</summary>
        public async Task<int> DecrementVolumeAsync()
        {
            return await SetVolumeAsync(VOLUME_DECREMENT);
        }

        private async Task<int> SetVolumeAsync(string where)
        {
            var method = new ParameteredMethodMessage<VolumeDirectionParameters>
                             {
                                 Method = "Application.SetVolume",
                                 Parameters = new VolumeDirectionParameters { Volume = where }
                             };

            var result = await _request.SendRequestAsync<BasicResponseMessage<int>>(method);
            return result.Result;
        }
    }
}