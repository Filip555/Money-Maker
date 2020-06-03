using System;
using System.Threading.Tasks;

namespace Domain.Strategies
{
    using Domain.AggregatesModel.AccountAggregate;
    using Domain.AggregatesModel.InstrumentsAggregate;

    public class TechnicalAnalysisStrategy
    {
        public Instrument Instrument { get; private set; }
        public int MaxPriceOneTransaction { get; private set; }
        public Account Account { get; private set; }
        public TechnicalAnalysisStrategy(Instrument instrument, int maxPriceOneTransaction, Account account)
        {
            Instrument = instrument;
            MaxPriceOneTransaction = maxPriceOneTransaction;
            Account = account;
        }

        public Task Play(string info, string interval)
        {
            var volume = (decimal)Instrument.ComputeVolumeToBuyByMaxPrice(1000, Instrument.Valuation.BidPrice);
            var transaction = Account.GetOpenTransaction(Instrument.Symbol);
            if (info == "MOCNE KUP")
            {
                if (transaction != null)
                {
                    if (transaction.TypeTransaction.Equals(TypeTransaction.Sell))
                    {
                        Account.CloseTransaction(transaction.OrderId,Instrument.Valuation.AskPrice,DateTime.Now);
                    }
                }
                if (interval == "60")
                {
                    Account.AddTransaction(1, Instrument, Instrument.Valuation.AskPrice, null, null, volume, TypeTransaction.Buy, DateTime.Now, typeof(TechnicalAnalysisStrategy).Name);
                }
            }
            else if (info == "MOCNE SPRZEDAJ")
            {
                if (transaction != null)
                {
                    if (transaction.TypeTransaction.Equals(TypeTransaction.Buy))
                    {
                        Account.CloseTransaction(transaction.OrderId, Instrument.Valuation.AskPrice, DateTime.Now);
                    }
                }
                if (interval == "60")
                {
                    Account.AddTransaction(1, Instrument, Instrument.Valuation.BidPrice, null, null, volume, TypeTransaction.Sell, DateTime.Now, typeof(TechnicalAnalysisStrategy).Name);
                }
            }
            else
            {
                if (transaction != null)
                {
                    if (transaction.TypeTransaction == TypeTransaction.Buy)
                    {
                        Account.CloseTransaction(transaction.OrderId, Instrument.Valuation.AskPrice, DateTime.Now);
                    }
                    if (transaction.TypeTransaction == TypeTransaction.Sell)
                    {
                        Account.CloseTransaction(transaction.OrderId, Instrument.Valuation.BidPrice, DateTime.Now);
                    }
                }
            }
            return Task.CompletedTask;
        }
    }
}
