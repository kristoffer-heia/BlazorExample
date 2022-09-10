using Microsoft.AspNetCore.Components;
using api.Ipify;
using api.IpInfo;
using System.Reflection;

namespace gui.Pages
{
    public partial class IpInformation : ComponentBase
    {
        private readonly Dictionary<string, string> _ipInfo = new();
        private string? error;
        private string? search;
        private bool? isLoading;

        [Inject]
        private IIpifyService IpifyService { get; set; }

        [Inject]
        private IIpInfoService IpInfoService { get; set; }

        public IpInformation()
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
                search = ipInfoResponse.Ip;

            }
            catch (Exception e)
            {
                error = e.Message;
            }

            isLoading = false;
        }

        private void SetAddressInformation(IpInfoResponse ipInfoResponse)
        {
            _ipInfo.Clear();
            error = null;

            foreach (var prop in typeof(IpInfoResponse).GetProperties())
            {
                string? value = prop.GetValue(ipInfoResponse)?.ToString();
                _ipInfo.Add(prop.Name, value ?? "Not found");
            }
        }

        private async Task Search()
        {
            if (string.IsNullOrEmpty(search))
            {
                error = "Please provide a valid IP";
            }
            else
            {
                try
                {
                    IpInfoResponse ipInfoResponse = await IpInfoService.GetIpInfo(search);
                    SetAddressInformation(ipInfoResponse);
                }
                catch (Exception e)
                {
                    error = e.Message;
                }
            }
        }
    }
}
