using System.Collections.Generic;

namespace FundaHomework
{
    public interface IStatService
    {
        public void PrintTopTen();
        void PrintResults(IList<BrokerStat> list);
    }
}