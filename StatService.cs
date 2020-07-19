using System;
using System.Collections.Generic;

namespace FundaHomework
{
    class StatService : IStatService
    {
        private Repository rep;
        private PropertyCountByBrokerCalculator statCalculator;

        public StatService(Repository _rep, PropertyCountByBrokerCalculator _statCalculator)
        {
            rep = _rep;
            statCalculator = _statCalculator;
        }

        public void PrintTopTen()
        {
            var page = 1;
            var c = 0;
            var calculateStats = new PropertyCountByBrokerCalculator();

            while (true)
            {
                c++;
                var fundaResponse = rep.GetPropertiesAsync(page).GetAwaiter().GetResult();
                if (fundaResponse.Objects.Count < 25 || fundaResponse == null) { break; }
                calculateStats.AddProperties(fundaResponse.Objects);
                System.Threading.Thread.Sleep(600);
                page++;
            };

            page = 1;
            c = 0;
            var calculateStatsWithGarden = new PropertyCountByBrokerCalculator();

            while (true)
            {
                c++;
                var fundaResponseWithGarden = rep.GetPropertiesWithGardenAsync(page).GetAwaiter().GetResult();
                if (fundaResponseWithGarden.Objects.Count < 25 || fundaResponseWithGarden == null) { break; }
                calculateStatsWithGarden.AddProperties(fundaResponseWithGarden.Objects);
                System.Threading.Thread.Sleep(600);
                page++;
            };

            var topTen = calculateStats.GetTopTen();
            var topTenWithGArden = calculateStatsWithGarden.GetTopTen();

            Console.WriteLine("Top ten");
            PrintResults(topTen);

            Console.WriteLine("Top ten with garden");
            PrintResults(topTenWithGArden);
        }

        public void PrintResults(IList<BrokerStat> list)
        {
            foreach (var broker in list)
            {
                Console.WriteLine($"Makler Id: {broker.Id}\t Makler name: {broker.Name}\t Property Count: {broker.PropertyCount}");
            }
        }
    }
}
