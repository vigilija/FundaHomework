using FundaHomework.Entity;
using System.Collections.Generic;
using System.Linq;

namespace FundaHomework.UseCase
{
    public class PropertyCountByBrokerCalculator : IPropertyCountByBrokerCalculator
    {
        private Dictionary<int, int> PropertyCountById = new Dictionary<int, int>();
        private Dictionary<int, string> BrokerNameById = new Dictionary<int, string>();

        public IList<BrokerStat> GetTopTen(IList<Property> propertyList)
        {
            foreach (var _property in propertyList)
            {
                var count = PropertyCountById.GetValueOrDefault(_property.BrokerId, 0);
                PropertyCountById[_property.BrokerId] = ++count;
                BrokerNameById[_property.BrokerId] = _property.BrokerName;
            } 
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
