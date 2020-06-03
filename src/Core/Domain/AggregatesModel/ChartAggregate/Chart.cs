using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.AggregatesModel.ChartAggregate
{
    using Domain.AggregatesModel.AccountAggregate;

    public class Chart
    {
        public List<Quotation> Quotations { get; private set; }
        public Inteval Interval { get; private set; }
        public string Symbol { get; private set; }
        public InsideBar InsideBar { get; private set; }
        public Chart(List<Quotation> quotations, Inteval interval, string symbol)
        {
            Quotations = quotations;
            Interval = interval;
            Symbol = symbol;
        }

        public Quotation GetDayQuotation(DateTime dateTime, int daysAgo)
        {
            var previousDay = dateTime.DayOfWeek == DayOfWeek.Sunday ? dateTime.AddDays(-1 - daysAgo).Date
                                                         : (dateTime.DayOfWeek == DayOfWeek.Monday ? dateTime.AddDays(-2 - daysAgo)
                                                                                                   : dateTime.AddDays(0 - daysAgo));
            if (!Interval.Equals(Inteval.D1))
            {
                var quotationByDay = Quotations.Where(x => x.Time.Date == previousDay.Date);
                if (!quotationByDay.Any())
                {
                    throw new DomainException("There is no previous quotations");
                }
                var open = quotationByDay.OrderBy(x => x.Time).FirstOrDefault().Open;
                var high = quotationByDay.Max(x => x.High);
                var low = quotationByDay.Min(x => x.Low);
                var close = quotationByDay.OrderByDescending(x => x.Time).FirstOrDefault().Close;
                var volume = quotationByDay.Sum(x => x.Volume);
                return new Quotation(previousDay.Date, open, low, high, close, volume);
            }
            return Quotations.FirstOrDefault(x => x.Time.Date == previousDay.Date) ?? throw new DomainException("There is no previous day quotation.");
        }

        public Quotation GetLastQuotation()
            => Quotations.OrderBy(x => x.Time).LastOrDefault();

        public List<Quotation> GetLastDayQuotations(DateTime dateTime)
        {
            var previousDay = dateTime.DayOfWeek == DayOfWeek.Sunday ? dateTime.AddDays(-2).Date
                                             : (dateTime.DayOfWeek == DayOfWeek.Monday ? dateTime.AddDays(-3)
                                                                                       : dateTime.AddDays(-1));
            return Quotations.Where(x => x.Time.Day == previousDay.Date.Day).ToList();
        }
        public void SetInsideBar(DateTime dateTime)
        {
            try
            {
                var motherQuotation = GetDayQuotation(dateTime, 2);
                var childQuotation = GetDayQuotation(dateTime, 1);
                if (IsInsideBar(motherQuotation, childQuotation))
                {
                    var side = (childQuotation.Low - motherQuotation.Low) > (motherQuotation.High - childQuotation.High) ? TypeTransaction.Sell : TypeTransaction.Buy;
                    InsideBar = new InsideBar(childQuotation.High, childQuotation.Low, motherQuotation.High, motherQuotation.Low, side);
                }
            }
            catch (DomainException e)
            {
                Console.WriteLine(e);
            }
        }
        public bool HasInsideBar()
            => InsideBar != null;
        private bool IsInsideBar(Quotation mother, Quotation child)
        {
            var rangeMother = mother.High - mother.Low;
            var rangeChild = child.High - child.Low;

            if (rangeMother > rangeChild && mother.High > child.High && mother.Low < child.Low)
            {
                return true;
            }
            return false;
        }
    }
}
