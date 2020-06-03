using System;

namespace Domain.AggregatesModel.InstrumentsAggregate
{
    public class Dividend
    {
        public double Price { get; private set; }
        public DateTime Date { get; private set; }
        public Dividend(double price, DateTime date)
        {
            Price = price;
            Date = date;
        }
    }
}
