using Domain.SeedWork;
using System.Collections.Generic;

namespace Domain.AggregatesModel.InstrumentsAggregate
{
    public class Valuation : ValueObject
    {
        public double BidPrice { get; protected set; }
        public double AskPrice { get; protected set; }

        public Valuation(double bidPrice, double askPrice)
        {
            BidPrice = bidPrice;
            AskPrice = askPrice;
        }

        public void ChangeBidPrice(double bidPrice)
        {
            BidPrice = bidPrice;
        }

        public void ChangeAskPrice(double askPrice)
        {
            AskPrice = askPrice;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return BidPrice;
            yield return AskPrice;
        }
    }
}
