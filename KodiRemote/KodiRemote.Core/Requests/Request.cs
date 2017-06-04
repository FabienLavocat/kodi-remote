using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using KodiRemote.Core.Base;
using KodiRemote.Core.Responses;
using Newtonsoft.Json;

namespace KodiRemote.Core.Requests
{
    internal sealed class Request
    {
        private readonly Connection _xbmc;

        internal Request(Connection xbmc)
        {
            _xbmc = xbmc;
        }

        internal async Task<T> SendRequestAsync<T>(string methodName, int timeoutSeconds = 30)
            where T : ResponseMessageBase
        {
            var methodMessage = new MethodMessage
                                    {
                                        Method = methodName
                                    };

            return await SendRequestAsync<T>(methodMessage, timeoutSeconds);
        }

        internal async Task<T> SendRequestAsync<T>(MethodMessage methodMessage, int timeoutSeconds = 30)
            where T : ResponseMessageBase
        {
            int id = RandomNumber.GetRandomNumber(1, int.MaxValue);
            methodMessage.Id = id;
            methodMessage.JsonRpc = "2.0";

            string serialization = JsonConvert.SerializeObject(methodMessage, Formatting.None);
            string resultStr;

            using (var handler = new HttpClientHandler())
            {
                if (!string.IsNullOrWhiteSpace(_xbmc.Login))
                    handler.Credentials = new NetworkCredential(_xbmc.Login, _xbmc.Password);

                var httpClient = new HttpClient(handler)
                                 {
                                     Timeout = TimeSpan.FromSeconds(timeoutSeconds)
                                 };

                var request = new HttpRequestMessage(HttpMethod.Post, _xbmc.BaseUrl)
                              {
                                  Content = new StringContent(serialization, Encoding.UTF8, "application/json")
                              };

                try
                {
                    HttpResponseMessage response = await httpClient.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    resultStr = await response.Content.ReadAsStringAsync();
                }
                catch (TaskCanceledException)
                {
                    throw new TimeoutException(methodMessage.Method);
                }
            }
            
            var result = JsonConvert.DeserializeObject<T>(resultStr);

            if (result.Id != methodMessage.Id)
                throw new RequestException(methodMessage.Method, "The Id received does not match the one that was sent.");

            if (result.Error != null)
                throw new RequestException(methodMessage.Method, result.Error.ToString());

            return result;
        }
    }
}