using FundaHomework.Entity;
using FundaHomework.UseCase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;

namespace FundaHomework.DB
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

        public IList<Property> GetAllProperties()
        {
            var page = 0;
            var result = new List<Property>();
            var continueLoop = true;
            while (continueLoop)
            {
                //c++;
                var fundaResponse = GetProperties(string.Format(path, "", page));
                if (fundaResponse.Objects.Count < 25 || fundaResponse == null || page == 5) { continueLoop = false;}
                System.Threading.Thread.Sleep(600);
                result.AddRange(fundaResponse.Objects.Select(s => new Property {GlobalId = s.GlobalId, BrokerId = s.MakelaarId, BrokerName = s.MakelaarNaam }));
                page++;
            };
            return result;
        }

        public IList<Property> GetAllPropertiesWithGarden()
        {
            var page = 0;
            var result = new List<Property>();
            var continueLoop = true;
            while (continueLoop)
            {
                //c++;
                var fundaResponse = GetProperties(string.Format(path, garden, page));
                if (fundaResponse.Objects.Count < 25 || fundaResponse == null) { continueLoop = false; }
                System.Threading.Thread.Sleep(600);
                result.AddRange(fundaResponse.Objects.Select(s => new Property { GlobalId = s.GlobalId, BrokerId = s.MakelaarId, BrokerName = s.MakelaarNaam }));
                page++;
            };
            return result;
        }

        private FundaResponse GetProperties(string path)
        {
            for (var i = 0; i < 5; i++)
            {
                try
                {
                    HttpResponseMessage response = client.GetAsync(path).GetAwaiter().GetResult();
                    if (response.IsSuccessStatusCode)
                    {
                        var respString = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
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
