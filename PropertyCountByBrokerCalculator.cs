using System.Collections.Generic;
using System.Linq;

namespace FundaHomework
{
    public class PropertyCountByBrokerCalculator : IPropertyCountByBrokerCalculator
    {
        private Dictionary<int, int> PropertyCountById = new Dictionary<int, int>();
        private Dictionary<int, string> BrokerNameById = new Dictionary<int, string>();

        public void AddProperties(IList<Property> propertyList)
        {
            foreach (var _property in propertyList)
            {
                var count = PropertyCountById.GetValueOrDefault(_property.MakelaarId, 0);
                PropertyCountById[_property.MakelaarId] = ++count;
                BrokerNameById[_property.MakelaarId] = _property.MakelaarNaam;
            }
        }

        public IList<BrokerStat> GetTopTen()
        {
            return PropertyCountById
                .OrderBy(kv => -kv.Value)
                .Take(10)
                .Select(kv => new BrokerStat() { Id = kv.Key, PropertyCount = kv.Value, Name = BrokerNameById[kv.Key]})
                .ToList();
        }

        public string GetBrokerNameById(int id)
        {
            return BrokerNameById.GetValueOrDefault(id);
        }
    }
}
