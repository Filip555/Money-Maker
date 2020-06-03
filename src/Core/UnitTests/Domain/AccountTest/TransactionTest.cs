using Domain.AggregatesModel.AccountAggregate;
using Domain.AggregatesModel.InstrumentsAggregate;
using System;
using Xunit;

namespace DomainTests.Domain.AccountTest
{
    public class TransactionTest
    {
        [Fact]
        public void Should_return_3_pips_profit_diffrent_when_open_price_is_12267point8_and_close_price_is_12270point8_on_index_DE30_and_type_transaction_is_buy()
        {
            var volume = 0.01M;
            var instrument = CreateDe30Instrument();
            var openPrice = 12267.8;
            var position = new Position(openPrice, volume);
            var transaction = new Transaction(0, instrument, position, TypeTransaction.Buy, DateTime.Now, "");
            var closePrice = 12270.8;

            transaction.CloseTransaction(closePrice, DateTime.Now);

            Assert.Equal(3, transaction.ProfitPips);
        }

        [Fact]
        public void Should_return_minus_2_profit_pips_diffrent_when_open_price_is_1point12468_and_close_price_is_1point12488_on_index_EURUSD_and_type_transaction_is_sell()
        {
            var volume = 0.01M;
            var instrument = CreateEurUsdInstrument();
            var openPrice = 1.12468;
            var position = new Position(openPrice, volume);
            var transaction = new Transaction(0, instrument, position, TypeTransaction.Sell, DateTime.Now, "");
            var closePrice = 1.12488;

            transaction.CloseTransaction(closePrice, DateTime.Now);

            Assert.Equal(-2, transaction.ProfitPips);
        }

        [Fact]
        public void Should_return_2_profit_pips_diffrent_when_open_price_is_145point293_and_close_price_is_145point313_on_index_GBPJPY_and_type_transaction_is_buy()
        {
            var volume = 0.01M;
            var instrument = CreateGbpJpyInstrument();
            var openPrice = 145.293;
            var position = new Position(openPrice, volume);
            var transaction = new Transaction(0, instrument, position, TypeTransaction.Buy, DateTime.Now, "");
            var closePrice = 145.313;

            transaction.CloseTransaction(closePrice, DateTime.Now);

            Assert.Equal(2, transaction.ProfitPips);
        }
        [Fact]
        public void Should_return_0point1_profit_pips_diffrent_when_open_price_is_2914point4_and_close_price_is_2914dot5_on_index_US500_and_type_transaction_is_buy()
        {
            var volume = 0.01M;
            var instrument = CreateUs500Instrument();
            var openPrice = 2914.4;
            var position = new Position(openPrice, volume);
            var transaction = new Transaction(0, instrument, position, TypeTransaction.Buy, DateTime.Now, "");
            var closePrice = 2914.5;

            transaction.CloseTransaction(closePrice, DateTime.Now);

            var diffrentInPips = 0.1M;
            Assert.Equal(diffrentInPips, transaction.ProfitPips);
        }

        [Fact]
        public void Should_return_3point24_profit_when_diffrent_in_pips_is_3_and_value_of_one_pips_is_1point08_on_index_DE30_and_type_transaction_is_buy()
        {
            var valueOfOnePips = 1.08M;
            var volume = 0.01M;
            var instrument = CreateDe30Instrument();
            var openPrice = 12267.8;
            var position = new Position(openPrice, volume);
            var transaction = new Transaction(0, instrument, position, TypeTransaction.Buy, DateTime.Now, "");
            var closePrice = 12270.8;
            transaction.CloseTransaction(closePrice, DateTime.Now);

            var profit = valueOfOnePips * 3;
            Assert.Equal(profit, transaction.Profit);
        }

        [Fact]
        public void Should_return_0point76_profit_when_diffrent_in_pips_is_2_and_value_of_one_pips_is_0point38_on_index_EURUSD_and_type_transaction_is_buy()
        {
            var valueOfOnePips = 0.38M;
            var volume = 0.01M;
            var instrument = CreateEurUsdInstrument();
            var openPrice = 1.12468;
            var position = new Position(openPrice, volume);
            var transaction = new Transaction(0, instrument, position, TypeTransaction.Buy, DateTime.Now, "");
            var closePrice = 1.12488;
            transaction.CloseTransaction(closePrice, DateTime.Now);

            var profit = valueOfOnePips * 2;
            Assert.Equal(profit, transaction.Profit);
        }
        [Fact]
        public void Should_return_0point32_profit_when_diffrent_in_pips_is_1_and_value_of_one_pips_is_0point32_on_index_GBPJPY_and_type_transaction_is_buy()
        {
            var valueOfOnePips = 0.32M;
            var volume = 0.01M;
            var instrument = CreateGbpJpyInstrument();
            var openPrice = 132.667;
            var position = new Position(openPrice, volume);
            var transaction = new Transaction(0, instrument, position, TypeTransaction.Buy, DateTime.Now, "");
            var closePrice = 132.677;

            transaction.CloseTransaction(closePrice, DateTime.Now);

            var profit = valueOfOnePips * 1;
            Assert.Equal(profit, transaction.Profit);
        }


