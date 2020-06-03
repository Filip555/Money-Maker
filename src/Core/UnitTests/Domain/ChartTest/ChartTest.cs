using Domain.AggregatesModel.ChartAggregate;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using Xunit;

namespace DomainTests.Domain.ChartTest
{
    public class ChartTest
    {
        [Fact]
        public void Should_return_quotation_from_20_02_2020_thursday_day_when_give_date_21_02_2020_friday()
        {
            var quotations = new List<Quotation>
            {
                new Quotation(new DateTime(2020, 02, 17, 0, 0, 0), 10, 8, 12, 11, 1000),
                new Quotation(new DateTime(2020, 02, 20, 0, 0, 0), 10, 8, 12, 11, 1000),
                new Quotation(new DateTime(2020, 02, 19, 0, 0, 0), 10, 8, 12, 11, 1000),
                new Quotation(new DateTime(2020, 02, 18, 0, 0, 0), 10, 8, 12, 11, 1000)
            };

            var chart = new Chart(quotations, Inteval.D1, "DE30");

            var lastQuotation = chart.GetDayQuotation(new DateTime(2020, 02, 21, 4, 0, 0), 1);
            Assert.Equal(lastQuotation.Time, new DateTime(2020, 02, 20, 0, 0, 0));
        }

        [Fact]
        public void Should_return_quotation_from_21_02_2020_friday_day_when_give_date_24_02_2020_monday()
        {
            var quotations = new List<Quotation>
            {
                new Quotation(new DateTime(2020, 02, 19, 5, 0, 0), 10, 8, 12, 11, 1000),
                new Quotation(new DateTime(2020, 02, 20, 2, 0, 0), 10, 8, 12, 11, 1000),
                new Quotation(new DateTime(2020, 02, 21, 1, 0, 0), 10, 8, 12, 11, 1000),
                new Quotation(new DateTime(2020, 02, 18, 0, 0, 0), 10, 8, 12, 11, 1000)
            };

            var chart = new Chart(quotations, Inteval.D1, "DE30");

            var lastQuotation = chart.GetDayQuotation(new DateTime(2020, 02, 24, 1, 0, 0), 1);
            Assert.Equal(lastQuotation.Time, new DateTime(2020, 02, 21, 1, 0, 0));
        }

        [Fact]
        public void Should_return_quotation_from_20_02_2020_friday_day_when_give_date_24_02_2020_monday_and_2_days_ago()
        {
            var quotations = new List<Quotation>
            {
                new Quotation(new DateTime(2020, 02, 19, 5, 0, 0), 10, 8, 12, 11, 1000),
                new Quotation(new DateTime(2020, 02, 20, 2, 0, 0), 10, 8, 12, 11, 1000),
                new Quotation(new DateTime(2020, 02, 21, 1, 0, 0), 10, 8, 12, 11, 1000),
                new Quotation(new DateTime(2020, 02, 18, 0, 0, 0), 10, 8, 12, 11, 1000)
            };

            var chart = new Chart(quotations, Inteval.D1, "DE30");

            var lastQuotation = chart.GetDayQuotation(new DateTime(2020, 02, 24, 1, 0, 0), 2);
            Assert.Equal(lastQuotation.Time, new DateTime(2020, 02, 20, 2, 0, 0));
        }

        [Fact]
        public void Should_return_quotation_from_21_02_2020_friday_day_when_give_date_22_02_2020_saturday()
        {
            var quotations = new List<Quotation>
            {
                new Quotation(new DateTime(2020, 02, 21, 1, 0, 0), 10, 8, 12, 11, 1000),
                new Quotation(new DateTime(2020, 02, 20, 3, 0, 0), 10, 8, 12, 11, 1000),
                new Quotation(new DateTime(2020, 02, 19, 3, 0, 0), 10, 8, 12, 11, 1000),
                new Quotation(new DateTime(2020, 02, 18, 5, 0, 0), 10, 8, 12, 11, 1000)
            };

            var chart = new Chart(quotations, Inteval.D1, "DE30");

            var lastQuotation = chart.GetDayQuotation(new DateTime(2020, 02, 22, 5, 0, 0), 1);
            Assert.Equal(lastQuotation.Time, new DateTime(2020, 02, 21, 1, 0, 0));
        }
        [Fact]
        public void Should_return_quotation_from_21_02_2020_friday_day_when_give_date_23_02_2020_sunday()
        {
            var quotations = new List<Quotation>
            {
                new Quotation(new DateTime(2020, 02, 21, 0, 0, 0), 10, 8, 12, 11, 1000),
                new Quotation(new DateTime(2020, 02, 20, 0, 0, 0), 10, 8, 12, 11, 1000),
                new Quotation(new DateTime(2020, 02, 19, 0, 0, 0), 10, 8, 12, 11, 1000),
                new Quotation(new DateTime(2020, 02, 18, 0, 0, 0), 10, 8, 12, 11, 1000)
            };

            var chart = new Chart(quotations, Inteval.D1, "DE30");

            var lastQuotation = chart.GetDayQuotation(new DateTime(2020, 02, 23, 0, 0, 0), 1);
            Assert.Equal(lastQuotation.Time, new DateTime(2020, 02, 21, 0, 0, 0));
        }

