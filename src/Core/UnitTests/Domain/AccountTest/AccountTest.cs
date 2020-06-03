using Domain.AggregatesModel.AccountAggregate;
using Domain.AggregatesModel.InstrumentsAggregate;
using Domain.Exceptions;
using System;
using System.Linq;
using Xunit;

namespace DomainTests.Domain.AccountTest
{
    public class AccountTest
    {
        [Fact]
        public void Should_return_3dot_21_profit_when_diffrent_in_pips_is_3_and_one_value_pips_is_1dot07_on_index_DE30()
        {
            var interval = "M1";
            var valueOfOnePips = 1.08M;
            var volumen = 0.01M;
            var openPrice = 12267.8;
            var instrument = CreateDe30Instrument();
            var closePrice = 12270.8;
            var deposite = 0;
            var account = new Account(deposite, "Test user");
            var transactionId = 1;
            account.AddTransaction(transactionId, instrument, openPrice, null, null, volumen, TypeTransaction.Buy, DateTime.Now, interval);
            account.CloseTransaction(transactionId, closePrice, DateTime.Now);

            var profit = valueOfOnePips * 3;
            Assert.Equal(profit, account.Deposite);
        }

        [Fact]
        public void Should_return_minus_3dot_21_profit_when_diffrent_in_pips_is_3_and_one_value_pips_is_1dot07_on_index_DE30()
        {
            var interval = "M1";
            var valueOfOnePips = 1.08M;
            var volumen = 0.01M;
            var openPrice = 12267.8;
            var closePrice = 12270.8;
            var instrument = CreateDe30Instrument();
            var transactionId = 1;
            var deposite = 0;
            var account = new Account(deposite, "Test user");
            account.AddTransaction(transactionId, instrument, openPrice, null, null, volumen, TypeTransaction.Sell, DateTime.Now, interval);
            account.CloseTransaction(transactionId, closePrice, DateTime.Now);

            var profit = -valueOfOnePips * 3;
            Assert.Equal(profit, account.Deposite);
        }

        [Fact]
        public void Should_order_id_be_1_when_order_id_is()
        {
            var interval = "M1";
            var volumen = 0.01M;
            var openPrice = 12267.8;
            var instrument = CreateDe30Instrument();
            var deposite = 0;
            var account = new Account(deposite, "Test user");
            account.AddTransaction(0, instrument, openPrice, null, null, volumen, TypeTransaction.Sell, DateTime.Now, interval);

            var transaction = account.Transactions.FirstOrDefault(x => x.Instrument.Symbol == "DE30");
            Assert.Equal(1, transaction.OrderId);
        }

        [Fact]
        public void Should_order_id_be_2_when_max_order_id_is_1_and_add_new_transaction()
        {
            var interval = "M1";
            var volumen = 0.01M;
            var openPrice = 12267.8;
            var instrumentDe = CreateDe30Instrument();
            var instrumentUs = CreateUs500Instrument();
            var deposite = 0;
            var account = new Account(deposite, "Test user");
            account.AddTransaction(0, instrumentDe, openPrice, null, null, volumen, TypeTransaction.Sell, DateTime.Now, interval);
            account.AddTransaction(0, instrumentUs, openPrice, null, null, volumen, TypeTransaction.Sell, DateTime.Now, interval);

            var transaction = account.Transactions.FirstOrDefault(x => x.Instrument.Symbol == "US500");
            Assert.Equal(2, transaction.OrderId);
        }

        [Fact]
        public void Should_order_id_be_5_when_transaction_has_order_id_5()
        {
            var interval = "M1";
            var volumen = 0.01M;
            var openPrice = 12267.8;
            var instrumentDe = CreateDe30Instrument();
            var deposite = 0;
            var account = new Account(deposite, "Test user");
            account.AddTransaction(5, instrumentDe, openPrice, null, null, volumen, TypeTransaction.Sell, DateTime.Now, interval);

            var transaction = account.Transactions.FirstOrDefault(x => x.Instrument.Symbol == "DE30");
            Assert.Equal(5, transaction.OrderId);
        }

