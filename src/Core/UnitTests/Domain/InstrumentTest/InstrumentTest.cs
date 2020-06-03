using Xunit;

namespace DomainTests.Aggregates.InstrumentTest
{
    using global::Domain.AggregatesModel.InstrumentsAggregate;
    using System;

    public class InstrumentTest
    {
        [Fact]
        public void Should_return_0dot02_volumen_can_buy_when_have_1500_pln_on_index_DE30()
        {
            var symbol = "DE30";
            var categoryName = CategoryName.Index;
            var contractSize = 25;
            var bidPrice = 12265.3;
            var askPrice = 12267.8;
            var currency = "EUR";
            var laverage = 5;
            var precision = 1;
            var spread = 1;

            var valuation = new Valuation(bidPrice, askPrice);

            var instrument = new Instrument(symbol, categoryName, currency, valuation, new TimeSpan(02, 00, 00));
            instrument.SetContractSize(contractSize);
            instrument.SetLeverage(laverage);
            instrument.SetPrecision(precision);
            instrument.SetSpread(spread);

            var volumeToBuy = instrument.ComputeVolumeToBuyByMaxPrice(1500, instrument.Valuation.BidPrice);

            Assert.Equal(0.02M, volumeToBuy);
        }

        [Fact]
        public void Should_return_0dot10_volumen_can_buy_when_have_1500_pln_on_index_EURUSD()
        {
            var symbol = "EURUSD";
            var categoryName = CategoryName.Forex;
            var contractSize = 100000;
            var bidPrice = 1.12451;
            var askPrice = 1.1246;
            var currency = "EUR";
            var laverage = 3.33;
            var precision = 5;
            var spread = 1;

            var valuation = new Valuation(bidPrice, askPrice);
            var instrument = new Instrument(symbol, categoryName, currency, valuation, new TimeSpan(02, 00, 00));
            instrument.SetContractSize(contractSize);
            instrument.SetLeverage(laverage);
            instrument.SetPrecision(precision);
            instrument.SetSpread(spread);

            var volumeToBuy = instrument.ComputeVolumeToBuyByMaxPrice(1500, instrument.Valuation.BidPrice);

            Assert.Equal(0.10M, volumeToBuy);
        }

        [Fact]
        public void Should_return_value_of_one_pips_equal_to_1dot075_when_give_0dot01_volument_on_index_DE30()
        {
            var symbol = "DE30";
            var categoryName = CategoryName.Index;
            var contractSize = 25;
            var bidPrice = 12265.3;
            var askPrice = 12267.8;
            var currency = "EUR";
            var laverage = 5;
            var precision = 1;
            var spread = 1;

            var valuation = new Valuation(bidPrice, askPrice);
            var instrument = new Instrument(symbol, categoryName, currency, valuation, new TimeSpan(02, 00, 00));
            instrument.SetContractSize(contractSize);
            instrument.SetLeverage(laverage);
            instrument.SetPrecision(precision);
            instrument.SetSpread(spread);

            var valueOfOnePips = instrument.ComputeValueOfOnePips(0.01M, instrument.Valuation.BidPrice);

            Assert.Equal(1.08, valueOfOnePips);
        }

        [Fact]
        public void Should_return_value_of_one_pips_equal_to_656dot34_when_give_0dot01_volument_on_index_EURUSD()
        {
            var symbol = "EURUSD";
            var categoryName = CategoryName.Forex;
            var contractSize = 100000;
            var bidPrice = 1.12451;
            var askPrice = 1.1246;
            var currency = "EUR";
            var laverage = 3.33;
            var precision = 5;
            var spread = 1;

            var valuation = new Valuation(bidPrice, askPrice);
            var instrument = new Instrument(symbol, categoryName, currency, valuation, new TimeSpan(02, 00, 00));
            instrument.SetContractSize(contractSize);
            instrument.SetLeverage(laverage);
            instrument.SetPrecision(precision);
            instrument.SetSpread(spread);

            var valueOfOnePips = instrument.ComputeValueOfOnePips(0.01M, instrument.Valuation.BidPrice);

            Assert.Equal(0.38, valueOfOnePips);
        }

        [Fact]
        public void Should_return_695dot26_value_of_tranaction_when_give_index_de30()
        {
            var symbol = "DE30";
            var categoryName = CategoryName.Index;
            var contractSize = 25;
            var bidPrice = 12265.3;
            var askPrice = 12267.8;
            var currency = "EUR";
            var laverage = 5;
            var precision = 1;
            var spread = 1;

            var valuation = new Valuation(bidPrice, askPrice);
            var instrument = new Instrument(symbol, categoryName, currency, valuation, new TimeSpan(02, 00, 00));
            instrument.SetContractSize(contractSize);
            instrument.SetLeverage(laverage);
            instrument.SetPrecision(precision);
            instrument.SetSpread(spread);

            var valueTransaction = instrument.ComputeValueTransaction(0.01M, instrument.Valuation.BidPrice);

            Assert.Equal(659.26M, valueTransaction);
        }
    }
}
