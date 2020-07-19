using System.Collections.Generic;

namespace FundaHomework
{
    public interface IPropertyCountByBrokerCalculator
    {
        public void AddProperties(IList<Property> propertyList);
        public IList<BrokerStat> GetTopTen();
        public string GetBrokerNameById(int id);
    }
}