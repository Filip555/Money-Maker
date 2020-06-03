using System;
using System.Linq;

namespace Domain.Strategies
{
    using Domain.AggregatesModel.AccountAggregate;
    using Domain.AggregatesModel.ChartAggregate;
    using Domain.AggregatesModel.InstrumentsAggregate;

    public class GapStrategy : IStrategy
    {
        public Instrument Instrument { get; }

        public int MaxPriceOneTransaction { get; }

        public Account Account { get; }

        public GapStrategy(Instrument instrument, int maxPriceOneTransaction, Account account)
        {
            Instrument = instrument;
            MaxPriceOneTransaction = maxPriceOneTransaction;
            Account = account;
        }
        public void Play(Chart chart)
        {
            var highLastDay = chart.GetLastDayQuotations(DateTime.Now).Where(w => w.IsInEuropeTimeFrame())?.DefaultIfEmpty().Max(m => m?.High ?? 0) ?? 0;
            var lowLastDay = chart.GetLastDayQuotations(DateTime.Now).Where(x => x.IsInEuropeTimeFrame())?.DefaultIfEmpty().Min(m => m?.Low ?? 0) ?? 0;
            var lastQuotation = chart.GetLastQuotation();
            if (!TodayPlayedSymbol() && Account.GetOpenTransaction(chart.Symbol) != null)
            {
                if (lastQuotation.Close > (highLastDay + Instrument.Spread * Math.Pow(10, 0 - Instrument.Precision)))
                {
                    var volumen = Instrument.ComputeVolumeToBuyByMaxPrice(MaxPriceOneTransaction, Instrument.Valuation.BidPrice);
                    var openPriceToPrecision = lastQuotation.Close * Math.Pow(10, Instrument.Precision);
                    var closePriceToPrecision = highLastDay * Math.Pow(10, Instrument.Precision);
                    var takeProfit = Math.Abs(closePriceToPrecision - openPriceToPrecision) / 10d;
                    var stopLoss = takeProfit;

                    if (ComputeProcentOfProfit(Instrument, volumen, MaxPriceOneTransaction, takeProfit) >= 2)
                    {
                        Account.AddTransaction(1, Instrument, lastQuotation.Close, stopLoss, takeProfit, volumen, TypeTransaction.Sell, lastQuotation.Time, typeof(InsideBarStrategy).Name);
                    }
                }
                else if (lastQuotation.Close + Instrument.Spread * Math.Pow(10, 0 - Instrument.Precision) < lowLastDay)
                {
                    var volumen = Instrument.ComputeVolumeToBuyByMaxPrice(MaxPriceOneTransaction, Instrument.Valuation.BidPrice);
                    var openPriceToPrecision = lastQuotation.Close * Math.Pow(10, Instrument.Precision);
                    var closePriceToPrecision = lowLastDay * Math.Pow(10, Instrument.Precision);
                    var takeProfit = Math.Abs(closePriceToPrecision - openPriceToPrecision) / 10d;
                    var stopLoss = takeProfit;
                    if (ComputeProcentOfProfit(Instrument, volumen, MaxPriceOneTransaction, takeProfit) >= 2)
                    {
                        Account.AddTransaction(1, Instrument, lastQuotation.Close, stopLoss, takeProfit, volumen, TypeTransaction.Buy, lastQuotation.Time, typeof(InsideBarStrategy).Name);
                    }
                }
            }
        }
        private bool TodayPlayedSymbol()
        {
            var closedTransactions = Account.GetClosedTransactions();

            return closedTransactions.Any(x => x.Instrument.Symbol == Instrument.Symbol && x.DateClose?.Date == DateTime.Now.Date);
        }

        private double ComputeProcentOfProfit(Instrument instrument, decimal volumen, double amountOfCash, double takeProfit)
        {
            var valueOfOnePips = instrument.ComputeValueOfOnePips(volumen, instrument.Valuation.BidPrice);
            var valueOfTransactionProfit = valueOfOnePips * takeProfit;
            var percentOfProfit = valueOfTransactionProfit / amountOfCash * 100;

            return percentOfProfit;
        }
    }
}

