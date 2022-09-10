using System.Text.Json;

namespace api.IpInfo
{
    public class IpInfoService : IIpInfoService
    {
        private HttpClient _httpClient;

        public IpInfoService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<IpInfoResponse> GetIpInfo(string ip)
        {
            if (string.IsNullOrEmpty(ip))
                throw new ArgumentNullException(nameof(ip));

            HttpResponseMessage responseMessage = await _httpClient.GetAsync($"https://ipinfo.io/{ip}/geo");
            string respone = await responseMessage.Content.ReadAsStringAsync();
            IpInfoResponse? ipInfo = JsonSerializer.Deserialize<IpInfoResponse?>(respone);

            if (ipInfo == null)
                throw new Exception("Could not retrieve IP info");

            return ipInfo;
        }
    }
}
