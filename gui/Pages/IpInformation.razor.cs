using Microsoft.AspNetCore.Components;
using api.Ipify;
using api.IpInfo;
using System.Reflection;
using System.Text;

namespace gui.Pages
{
    record HistoryItem
    (
        string ipAddress,
        DateTime searchTime
    );

    public partial class IpInformation : ComponentBase
    {
        private readonly Dictionary<string, string> _ipInfo = new();
        private readonly List<HistoryItem> _history = new();
        private string? error;
        private string? searchInput;
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
                searchInput = ipInfoResponse.Ip;

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
            _history.Add(new HistoryItem(
                ipInfoResponse.Ip,
                DateTime.Now));

            foreach (var prop in typeof(IpInfoResponse).GetProperties())
            {
                string? value = prop.GetValue(ipInfoResponse)?.ToString();
                _ipInfo.Add(prop.Name, value ?? "Not found");
            }
        }

        private async Task Search(string ip)
        {
            if (string.IsNullOrEmpty(ip))
            {
                error = "Please provide a valid IP";
            }
            else
            {
                try
                {
                    IpInfoResponse ipInfoResponse = await IpInfoService.GetIpInfo(ip);
                    SetAddressInformation(ipInfoResponse);
                }
                catch (Exception e)
                {
                    error = e.Message;
                }
            }
        }

        private string RandomIpAddress()
        {
            Random random = new();
            const int ipSectionCount = 4;
            string[] ipSections = new string[ipSectionCount];

            const int sectionMinVale = 0;
            const int sectionMaxVale = 255;

            for (int i=0; i<ipSectionCount; i++)
            {
                int sectionValue = random.Next(sectionMinVale, sectionMaxVale);
                ipSections[i] = sectionValue.ToString();
            }

            return string.Join(".", ipSections);
        }
    }
}
