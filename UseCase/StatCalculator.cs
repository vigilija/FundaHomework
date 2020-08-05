using FundaHomework.Adapter;
using FundaHomework.Entity;
using System.Collections.Generic;
using System.Linq;

namespace FundaHomework.UseCase
{
    public class StatCalculator : IStatCalculator
    {
        private IRepository rep;
        private ITopTenPresenter presenter;

        public StatCalculator(IRepository _rep, ITopTenPresenter _presenter)
        {
            rep = _rep;
            presenter = _presenter;
        }

        public void CalculateTopTen()
        {
            var allProperties = rep.GetAllProperties();

            presenter.SetTopTen(GetTopTen(allProperties));
        }

        public void CalculateTopTenWithGArden()
        {
            var withGardenProperties = rep.GetAllPropertiesWithGarden();

            presenter.SetTopTen(GetTopTen(withGardenProperties));
        }

        private IList<BrokerStat> GetTopTen(IList<Property> propertyList)
        {
            Dictionary<int, int> PropertyCountById = new Dictionary<int, int>();
            Dictionary<int, string> BrokerNameById = new Dictionary<int, string>();

            foreach (var _property in propertyList)
            {
                var count = PropertyCountById.GetValueOrDefault(_property.BrokerId, 0);
                PropertyCountById[_property.BrokerId] = ++count;
                BrokerNameById[_property.BrokerId] = _property.BrokerName;
            }
            return PropertyCountById
                .OrderBy(kv => -kv.Value)
                .Take(10)
                .Select(kv => new BrokerStat() { Id = kv.Key, PropertyCount = kv.Value, Name = BrokerNameById[kv.Key] })
                .ToList();
        }
    }
}
