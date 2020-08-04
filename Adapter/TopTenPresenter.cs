using FundaHomework.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FundaHomework.Adapter
{
    internal class TopTenPresenter
    {
        private TopTen topTen;

        public TopTenPresenter(TopTen topTen)
        {
            this.topTen = topTen;
        }

        internal void SetTopTen(IList<BrokerStat> lists)
        {
            var i = 1;
            topTen.ViewStats = lists.Select(s => new ViewStat() {SequenceNumber = i++, BrokerName = s.Name, PropertyCount = s.PropertyCount }).ToList();
        }
    }
}