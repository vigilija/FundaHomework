using FundaHomework.Entity;
using System.Collections.Generic;

namespace FundaHomework.UseCase
{
    public interface IStatCalculator
    {
        public void CalculateTopTen();
        //void PrintResults(IList<BrokerStat> list);
    }
}