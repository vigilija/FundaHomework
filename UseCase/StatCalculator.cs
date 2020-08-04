using FundaHomework.Adapter;
using FundaHomework.DB;
using FundaHomework.Entity;
using System;
using System.Collections.Generic;

namespace FundaHomework.UseCase
{
    class StatCalculator : IStatCalculator
    {
        private IRepository rep;
        private PropertyCountByBrokerCalculator statCalculator;
        private TopTenPresenter presenter;

        public StatCalculator(IRepository _rep, PropertyCountByBrokerCalculator _statCalculator, TopTenPresenter _presenter)
        {
            rep = _rep;
            statCalculator = _statCalculator;
            presenter = _presenter;
        }

        public void CalculateTopTen()
        {
            var calculateStats = new PropertyCountByBrokerCalculator();
            var allProperties = rep.GetAllProperties();

            presenter.SetTopTen(calculateStats.GetTopTen(allProperties));

         /*   
            var calculateStatsWithGarden = new PropertyCountByBrokerCalculator();
            while (true)
            {
                var fundaResponseWithGarden = rep.GetPropertiesWithGardenAsync(page).GetAwaiter().GetResult();
                if (fundaResponseWithGarden.Objects.Count < 25 || fundaResponseWithGarden == null) { break; }
                calculateStatsWithGarden.AddProperties(fundaResponseWithGarden.Objects);
                System.Threading.Thread.Sleep(600);
                page++;
            };*/

        }
    }
}
