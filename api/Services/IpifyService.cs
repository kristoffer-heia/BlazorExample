using System.Text.Json;

namespace api.Ipify
{
    public class IpifyService : IIpifyService
    {
        private readonly HttpClient _httpClient;

        public IpifyService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<IpifyResponse> CurrentIpAddress()
        {
            HttpResponseMessage responseMessage = await _httpClient
                .GetAsync("https://api.ipify.org/?format=json");

            string response = await responseMessage.Content.ReadAsStringAsync();

            IpifyResponse? ipify = JsonSerializer.Deserialize<IpifyResponse?>(response);

            if (ipify == null)
                throw new Exception("Could not retrieve cuttent IP address");

            return ipify;
        }
    }
}
