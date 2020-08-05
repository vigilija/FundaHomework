using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Moq.Protected;
using FundaHomework.Entity;
using FundaHomework.UseCase;
using FundaHomework.DB;
using FundaHomework.Adapter;

namespace FundaHomeworkTest
{
    public class PropertyCountByBrokerCalculatorTest
    {
        [Fact]
        public void TestLessThanTenProperties()
        {
            var properties = new List<Property>() { new Property() { GlobalId = 1, BrokerId = 1, BrokerName = "Broker name1" },
                                                    new Property() { GlobalId = 2, BrokerId = 2, BrokerName = "Broker name2" },
                                                    new Property() { GlobalId = 3, BrokerId = 3, BrokerName = "Broker name3" },
                                                    new Property() { GlobalId = 4, BrokerId = 4, BrokerName = "Broker name4" },
                                                    new Property() { GlobalId = 5, BrokerId = 2, BrokerName = "Broker name2" }};


            var mockRepository = new Mock<IRepository>();
            mockRepository.Setup(s => s.GetAllProperties()).Returns(properties);
            var mockPresenter = new Mock<ITopTenPresenter>();

            IList<BrokerStat> result = null;

            mockPresenter.Setup(h => h.SetTopTen(It.IsAny<IList<BrokerStat>>())).Callback<IList<BrokerStat>>(r => result = r);

            var calculator = new StatCalculator(mockRepository.Object, mockPresenter.Object);

            calculator.CalculateTopTen();

            Assert.Equal(4, result.Count);
            Assert.Contains(result, x => x.Name == "Broker name1" && x.PropertyCount == 1);
            Assert.Contains(result, x => x.Name == "Broker name2" && x.PropertyCount == 2);
        }

        [Fact]
        public void TestMoreThanTenBrokers()
        {
            var properties = new List<Property>();
            for (var i = 0; i < 20; i++)
            {
                properties.Add(new Property() { GlobalId = i, BrokerId = i, BrokerName = $"Broker name{i}" });
            }

            var mockRepository = new Mock<IRepository>();
            mockRepository.Setup(s => s.GetAllProperties()).Returns(properties);
            var mockPresenter = new Mock<ITopTenPresenter>();

            IList<BrokerStat> result = null;

            mockPresenter.Setup(h => h.SetTopTen(It.IsAny<IList<BrokerStat>>())).Callback<IList<BrokerStat>>(r => result = r);

            var calculator = new StatCalculator(mockRepository.Object, mockPresenter.Object);

            calculator.CalculateTopTen();

            Assert.Equal(10, result.Count);

        }
    }

    public class ServiceCallTest
    {
        [Fact]
        public void TestRepositoryResponceObjectCorrectness()
        {
            var testClass = new Repository(new HttpClient(new NewClass()));
            var result = testClass.GetAllProperties();
            Assert.Equal(1, result.Count);
            Assert.Equal(5586465, result[0].GlobalId);
            Assert.Equal(24614, result[0].BrokerId);
        }

        [Fact]
        public void TestRepositoryRetryOnErrorCode()
        {
            var badResponse = Task.FromResult(new HttpResponseMessage()
            { StatusCode = HttpStatusCode.BadRequest });
            
            var response = Task.FromResult(new HttpResponseMessage()
            { StatusCode = HttpStatusCode.OK, Content = new StringContent(@"{""Objects"": [{""GlobalId"": 5586465,""MakelaarId"": 24614,""MakelaarNaam"": ""Nieuw West Makelaardij B.V.""}]}") });

            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler.Protected()
                       .SetupSequence<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                       .Returns(badResponse)
                       .Returns(response);

            var testRepository = new Repository(new HttpClient(mockHandler.Object));

            var result = testRepository.GetAllProperties();

            mockHandler.Protected().Verify("SendAsync", Times.Exactly(2), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());

            Assert.Equal(1, result.Count);
            Assert.Equal(5586465, result[0].GlobalId);
            Assert.Equal(24614, result[0].BrokerId);
        }
    }
    public class NewClass : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new HttpResponseMessage()
            { StatusCode = HttpStatusCode.OK, Content = new StringContent(@"{""Objects"": [{""GlobalId"": 5586465,""MakelaarId"": 24614,""MakelaarNaam"": ""Nieuw West Makelaardij B.V.""}]}") });
        }
    }
}