        [Fact]
        public void Should_return_quotation_from_21_02_2020_friday_day_when_give_date_23_02_2020_inteval_h1()
        {
            var quotations = new List<Quotation>
            {
                new Quotation(new DateTime(2020, 02, 21, 0, 0, 0), 10, 8, 12, 11, 1000),
                new Quotation(new DateTime(2020, 02, 20, 0, 0, 0), 10, 8, 12, 11, 1000),
                new Quotation(new DateTime(2020, 02, 19, 0, 0, 0), 10, 8, 12, 11, 1000),
                new Quotation(new DateTime(2020, 02, 18, 0, 0, 0), 10, 8, 12, 11, 1000)
            };

            var chart = new Chart(quotations, Inteval.H1, "DE30");

            var lastQuotation = chart.GetDayQuotation(new DateTime(2020, 02, 23, 0, 0, 0), 1);
            Assert.Equal(lastQuotation.Time, new DateTime(2020, 02, 21, 0, 0, 0));
            Assert.Equal(10, lastQuotation.Open);
            Assert.Equal(12, lastQuotation.High);
            Assert.Equal(8, lastQuotation.Low);
            Assert.Equal(11, lastQuotation.Close);
        }

        [Fact]
        public void Should_throw_domain_exception_no_previous_day_quotation_when_give_date_25_02_2020()
        {
            var quotations = new List<Quotation>
            {
                new Quotation(new DateTime(2020, 02, 21, 0, 0, 0), 10, 8, 12, 11, 1000),
                new Quotation(new DateTime(2020, 02, 20, 0, 0, 0), 10, 8, 12, 11, 1000),
                new Quotation(new DateTime(2020, 02, 19, 0, 0, 0), 10, 8, 12, 11, 1000),
                new Quotation(new DateTime(2020, 02, 18, 0, 0, 0), 10, 8, 12, 11, 1000)
            };

            var chart = new Chart(quotations, Inteval.D1, "DE30");

            var ex = Assert.Throws<DomainException>(() => chart.GetDayQuotation(new DateTime(2020, 02, 25, 0, 0, 0), 1));
            Assert.Equal("There is no previous day quotation.", ex.Message);
        }


        [Fact]
        public void Should_return_quotation_from_date_21_02_2020_when_call_method_get_last_quotation()
        {
            var quotations = new List<Quotation>
            {
                new Quotation(new DateTime(2020, 02, 20, 0, 0, 0), 10, 8, 12, 11, 1000),
                new Quotation(new DateTime(2020, 02, 19, 0, 0, 0), 10, 8, 12, 11, 1000),
                new Quotation(new DateTime(2020, 02, 21, 0, 0, 0), 10, 8, 12, 11, 1000),
                new Quotation(new DateTime(2020, 02, 18, 0, 0, 0), 10, 8, 12, 11, 1000)
            };

            var chart = new Chart(quotations, Inteval.D1, "DE30");

            var quotation = chart.GetLastQuotation();
            Assert.Equal(quotation.Time, new DateTime(2020, 02, 21, 0, 0, 0));
        }

        [Fact]
        public void Should_return_ohlc_10_12_8_11_when_give_m1_quotations()
        {
            var quotations = new List<Quotation>
            {
                new Quotation(new DateTime(2020, 02, 20, 8, 0, 0), 10, 9, 11, 9, 1000),
                new Quotation(new DateTime(2020, 02, 20, 12, 0, 0), 12, 11, 12, 11, 1000),
                new Quotation(new DateTime(2020, 02, 20, 15, 0, 0), 11, 8, 12, 8, 1000),
                new Quotation(new DateTime(2020, 02, 20, 23, 0, 0), 12, 10, 12, 11, 1000)
            };

            var chart = new Chart(quotations, Inteval.M1, "DE30");

            var quotation = chart.GetDayQuotation(new DateTime(2020, 02, 21, 0, 0, 0), 1);
            Assert.Equal(10, quotation.Open);
            Assert.Equal(12, quotation.High);
            Assert.Equal(8, quotation.Low);
            Assert.Equal(11, quotation.Close);
        }
    }
}
