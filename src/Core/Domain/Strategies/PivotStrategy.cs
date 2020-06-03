using System;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Strategies
{
    using Domain.AggregatesModel.IndicatorAggregate;
    using Domain.AggregatesModel.AccountAggregate;
    using Domain.AggregatesModel.ChartAggregate;
    using Domain.AggregatesModel.InstrumentsAggregate;

    public class PivotStrategy
    {
        public Instrument Instrument { get; }

        public int MaxPriceOneTransaction { get; }

        public Account Account { get; }

        public PivotStrategy(Instrument instrument, int maxPriceOneTransaction, Account account)
        {
            Instrument = instrument;
            MaxPriceOneTransaction = maxPriceOneTransaction;
            Account = account;
        }

        // Ema - 44
        public Task Play(Pivot pivot, Chart chart, int ema, int stopLoss) 
        {
            var emasH = Indicators.EMA(chart.Quotations.Select(x => x.Close).ToArray(), ema);
            for (int i = 0; i < emasH.Count(); i++)
            {
                chart.Quotations[i].AddEma(emasH[i]);
            }
            var openTransactions = Account.GetOpenTransaction(chart.Symbol);
            if (openTransactions == null)
            {
                var lastQuotation = chart.GetLastQuotation();
                if (lastQuotation.High < pivot.P && lastQuotation.Close > pivot.S1 && lastQuotation.Low < pivot.S1)
                {
                    if (lastQuotation.Close < lastQuotation.EmaValue)
                    {
                        var takeProfit = (int)Math.Abs(Math.Round(lastQuotation.Close - pivot.S1, 0));
                        var stopLossAdditional = (int)Math.Abs(Math.Round(lastQuotation.Close - pivot.P, 0));
                        var volumen = GetVolumen(TypeTransaction.Sell);
                        if (takeProfit / stopLoss > 2) Account.AddTransaction(1, Instrument, lastQuotation.Close, stopLoss, takeProfit, volumen, TypeTransaction.Sell, lastQuotation.Time, typeof(PivotStrategy).Name);
                    }
                    else if (lastQuotation.Close > lastQuotation.EmaValue)
                    {
                        var takeProfit = (int)Math.Abs(Math.Round(lastQuotation.Close - pivot.P, 0));
                        var stopLossAdditional = (int)Math.Abs(Math.Round(lastQuotation.Close - pivot.S1, 0));
                        var volumen = GetVolumen(TypeTransaction.Buy);
                        if (takeProfit / stopLoss > 2) Account.AddTransaction(1, Instrument, lastQuotation.Close, stopLoss, takeProfit, volumen, TypeTransaction.Buy, lastQuotation.Time, typeof(PivotStrategy).Name);
                    }
                }
                else if (lastQuotation.High < pivot.S1 && lastQuotation.Close > pivot.S2)
                {
                    if (lastQuotation.Close < lastQuotation.EmaValue)
                    {
                        var takeProfit = (int)Math.Abs(Math.Round(lastQuotation.Close - pivot.S2, 0));
                        var stopLossAdditional = (int)Math.Abs(Math.Round(lastQuotation.Close - pivot.S1, 0));
                        var volumen = GetVolumen(TypeTransaction.Sell);
                        if (takeProfit / stopLoss > 2) Account.AddTransaction(1, Instrument, lastQuotation.Close, stopLoss, takeProfit, volumen, TypeTransaction.Sell, lastQuotation.Time, typeof(PivotStrategy).Name);
                    }
                    else if (lastQuotation.Close > lastQuotation.EmaValue)
                    {
                        var takeProfit = (int)Math.Abs(Math.Round(lastQuotation.Close - pivot.S1, 0));
                        var stopLossAdditional = (int)Math.Abs(Math.Round(lastQuotation.Close - pivot.S2, 0));
                        var volumen = GetVolumen(TypeTransaction.Buy);
                        if (takeProfit / stopLoss > 2) Account.AddTransaction(1, Instrument, lastQuotation.Close, stopLoss, takeProfit, volumen, TypeTransaction.Buy, lastQuotation.Time, typeof(PivotStrategy).Name);
                    }
                }
                else if (lastQuotation.High < pivot.S2 && lastQuotation.Close > pivot.S3)
                {
                    if (lastQuotation.Close < lastQuotation.EmaValue) 
                    {
                        var takeProfit = (int)Math.Abs(Math.Round(lastQuotation.Close - pivot.S3, 0));
                        var stopLossAdditional = (int)Math.Abs(Math.Round(lastQuotation.Close - pivot.S2, 0));
                        var volumen = GetVolumen(TypeTransaction.Sell);
                        if (takeProfit / stopLoss > 2) Account.AddTransaction(1, Instrument, lastQuotation.Close, stopLoss, takeProfit, volumen, TypeTransaction.Sell, lastQuotation.Time, typeof(PivotStrategy).Name);
                    }
                    else if (lastQuotation.Close > lastQuotation.EmaValue) 
                    {
                        var takeProfit = (int)Math.Abs(Math.Round(lastQuotation.Close - pivot.S2, 0));
                        var stopLossAdditional = (int)Math.Abs(Math.Round(lastQuotation.Close - pivot.S3, 0));
                        var volumen = GetVolumen(TypeTransaction.Buy);
                        if (takeProfit / stopLoss > 2) Account.AddTransaction(1, Instrument, lastQuotation.Close, stopLoss, takeProfit, volumen, TypeTransaction.Buy, lastQuotation.Time, typeof(PivotStrategy).Name);
                    }
                }
                else if (lastQuotation.Low > pivot.P && lastQuotation.Close < pivot.R1) 
                {
                    if (lastQuotation.Close > lastQuotation.EmaValue)
                    {
                        var takeProfit = (int)Math.Abs(Math.Round(lastQuotation.Close - pivot.R1, 0));
                        var stopLossAdditional = (int)Math.Abs(Math.Round(lastQuotation.Close - pivot.P, 0));
                        var volumen = GetVolumen(TypeTransaction.Buy);
                        if (takeProfit / stopLoss > 2) Account.AddTransaction(1, Instrument, lastQuotation.Close, stopLoss, takeProfit, volumen, TypeTransaction.Buy, lastQuotation.Time, typeof(PivotStrategy).Name);
                    }
                    else if (lastQuotation.Close < lastQuotation.EmaValue)
                    {
                        var takeProfit = (int)Math.Abs(Math.Round(lastQuotation.Close - pivot.P, 0));
                        var stopLossAdditional = (int)Math.Abs(Math.Round(lastQuotation.Close - pivot.R1, 0));
                        var volumen = GetVolumen(TypeTransaction.Sell);
                        if (takeProfit / stopLoss > 2) Account.AddTransaction(1, Instrument, lastQuotation.Close, stopLoss, takeProfit, volumen, TypeTransaction.Sell, lastQuotation.Time, typeof(PivotStrategy).Name);
                    }
                }
                else if (lastQuotation.Low > pivot.R1 && lastQuotation.Close < pivot.R2)
                {
                    if (lastQuotation.Close > lastQuotation.EmaValue)
                    {
                        var takeProfit = (int)Math.Abs(Math.Round(lastQuotation.Close - pivot.R2, 0));
                        var stopLossAdditional = (int)Math.Abs(Math.Round(lastQuotation.Close - pivot.R1, 0));
                        var volumen = GetVolumen(TypeTransaction.Buy);
                        if (takeProfit / stopLoss > 2) Account.AddTransaction(1, Instrument, lastQuotation.Close, stopLoss, takeProfit, volumen, TypeTransaction.Buy, lastQuotation.Time, typeof(PivotStrategy).Name);
                    }
                    else if (lastQuotation.Close < lastQuotation.EmaValue)
                    {
                        var takeProfit = (int)Math.Abs(Math.Round(lastQuotation.Close - pivot.R1, 0));
                        var stopLossAdditional = (int)Math.Abs(Math.Round(lastQuotation.Close - pivot.R2, 0));
                        var volumen = GetVolumen(TypeTransaction.Sell);
                        if (takeProfit / stopLoss > 2) Account.AddTransaction(1, Instrument, lastQuotation.Close, stopLoss, takeProfit, volumen, TypeTransaction.Sell, lastQuotation.Time, typeof(PivotStrategy).Name);
                    }
                }
                else if (lastQuotation.Low > pivot.R2 && lastQuotation.Close < pivot.R3) 
                {
                    if (lastQuotation.Close > lastQuotation.EmaValue)
                    {
                        var takeProfit = (int)Math.Abs(Math.Round(lastQuotation.Close - pivot.R3, 0));
                        var stopLossAdditional = (int)Math.Abs(Math.Round(lastQuotation.Close - pivot.R2, 0));
                        var volumen = GetVolumen(TypeTransaction.Buy);
                        if (takeProfit / stopLoss > 2) Account.AddTransaction(1, Instrument, lastQuotation.Close, stopLoss, takeProfit, volumen, TypeTransaction.Buy, lastQuotation.Time, typeof(PivotStrategy).Name);
                    }
                    else if (lastQuotation.Close < lastQuotation.EmaValue)
                    {
                        var takeProfit = (int)Math.Abs(Math.Round(lastQuotation.Close - pivot.R2, 0));
                        var stopLossAdditional = (int)Math.Abs(Math.Round(lastQuotation.Close - pivot.R3, 0));
                        var volumen = GetVolumen(TypeTransaction.Sell);
                        if (takeProfit / stopLoss > 2) Account.AddTransaction(1, Instrument, lastQuotation.Close, stopLoss, takeProfit, volumen, TypeTransaction.Sell, lastQuotation.Time, typeof(PivotStrategy).Name);
                    }
                }
            }
            return Task.CompletedTask;
        }
        private decimal GetVolumen(TypeTransaction typeTransaction)
        {
            if (typeTransaction.Equals(TypeTransaction.Buy)) return Instrument.ComputeVolumeToBuyByMaxPrice(MaxPriceOneTransaction, Instrument.Valuation.AskPrice);
            if (typeTransaction.Equals(TypeTransaction.Sell)) return Instrument.ComputeVolumeToBuyByMaxPrice(MaxPriceOneTransaction, Instrument.Valuation.BidPrice);
            return 0;
        }
    }
}
