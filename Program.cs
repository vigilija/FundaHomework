using System.Net.Http;

namespace FundaHomework
{
    class Program
    {
        static void Main(string[] args)
        {
            var fundaTopCalculatorService = new StatService(new Repository(new HttpClient()), new PropertyCountByBrokerCalculator());
            fundaTopCalculatorService.PrintTopTen();
        }
    }
}
