using FundaHomework.Adapter;
using FundaHomework.DB;
using FundaHomework.UseCase;
using FundaHomework.View;
using System;
using System.Net.Http;

namespace FundaHomework
{
    class Program
    {
        static void Main(string[] args)
        {
            var topTen = new TopTen(); // ViewModel
            var presenter = new TopTenPresenter(topTen); // Presenter, updates ViewModel
            var fundaTopCalculator = new StatCalculator(new Repository(new HttpClient()), presenter); // Use Case, passes result to presenter
            var view = new TopTenView(topTen); // View prints top ten

            // Controller would be empty, so we call calculator directly.
            Console.Out.WriteLine("Top ten properties:");
            fundaTopCalculator.CalculateTopTen();
            view.Display();
            Console.Out.WriteLine("Top ten properties with garden:");
            fundaTopCalculator.CalculateTopTenWithGArden();
            view.Display();
        }
    }
}
