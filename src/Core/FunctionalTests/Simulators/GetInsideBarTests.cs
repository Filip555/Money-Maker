using System;
using Newtonsoft.Json;
using System.IO;
using Xunit;
using System.Collections.Generic;

namespace FunctionalTests.Simulators
{
    using Domain.AggregatesModel.ChartAggregate;

    public class GetInsideBarTests
    {
        private List<Chart> _charts;
        public GetInsideBarTests()
        {
            using (StreamReader r = new StreamReader(@"Simulators\SeedData\ResultGetInsideBarListChart.json"))
            {
                string json = r.ReadToEnd();
                _charts = JsonConvert.DeserializeObject<List<Chart>>(json);
            }
        }
        [Fact]
        public void Should_return_14_inside_bars_when_generate_inside_bars()
        {
            var insideBars = new List<InsideBar>();
            foreach (var chart in _charts)
            {
                chart.SetInsideBar(new DateTime(2020, 03, 18));
                if (chart.InsideBar != null)
                {
                    insideBars.Add(chart.InsideBar);
                }
            }
            Assert.Equal(14, insideBars.Count);
        }
    }
}