        [Fact]
        public void Should_return_closed_transaction_count_2_when_open_5_transactions()
        {
            var interval = "M1";
            var volumen = 0.01M;
            var openPrice = 12267.8;
            var instrumentDe = CreateDe30Instrument();
            var instrumentUs = CreateUs500Instrument();
            var instrumentEurUsd = CreateEurUsdInstrument();
            var instrumentGbpJpy = CreateGbpJpyInstrument();
            var deposite = 1000;
            var account = new Account(deposite, "Test user");
            account.AddTransaction(1, instrumentDe, openPrice, null, null, volumen, TypeTransaction.Sell, DateTime.Now, interval);
            account.CloseTransaction(1, openPrice, DateTime.Now);
            account.AddTransaction(2, instrumentUs, openPrice, null, null, volumen, TypeTransaction.Sell, DateTime.Now, interval);
            account.CloseTransaction(2, openPrice, DateTime.Now);
            account.AddTransaction(3, instrumentEurUsd, openPrice, null, null, volumen, TypeTransaction.Sell, DateTime.Now, interval);
            account.AddTransaction(4, instrumentGbpJpy, openPrice, null, null, volumen, TypeTransaction.Sell, DateTime.Now, interval);
            account.AddTransaction(5, instrumentDe, openPrice, null, null, volumen, TypeTransaction.Sell, DateTime.Now, interval);

            var openTransactions = account.GetClosedTransactions();
            Assert.Equal(2, openTransactions.Count);
            Assert.Equal(5, account.Transactions.Count);
        }

        [Fact]
        public void Should_throw_domain_exception_message_is_not_possible_to_close_transaction_twice_when_try_to_close_transaction_twice()
        {
            var interval = "M1";
            var volumen = 0.01M;
            var openPrice = 12267.8;
            var instrumentDe = CreateDe30Instrument();
            var deposite = 1000;
            var account = new Account(deposite, "Test user");

            account.AddTransaction(1, instrumentDe, openPrice, null, null, volumen, TypeTransaction.Sell, DateTime.Now, interval);
            account.CloseTransaction(1, openPrice, DateTime.Now);
            var ex = Assert.Throws<DomainException>(() => account.CloseTransaction(1, openPrice, DateTime.Now));

            Assert.Equal("Is not possible to close transaction twice.", ex.Message);
        }

        [Fact]
        public void Should_return_de30_when_de30_is_open_transaction_and_us500_is_close_transaction()
        {
            var interval = "M1";
            var volumen = 0.01M;
            var openPrice = 12267.8;
            var instrumentDe = CreateDe30Instrument();
            var instrumentUs = CreateUs500Instrument();
            var deposite = 1000;
            var account = new Account(deposite, "Test user");
            account.AddTransaction(1, instrumentDe, openPrice, null, null, volumen, TypeTransaction.Sell, DateTime.Now, interval);
            account.AddTransaction(2, instrumentUs, openPrice, null, null, volumen, TypeTransaction.Sell, DateTime.Now, interval);
            account.CloseTransaction(2, openPrice, DateTime.Now);

            var openTransactions = account.GetOpenTransaction("DE30");
            Assert.Equal("DE30", openTransactions.Instrument.Symbol);
        }
        [Fact]
        private void Should_stop_loss_10010_when_give_10010_stop_loss()
        {
            var interval = "M1";
            var volumen = 0.01M;
            var openPrice = 12267.8;
            var instrumentDe = CreateDe30Instrument();
            var deposite = 1000;
            var account = new Account(deposite, "Test user");
            account.AddTransaction(1, instrumentDe, openPrice, 10010, null, volumen, TypeTransaction.Sell, DateTime.Now, interval);

            var openTransactions = account.GetOpenTransaction("DE30");
            Assert.Equal(10010, openTransactions.Position.StopLoss);
        }
        [Fact]
        private void Should_take_profit_10010_when_give_10010_take_profit()
        {
            var interval = "M1";
            var volumen = 0.01M;
            var openPrice = 9000;
            var instrumentDe = CreateDe30Instrument();
            var deposite = 1000;
            var account = new Account(deposite, "Test user");

            account.AddTransaction(1, instrumentDe, openPrice, null, 10010, volumen, TypeTransaction.Sell, DateTime.Now, interval);

            var openTransactions = account.GetOpenTransaction("DE30");
            Assert.Equal(10010, openTransactions.Position.TakeProfit);
        }

        [Fact]
        public void Should_return_null_when_de30_is_close_transaction_and_us500_is_close_transaction()
        {
            var interval = "M1";
            var volumen = 0.01M;
            var openPrice = 12267.8;
            var instrumentDe = CreateDe30Instrument();
            var instrumentUs = CreateUs500Instrument();
            var deposite = 1000;
            var account = new Account(deposite, "Test user");
            account.AddTransaction(1, instrumentDe, openPrice, null, null, volumen, TypeTransaction.Sell, DateTime.Now, interval);
            account.AddTransaction(2, instrumentUs, openPrice, null, null, volumen, TypeTransaction.Sell, DateTime.Now, interval);
            account.CloseTransaction(2, openPrice, DateTime.Now);
            account.CloseTransaction(1, openPrice, DateTime.Now);

            var openTransactions = account.GetOpenTransaction("DE30");
            Assert.Null(openTransactions?.Instrument?.Symbol);
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
