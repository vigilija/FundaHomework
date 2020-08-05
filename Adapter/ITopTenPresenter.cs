using FundaHomework.Entity;
using System.Collections.Generic;

namespace FundaHomework.Adapter
{
    public interface ITopTenPresenter
    {
        public void SetTopTen(IList<BrokerStat> lists);
    }
}