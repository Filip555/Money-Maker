using System;
using System.Linq;

namespace Domain.Strategies
{
    using Domain.AggregatesModel.AccountAggregate;
    using Domain.AggregatesModel.ChartAggregate;
    using Domain.AggregatesModel.InstrumentsAggregate;

    public class InsideBarStrategy : IStrategy
    {
        public Instrument Instrument { get; private set; }
        public int MaxPriceOneTransaction { get; private set; }
        public Account Account { get; private set; }

        public InsideBarStrategy(Instrument instrument, int maxPriceOneTransaction, Account account)
        {
            Instrument = instrument;
            MaxPriceOneTransaction = maxPriceOneTransaction;
            Account = account;
        }

        public void Play(Chart chart)
        {
            var lastQuotation = chart.GetLastQuotation();
            CloseTransaction(lastQuotation);
            chart.SetInsideBar(lastQuotation.Time);
            if (chart.HasInsideBar() && Account.GetOpenTransaction(chart.Symbol) != null)
            {
                if (!TodayPlayedSymbol())
                {
                    if (BreakingDownChart(lastQuotation, chart))
                    {
                        var volumen = Instrument.ComputeVolumeToBuyByMaxPrice(MaxPriceOneTransaction, lastQuotation.Close);
                        Account.AddTransaction(1, Instrument, lastQuotation.Close, null, null, volumen, TypeTransaction.Sell, lastQuotation.Time, typeof(InsideBarStrategy).Name);
                    }
                    else if (BreakingOutChart(lastQuotation, chart))
                    {
                        var volumen = Instrument.ComputeVolumeToBuyByMaxPrice(MaxPriceOneTransaction, lastQuotation.Close);
                        Account.AddTransaction(1, Instrument, lastQuotation.Close, null, null, volumen, TypeTransaction.Buy, lastQuotation.Time, typeof(InsideBarStrategy).Name);
                    }
                }
            }
        }

        private void CloseTransaction(Quotation quotation)
        {
            var transaction = Account.GetOpenTransaction(Instrument.Symbol);
            if (transaction != null)
            {
                if (DateTime.Now.Hour > 22)
                {
                    Account.CloseTransaction(transaction.OrderId, quotation.Close, quotation.Time);
                }
                return;
            }
        }

        private bool TodayPlayedSymbol()
        {
            var closedTransactions = Account.GetClosedTransactions();
            return closedTransactions.Any(x => x.Instrument.Symbol == Instrument.Symbol && x.DateClose?.Date == DateTime.Now.Date);
        }

        private bool BreakingDownChart(Quotation quotation, Chart chart)
            => quotation.Low < chart.InsideBar.LowInsideBar;

        private bool BreakingOutChart(Quotation quotation, Chart chart)
            => quotation.High > chart.InsideBar.HighInsideBar;
    }
}
