using Domain.AggregatesModel.ChartAggregate;
using Domain.AggregatesModel.InstrumentsAggregate;
using Newtonsoft.Json;
using System;
using System.IO;
using Xunit;

namespace FunctionalTests.Simulators
{
    using Domain.AggregatesModel.AccountAggregate;
    using Domain.Strategies;
    using StrategySimulator;

    public class BuyOnStartDaySellOnEndStrategyTests
    {
        private Chart _chart;
        private Chart _longerChart;
        public BuyOnStartDaySellOnEndStrategyTests()
        {
            using (StreamReader r = new StreamReader(@"Simulators\SeedData\BuyOnStartDay.json"))
            {
                string json = r.ReadToEnd();
                _chart = JsonConvert.DeserializeObject<Chart>(json);
            }
            using (StreamReader r = new StreamReader(@"Simulators\SeedData\BuyOnStartDay2.json"))
            {
                string json = r.ReadToEnd();
                _longerChart = JsonConvert.DeserializeObject<Chart>(json);
            }
        }
        [Fact]
        public void Should_return_2_entries_and_1_exit_when_simulate_strategy()
        {
            var account = new Account(1000, "Test user");
            var simulator = new Simulator();
            var instrument = new Instrument("DE30", CategoryName.Index, "EUR", new Valuation(10000, 10001), new TimeSpan(02, 00, 00));
            instrument.SetContractSize(25);
            instrument.SetLeverage(20);
            instrument.SetPrecision(1);
            instrument.SetSpread(1);
            var buyOnEndStrategy = new BuyOnEndDayStrategy(instrument, account, 1000);
            var result = simulator.Simulate(_chart, buyOnEndStrategy, account);

            Assert.Equal(2, result.Entries.Count);
            Assert.Single(result.Exits);
        }
        [Fact]
        public void Should_generate_minus_729point76_profit_and_minus_675point7_profit_pips_and_effectivness_0_when_try_to_play_with_strategy_open_on_start_close_on_end()
        {
            var account = new Account(1000, "Test user");
            var simulator = new Simulator();
            var instrument = new Instrument("DE30", CategoryName.Index, "EUR", new Valuation(10000, 10001), new TimeSpan(02, 00, 00));
            instrument.SetContractSize(25);
            instrument.SetLeverage(20);
            instrument.SetPrecision(1);
            instrument.SetSpread(1);
            var buyOnEndStrategy = new BuyOnEndDayStrategy(instrument, account, 1000);
            var result = simulator.Simulate(_chart, buyOnEndStrategy, account);

            Assert.Equal(-729.76M, result.Profit);
            Assert.Equal(-675.7M, result.ProfitPips);
            Assert.Equal(0, result.Effectiveness);
        }

        [Fact]
        public void Should_generate_minus_2073point60_profit_and_minus_1920point0_profit_pips_and_effectivness_40_when_try_to_play_with_strategy_open_on_start_close_on_end()
        {
            var account = new Account(1000, "Test user");
            var simulator = new Simulator();
            var instrument = new Instrument("DE30", CategoryName.Index, "EUR", new Valuation(10000, 10001), new TimeSpan(02, 00, 00));
            instrument.SetContractSize(25);
            instrument.SetLeverage(20);
            instrument.SetPrecision(1);
            instrument.SetSpread(1);
            var buyOnEndStrategy = new BuyOnEndDayStrategy(instrument, account, 1000);
            var result = simulator.Simulate(_longerChart, buyOnEndStrategy, account);

            Assert.Equal(-2073.60M, result.Profit);
            Assert.Equal(-1920.0M, result.ProfitPips);
            Assert.Equal(40, result.Effectiveness);
        }
    }
}
