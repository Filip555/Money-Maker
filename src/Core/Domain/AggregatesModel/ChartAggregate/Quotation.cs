using Domain.SeedWork;
using System;
using System.Collections.Generic;

namespace Domain.AggregatesModel.ChartAggregate
{
    //Entity
    public class Quotation : ValueObject
    {
        public double Open { get; protected set; }
        public double Low { get; protected set; }
        public double High { get; protected set; }
        public double Close { get; protected set; }
        public double Volume { get; protected set; }
        public DateTime Time { get; protected set; }
        public double EmaValue { get; set; }

        public Quotation(DateTime time, double open, double low, double high, double close, double volume)
        {
            Time = time;
            Open = open;
            Low = low;
            High = high;
            Close = close;
            Volume = volume;
        }

        public bool CheckIfQuotationIsPositive()
        {
            return Open < Close;
        }

        public bool IsInEuropeTimeFrame()
        {
            return Time.Hour >= 9 && (Time.Hour <= 15 && Time.Hour <= 30);
        }

        public bool IsInUsTimeFrame()
        {
            return ((Time.Hour >= 15 && Time.Minute >= 30) || Time.Hour > 15) && Time.Hour <= 22;
        }

        public void AddEma(double emaValue)
        {
            EmaValue = emaValue;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Open;
            yield return Low;
            yield return High;
            yield return Close;
            yield return Volume;
            yield return Time;
            yield return EmaValue;
        }
    }
}
