using FundaHomework.Adapter;
using System;

namespace FundaHomework.View
{
    internal class TopTenView
    {
        private TopTen topTen;

        public TopTenView(TopTen topTen)
        {
            this.topTen = topTen;
        }

        internal void Display()
        {
            foreach (var rec in topTen.ViewStats)
            {
                Console.WriteLine($"{rec.SequenceNumber}. Broker name: {rec.BrokerName}\t Property Count: {rec.PropertyCount}");
            }
        }
    }
}