using FundaHomework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Moq.Protected;

namespace FundaHomeworkTest
{
    public class PropertyCountByBrokerCalculatorTest
    {
        [Fact]
        public void TestLessThanTenProperties()
        {
            var properties = new List<Property>() { new Property() { GlobalId = 1, MakelaarId = 1, MakelaarNaam = "Broker name1" },
                                                    new Property() { GlobalId = 2, MakelaarId = 2, MakelaarNaam = "Broker name2" },
                                                    new Property() { GlobalId = 3, MakelaarId = 3, MakelaarNaam = "Broker name3" },
                                                    new Property() { GlobalId = 4, MakelaarId = 4, MakelaarNaam = "Broker name4" },
                                                    new Property() { GlobalId = 5, MakelaarId = 2, MakelaarNaam = "Broker name2" }
            };
            var calculator = new PropertyCountByBrokerCalculator();
            calculator.AddProperties(properties);
            var result = calculator.GetTopTen();

            Assert.Equal(4, result.Count);
            Assert.Contains(result, x => x.Id == 1 && x.Name == "Broker name1" && x.PropertyCount == 1);
            Assert.Contains(result, x => x.Id == 2 && x.Name == "Broker name2" && x.PropertyCount == 2);
        }

        [Fact]
        public void TestMoreThanTenBrokers()
        {
            var properties = new List<Property>();
            for (var i = 0; i < 20; i++)
            {
                properties.Add(new Property() { GlobalId = i, MakelaarId = i, MakelaarNaam = $"Broker name{i}" });
            }

            var calculator = new PropertyCountByBrokerCalculator();
            calculator.AddProperties(properties);
            var result = calculator.GetTopTen();

            Assert.Equal(10, result.Count);

        }
    }

    public class ServiceCallTest
    {
        [Fact]
        public void TestRepositoryResponceObjectCorrectness()
        {
            var testClass = new Repository(new HttpClient(new NewClass()));
            var result = testClass.GetPropertiesAsync(1);
            Assert.Equal(1, result.Result.Objects.Count);
            Assert.Equal(5586465, result.Result.Objects[0].GlobalId);
            Assert.Equal(24614, result.Result.Objects[0].MakelaarId);
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

            var result = testRepository.GetPropertiesAsync(1);

            mockHandler.Protected().Verify("SendAsync", Times.Exactly(2), ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>());

            Assert.Equal(1, result.Result.Objects.Count);
            Assert.Equal(5586465, result.Result.Objects[0].GlobalId);
            Assert.Equal(24614, result.Result.Objects[0].MakelaarId);
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
