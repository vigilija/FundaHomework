using FundaHomework.Adapter;
using FundaHomework.DB;
using FundaHomework.UseCase;
using FundaHomework.View;
using System.Net.Http;

namespace FundaHomework
{
    class Program
    {
        static void Main(string[] args)
        {
            var topTen = new TopTen(); // ViewModel
            var presenter = new TopTenPresenter(topTen); // Presenter, updates ViewModel
            var fundaTopCalculator = new StatCalculator(new Repository(new HttpClient()), new PropertyCountByBrokerCalculator(), presenter); // Use Case, passes result to presenter
            var view = new TopTenView(topTen); // View prints top ten

            // Controller would be empty, so we call calculator directly.
            fundaTopCalculator.CalculateTopTen();
            view.Display();
           
        }
    }
}
