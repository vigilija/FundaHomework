using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace FundaHomework
{
    public class Repository: IRepository
    {
        private const string garden = "/tuin"; 
        private const string path = "http://partnerapi.funda.nl/feeds/Aanbod.svc/JSON/ac1b0b1572524640a0ecc54de453ea9f/?type=koop&zo=/amsterdam{0}/&page={1}&pagesize=25";
        HttpClient client;

        public Repository(HttpClient _client)
        {
            client = _client;
        }

        public async Task<FundaResponse> GetPropertiesAsync(int page)
        {
            return await GetPropertiesByPath(string.Format(path, "", page));        
        }

        public async Task<FundaResponse> GetPropertiesWithGardenAsync(int page)
        {
            return await GetPropertiesByPath(string.Format(path, garden, page));
        }

        public async Task<FundaResponse> GetPropertiesByPath(string path)
        {
            for (var i = 0; i < 5; i++)
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(path);
                    if (response.IsSuccessStatusCode)
                    {
                        var respString = await response.Content.ReadAsStringAsync();
                        var fundaResponce = JsonSerializer.Deserialize<FundaResponse>(respString);
                        return fundaResponce;
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("Http error: " + e);
                }
                System.Threading.Thread.Sleep(2000);
            }
            throw new Exception($"Failed to reach path {path} after five tries");
        }
    }
}
