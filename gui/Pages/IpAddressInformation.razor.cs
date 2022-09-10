using Microsoft.AspNetCore.Components;
using api.Ipify;
using api.IpInfo;
using System.Reflection;

namespace gui.Pages
{
    public partial class IpAddressInformation : ComponentBase
    {
        private readonly Dictionary<string, string> _ipInfo = new();
        private string? error;
        private bool? isLoading;

        [Inject]
        private IIpifyService IpifyService { get; set; }

        [Inject]
        private IIpInfoService IpInfoService { get; set; }

        public IpAddressInformation()
        {
            isLoading = true;
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                IpifyResponse ipifyResponse = await IpifyService.CurrentIpAddress();
                IpInfoResponse ipInfoResponse = await IpInfoService.GetIpInfo(ipifyResponse.Ip);
                SetAddressInformation(ipInfoResponse);

            }
            catch (Exception e)
            {
                error = e.Message;
            }

            isLoading = false;
        }

        private void SetAddressInformation(IpInfoResponse ipInfoResponse)
        {
            foreach (var prop in typeof(IpInfoResponse).GetProperties())
            {
                string? value = prop.GetValue(ipInfoResponse)?.ToString();
                _ipInfo.Add(prop.Name, value ?? "Not found");
            }
        }
    }
}
