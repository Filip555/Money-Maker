using System;
using Newtonsoft.Json;
using StrategySimulator;
using System.IO;
using Xunit;

namespace FunctionalTests.Simulators
{
    using Domain.AggregatesModel.AccountAggregate;
    using Domain.AggregatesModel.ChartAggregate;
    using Domain.AggregatesModel.InstrumentsAggregate;
    using Domain.Strategies;

    public class InsideBarTests
    {
        private Chart _goldChart;
        public InsideBarTests()
        {
            using (StreamReader r = new StreamReader(@"Simulators\SeedData\InsideBarQuotationM1Gold.json"))
            {
                string json = r.ReadToEnd();
                _goldChart = JsonConvert.DeserializeObject<Chart>(json);
            }
        }
        [Fact]
        public void Should_return_0_entries_when_simulate_inside_bar_strategy()
        {
            var simulator = new Simulator();
            var account = new Account(10000, "Test user");
            var instrument = new Instrument("GOLD", CategoryName.Index, "USD", new Valuation(1499.94, 1498.19), new TimeSpan(06, 00, 00));
            instrument.SetContractSize(100);
            instrument.SetLeverage(5);
            instrument.SetPrecision(2);
            instrument.SetSpread(1);
            var insideBar = new InsideBarStrategy(instrument, 1000, account);
            var history = simulator.Simulate(_goldChart, insideBar, account);
            Assert.Empty(history.Entries);
        }
    }
}
