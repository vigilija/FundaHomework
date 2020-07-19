using FundaHomework;
using System.Collections.Generic;
using Xunit;

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
}
