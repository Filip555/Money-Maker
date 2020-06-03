using Domain.AggregatesModel.InstrumentsAggregate;
using Domain.Exceptions;
using Domain.SeedWork;
using System;

namespace Domain.AggregatesModel.AccountAggregate
{
    public class Transaction : Entity
    {
        public Instrument Instrument { get; protected set; }
        public DateTime DateOpen { get; protected set; }
        public DateTime? DateClose { get; protected set; }
        public decimal Profit { get; protected set; }
        public decimal ProfitPips { get; private set; }
        public TypeTransaction TypeTransaction { get; protected set; }
        public double? ClosePrice { get; protected set; }
        public long OrderId { get; set; }
        public Position Position { get; protected set; }

        public string Info { get; set; }

        public Transaction(long orderId, Instrument instrument, Position position, TypeTransaction typeTransaction, DateTime dateOpen, string info)
        {
            DateOpen = dateOpen;
            OrderId = orderId;
            Instrument = instrument ?? throw new DomainException("Instrument cannot be null.");
            Position = position ?? throw new DomainException("Instrument cannot be null."); ;
            TypeTransaction = typeTransaction ?? throw new DomainException("Instrument cannot be null."); ;
            Info = info;
        }

        public decimal ComputeProfitInPips(double closePrice)
        {
            if (closePrice <= 0) return 0;
            var openPriceToPrecision = Position.OpenPrice * Math.Pow(10, Instrument.Precision);
            var closePriceToPrecision = closePrice * Math.Pow(10, Instrument.Precision);
            var profitInPips = TypeTransaction.Equals(TypeTransaction.Buy) ? ((closePriceToPrecision - openPriceToPrecision) / 10)
                                                                           : (TypeTransaction.Equals(TypeTransaction.Sell) ? ((openPriceToPrecision - closePriceToPrecision) / 10)
                                                                                                                          : throw new DomainException("Transaction type is not set.")); //change it to domain
            return Math.Round((decimal)profitInPips, 2);
        }

        public void CloseTransaction(double closePrice, DateTime dateClose)
        {
            if (ClosePrice != null) throw new DomainException("Is not possible to close transaction twice.");
            ClosePrice = closePrice;
            DateClose = dateClose;
            ProfitPips = ComputeProfitInPips(closePrice);
            var valueOfOnePips = (decimal)Instrument.ComputeValueOfOnePips(Position.Volumen, Position.OpenPrice);
            SetProfit(Math.Round(valueOfOnePips * ProfitPips, 2));
        }
        public void SetProfit(decimal profit)
            => Profit = profit;

        public bool IsOpen()
            => DateClose == null;

        public long GetTickFromOpenLocalTimeString()
        {
            var date = Convert.ToDateTime(DateOpen);
            var dateTimeOffset = new DateTimeOffset(date);
            return dateTimeOffset.ToUnixTimeMilliseconds();
        }

        public long GetTickFromCloseLocalTimeString()
        {
            var date = Convert.ToDateTime(DateClose);
            var dateTimeOffset = new DateTimeOffset(date);
            return dateTimeOffset.ToUnixTimeMilliseconds();
        }
    }
}
