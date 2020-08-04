using FundaHomework.Entity;
using System.Collections.Generic;

namespace FundaHomework.UseCase
{
    public interface IPropertyCountByBrokerCalculator
    {
        public string GetBrokerNameById(int id);
        public IList<BrokerStat> GetTopTen(IList<Property> propertyList);
    }
}