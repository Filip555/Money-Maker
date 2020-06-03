using Domain.AggregatesModel.AccountAggregate;
using Domain.AggregatesModel.ChartAggregate;
using Domain.AggregatesModel.InstrumentsAggregate;

namespace Domain.Strategies
{
    public class BuyOnEndDayStrategy : IStrategy
    {
        public Instrument Instrument { get; }

        public Account Account { get; }

        public int MaxPriceOneTransaction { get; }

        public BuyOnEndDayStrategy(Instrument instrument, Account account, int maxPriceOneTransaction)
        {
            Instrument = instrument;
            Account = account;
            MaxPriceOneTransaction = maxPriceOneTransaction;
        }
        public void Play(Chart chart)
        {
            var lastQuotation = chart.GetLastQuotation();
            var transaction = Account.GetOpenTransaction(Instrument.Symbol);
            if (transaction != null)
            {
                if (transaction.DateOpen.Date < lastQuotation.Time.Date)
                {
                    Account.CloseTransaction(transaction.OrderId, lastQuotation.Close, lastQuotation.Time);
                }
            }
            if (Account.GetOpenTransaction(Instrument.Symbol) == null)
            {
                var volumen = Instrument.ComputeVolumeToBuyByMaxPrice(MaxPriceOneTransaction, lastQuotation.Close);
                Account.AddTransaction(0, Instrument, lastQuotation.Close, null, null, volumen, TypeTransaction.Buy, lastQuotation.Time, typeof(BuyOnEndDayStrategy).Name);
            }
        }
    }
}