        [Fact]
        public void Should_return_3point8_profit_when_diffrent_in_pips_is_2_and_value_of_one_pips_is_1dot97_on_index_US500_and_type_transaction_is_buy()
        {
            var valueOfOnePips = 1.97M;
            var volume = 0.01M;
            var instrument = CreateUs500Instrument();
            var openPrice = 2914.4;
            var position = new Position(openPrice, volume);
            var transaction = new Transaction(0, instrument, position, TypeTransaction.Buy, DateTime.Now, "");
            var closePrice = 2916.4;

            transaction.CloseTransaction(closePrice, DateTime.Now);

            var profit = Math.Round(valueOfOnePips * 2, 2);
            Assert.Equal(profit, transaction.Profit);
        }

        [Fact]
        public void Should_return_open_price_be_open_price_plus_spread_when_make_transaction_buy_cuz_buy_from_ask_price()
        {
            var volume = 0.01M;
            var instrument = CreateDe30Instrument();
            var openPrice = 12267.8;
            var position = new Position(openPrice, volume);
            var transaction = new Transaction(0, instrument, position, TypeTransaction.Buy, DateTime.Now, "");

            var openPriceAfterSpread = openPrice;
            Assert.Equal(openPriceAfterSpread, transaction.Position.OpenPrice);
        }

        [Fact]
        public void Should_return_1562309580000_ticks_when_convert_open_date_time_from_05_07_2019_8_53_00()
        {
            var volume = 0.01M;
            var instrument = CreateDe30Instrument();
            var openPrice = 12267.8;
            var position = new Position(openPrice, volume);
            var transaction = new Transaction(0, instrument, position, TypeTransaction.Buy, Convert.ToDateTime("2019-07-05 08:53:00"), "");

            Assert.Equal(1562309580000, transaction.GetTickFromOpenLocalTimeString());
        }

        [Fact]
        public void Should_return_1562309580000_ticks_when_convert_close_date_time_from_06_07_2019_8_53_00()
        {
            var volume = 0.01M;
            var instrument = CreateDe30Instrument();
            var openPrice = 12267.8;
            var position = new Position(openPrice, volume);
            var transaction = new Transaction(0, instrument, position, TypeTransaction.Buy, Convert.ToDateTime("2019-07-06 08:53:00"), "");
            transaction.CloseTransaction(0, Convert.ToDateTime("2019-07-06 08:53:00"));

            Assert.Equal(1562395980000, transaction.GetTickFromCloseLocalTimeString());
        }

        [Fact]
        public void Should_return_3point24_when_open_price_is_12278point_8_and_close_price_is_12270point8_and_volume_is_0point01_on_index_DE30()
        {
            var volume = 0.01M;
            var instrument = CreateDe30Instrument();
            var openPrice = 12267.8;
            var position = new Position(openPrice, volume);
            var transaction = new Transaction(0, instrument, position, TypeTransaction.Buy, DateTime.Now, "");
            var closePrice = 12270.8;

            var profitInPips = transaction.ComputeProfitInPips(closePrice);

            Assert.Equal(3, profitInPips);
        }

        [Fact]
        public void Should_return_2_when_open_price_is_1point12468_and_close_price_is_1point12488_and_volume_is_0point01_on_index_EURUSD()
        {
            var volume = 0.01M;
            var instrument = CreateEurUsdInstrument();
            var openPrice = 1.12468;
            var position = new Position(openPrice, volume);
            var transaction = new Transaction(0, instrument, position, TypeTransaction.Buy, DateTime.Now, "");
            var closePrice = 1.12488;

            var profitInPips = transaction.ComputeProfitInPips(closePrice);

            Assert.Equal(2, profitInPips);
        }
        [Fact]
        public void Should_return_1_when_open_price_is_132point667_and_close_price_is_132point677_and_volume_is_0point01_on_index_GBPJPY()
        {
            var volume = 0.01M;
            var instrument = CreateGbpJpyInstrument();
            var openPrice = 132.667;
            var position = new Position(openPrice, volume);
            var transaction = new Transaction(0, instrument, position, TypeTransaction.Buy, DateTime.Now, "");
            var closePrice = 132.677;

            var profitInPips = transaction.ComputeProfitInPips(closePrice);

            Assert.Equal(1, profitInPips);
        }


