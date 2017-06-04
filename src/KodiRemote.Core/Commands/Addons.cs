using System;
using System.Linq;
using System.Threading.Tasks;
using KodiRemote.Core.Model;
using KodiRemote.Core.Requests;
using KodiRemote.Core.Responses;

namespace KodiRemote.Core.Commands
{
    public sealed class Addons : CommandCollection
    {
        private readonly Request _request;

        internal Addons(Connection xbmc)
        {
            _request = new Request(xbmc);
        }

        /// <summary>Gets the details of a specific addon.</summary>
        public async Task<AddonDetailsBase> GetAddonDetailsAsync(string addonid, params AddonFields[] fields)
        {
            string[] items = fields.Select(p => p.ToString().ToLowerInvariant()).ToArray();

            if (!items.Any())
                items = Enum.GetNames(typeof(AddonFields));

            var method = new ParameteredMethodMessage<AddonDetailsParameters>
                {
                    Method = "Addons.GetAddonDetails",
                    Parameters = new AddonDetailsParameters
                        {
                            AddonId = addonid,
                            Properties = items
                        }
                };

            var result = await _request.SendRequestAsync<BasicResponseMessage<AddonBase>>(method);
            return result.Result.Addon;
        }

        /// <summary>Gets all available addons.</summary>
        public async Task<AddonsResponse> GetAddonsAsync(AddonTypes addonType = AddonTypes.unknown,
                                                         AddonContent addonContent = AddonContent.unknown,
                                                         AddonEnabled addonEnabled = AddonEnabled.all,
                                                         int start = 0, int end = int.MaxValue,
                                                         params AddonFields[] fields)
        {
            string[] properties = fields.Select(p => p.ToString().ToLowerInvariant()).ToArray();

            if (!properties.Any())
                properties = Enum.GetNames(typeof(AddonFields));

            var method = new ParameteredMethodMessage<GetAddonsParameters>
                {
                    Method = "Addons.GetAddons",
                    Parameters = new GetAddonsParameters
                        {
                            Properties = properties,
                            Enabled = addonEnabled.ToString(),
                            Type = addonType.ToString(),
                            Content = addonContent.ToString().Replace("_", "."),
                            Limits = new ListLimits {Start = start, End = end}
                        }
                };

            var result = await _request.SendRequestAsync<BasicResponseMessage<AddonsResponse>>(method);
            return result.Result;
        }

        /// <summary>Enables/Disables a specific addon.</summary>
        public async Task<string> SetAddonEnabledAsync(string addonid, bool enabled)
        {
            var method = new ParameteredMethodMessage<SetAddonEnabledParameters>
                {
                    Method = "Addons.SetAddonEnabled",
                    Parameters = new SetAddonEnabledParameters
                        {
                            AddonId = addonid,
                            Enabled = enabled,
                        }
                };

            var result = await _request.SendRequestAsync<BasicResponseMessage<string>>(method);
            return result.Result;
        }
    }
}