using System.Net.Http;
using System.Text.Json;

namespace api.Cat
{
    public interface ICatService
    {
        public Task<CatFact> GetCatFactAsync();
    }

    public class CatService : ICatService
    {
        private readonly HttpClient _httpClient;

        public CatService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<CatFact> GetCatFactAsync()
        {
            HttpResponseMessage responseMessage = await _httpClient.GetAsync("https://catfact.ninja/fact");
            string response = await responseMessage.Content.ReadAsStringAsync();
            CatFact? catFact = JsonSerializer.Deserialize<CatFact?>(response);

            if (catFact == null)
                throw new Exception("Could not get fact");

            return catFact;
        }
    }
}
