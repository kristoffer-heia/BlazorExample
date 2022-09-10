using System.Text.Json.Serialization;

namespace api.Ipify
{
    public interface IIpifyService
    {
        public Task<IpifyResponse> CurrentIpAddress();
    }

    public class IpifyResponse
    {
        [JsonPropertyName("ip")]
        public string? Ip { get; set; }
    }
}
