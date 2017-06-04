using System;
using System.Linq;
using System.Threading.Tasks;
using KodiRemote.Core.Model;
using KodiRemote.Core.Requests;
using KodiRemote.Core.Responses;

namespace KodiRemote.Core.Commands
{
    public sealed class System : CommandCollection
    {
        private readonly Request _request;

        internal System(Connection xbmc)
        {
            _request = new Request(xbmc);
        }

        /// <summary>Ejects or closes the optical disc drive (if available).</summary>
        public async Task EjectOpticalDriveAsync()
        {
            await _request.SendRequestAsync<BasicResponseMessage<string>>("System.EjectOpticalDrive");
        }

        /// <summary>Retrieves the values of the given properties.</summary>
        public async Task<SystemPropertyValue> GetPropertiesAsync(params PropertyNames[] propertyName)
        {
            string[] items = propertyName.Select(p => p.ToString().ToLowerInvariant()).ToArray();

            if (!items.Any())
                items = Enum.GetNames(typeof(PropertyNames));

            var method = new ParameteredMethodMessage<ItemsParameters>
                             {
                                 Method = "System.GetProperties",
                                 Parameters = new ItemsParameters { Items = items }
                             };

            var result = await _request.SendRequestAsync<BasicResponseMessage<SystemPropertyValue>>(method);
            return result.Result;
        }

        /// <summary>Puts the system running XBMC into hibernate mode.</summary>
        public async Task HibernateAsync()
        {
            await _request.SendRequestAsync<BasicResponseMessage<string>>("System.Hibernate");
        }

        /// <summary>Reboots the system running XBMC.</summary>
        public async Task RebootAsync()
        {
            await _request.SendRequestAsync<BasicResponseMessage<string>>("System.Reboot");
        }

        /// <summary>Shuts the system running XBMC down.</summary>
        public async Task ShutdownAsync()
        {
            await _request.SendRequestAsync<BasicResponseMessage<string>>("System.Shutdown");
        }

        /// <summary>Suspends the system running XBMC.</summary>
        public async Task SuspendAsync()
        {
            await _request.SendRequestAsync<BasicResponseMessage<string>>("System.Suspend");
        }
    }
}