        [Fact]
        public void Should_return_3point8_when_open_price_is_2914point4_and_close_price_is_2916point4_and_volume_is_0point01_on_index_US500()
        {
            var volume = 0.01M;
            var instrument = CreateUs500Instrument();
            var openPrice = 2914.4;
            var position = new Position(openPrice, volume);
            var transaction = new Transaction(0, instrument, position, TypeTransaction.Buy, DateTime.Now, "");
            var closePrice = 2916.4;

            var computeProfit = transaction.ComputeProfitInPips(closePrice);

            Assert.Equal(2, computeProfit);
        }

        [Fact]
        public void Should_return_100_profit_when_set_100_to_profit()
        {
            var profit = 100;
            var volume = 0.01M;
            var instrument = CreateUs500Instrument();
            var openPrice = 2914.4;
            var position = new Position(openPrice, volume);
            var transaction = new Transaction(0, instrument, position, TypeTransaction.Buy, DateTime.Now, "");

            transaction.SetProfit(profit);

            Assert.Equal(100, transaction.Profit);
        }

        [Fact]
        public void Should_return_true_when_transaction_is_open()
        {
            var volume = 0.01M;
            var instrument = CreateUs500Instrument();
            var openPrice = 2914.4;
            var position = new Position(openPrice, volume);
            var transaction = new Transaction(0, instrument, position, TypeTransaction.Buy, DateTime.Now, "");

            var isOpen = transaction.IsOpen();

            Assert.True(isOpen);
        }

        [Fact]
        public void Should_return_false_when_transaction_is_close()
        {
            var volume = 0.01M;
            var instrument = CreateUs500Instrument();
            var openPrice = 2914.4;
            var position = new Position(openPrice, volume);
            var transaction = new Transaction(0, instrument, position, TypeTransaction.Buy, DateTime.Now, "");
            var closePrice = 2916.4;

            transaction.CloseTransaction(closePrice, DateTime.Now);
            var isOpen = transaction.IsOpen();

            Assert.False(isOpen);
        }

        private Instrument CreateUs500Instrument()
        {
            var symbol = "US500";
            var categoryName = CategoryName.Index;
            var contractSize = 50;
            var currency = "USD";
            var laverage = 5;
            var precision = 1;
            var spread = 1;
            var bidPrice = 2914.4;
            var askPrice = 2915.4;
            var valuation = new Valuation(bidPrice, askPrice);
            var instrument = new Instrument(symbol, categoryName, currency, valuation, new TimeSpan(02, 00, 00));
            instrument.SetContractSize(contractSize);
            instrument.SetLeverage(laverage);
            instrument.SetPrecision(precision);
            instrument.SetSpread(spread);
            return instrument;
        }

        private Instrument CreateDe30Instrument()
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
            return instrument;
        }

        private Instrument CreateEurUsdInstrument()
        {
            var symbol = "EURUSD";
            var categoryName = CategoryName.Forex;
            var contractSize = 100000;
            var bidPrice = 1.36818;
            var askPrice = 1.36828;
            var currency = "EUR";
            var laverage = 5;
            var precision = 5;
            var spread = 1;
            var valuation = new Valuation(bidPrice, askPrice);
            var instrument = new Instrument(symbol, categoryName, currency, valuation, new TimeSpan(02, 00, 00));
            instrument.SetContractSize(contractSize);
            instrument.SetLeverage(laverage);
            instrument.SetPrecision(precision);
            instrument.SetSpread(spread);
            return instrument;
        }

        private Instrument CreateGbpJpyInstrument()
        {
            var symbol = "GBPJPY";
            var categoryName = CategoryName.Forex;
            var contractSize = 100000;
            var bidPrice = 158.503;
            var askPrice = 158.513;
            var currency = "EUR";
            var laverage = 5;
            var precision = 3;
            var spread = 1;
            var valuation = new Valuation(bidPrice, askPrice);
            var instrument = new Instrument(symbol, categoryName, currency, valuation, new TimeSpan(02, 00, 00));
            instrument.SetContractSize(contractSize);
            instrument.SetLeverage(laverage);
            instrument.SetPrecision(precision);
            instrument.SetSpread(spread);
            return instrument;
        }
    }
}
