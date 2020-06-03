using System;

namespace Domain.AggregatesModel.InstrumentsAggregate
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Json.Serialization;

    public class Instrument
    {
        public string Symbol { get; protected set; }
        public CategoryName CategoryName { get; protected set; }
        public long ContractSize { get; protected set; }
        public string Currency { get; protected set; }
        public double Leverage { get; protected set; }
        public double Precision { get; protected set; }
        public double Spread { get; protected set; }
        public string GroupName { get; protected set; }
        public string Isin { get; protected set; }
        public List<Dividend> Dividends { get; protected set; }
        public Valuation Valuation { get; protected set; }

        [JsonIgnore]
        public TimeSpan OpenTime { get; set; }

        public Instrument(string symbol, CategoryName categoryName, string currency, Valuation valuation, TimeSpan openTime, string groupName = "")
        {
            OpenTime = openTime;
            Valuation = valuation;
            GroupName = groupName;
            Symbol = symbol;
            CategoryName = categoryName;
            Currency = currency;
            Dividends ??= new List<Dividend>();
        }

        public void AddDividends(List<Dividend> dividend)
            => Dividends.AddRange(dividend);

        public Dividend GetLastDividend()
            => Dividends.OrderByDescending(x => x.Date).FirstOrDefault(x => x.Date.Year >= DateTime.Now.AddYears(-1).Date.Year && x.Date < DateTime.Now);

        public void SetPrices(Valuation valuation)
        {
            Valuation.ChangeAskPrice(valuation.AskPrice);
            Valuation.ChangeBidPrice(valuation.BidPrice);
        }

        public void SetContractSize(long contractSize)
            => ContractSize = contractSize;

        public void SetLeverage(double leverage)
            => Leverage = leverage;

        public void SetIsin(string isin)
            => Isin = isin;

        public void SetPrecision(double precision)
            => Precision = precision;

        public void SetSpread(double spread)
            => Spread = spread;

        private decimal ContractValue(double bidPrice)
            => (decimal)(CategoryName.Equals(CategoryName.Forex) ? 100000 : ContractSize * bidPrice);

        public decimal ComputeValueTransaction(decimal volumen, double bidPrice)
        {
            var levargeInt = (int)(100 / Leverage);
            return Math.Round((decimal)(CalculateExchangeRate(Currency) * (volumen * ContractValue(bidPrice)) / levargeInt), 2);
        }

        public decimal ComputeVolumeToBuyByMaxPrice(int priceMax, double bidPrice)
        {
            var volumen = 0.01M;
            for (decimal i = 0.01M; i < 100; i += 0.01M)
            {
                var levargeInt = (int)(100 / Leverage);
                if ((CalculateExchangeRate(Currency) * (i * ContractValue(bidPrice)) / levargeInt) < priceMax)
                {
                    volumen = i;
                    continue;
                }
                break;
            }
            return Math.Round(volumen, 2);
        }

        public double ComputeValueOfOnePips(decimal volumen, double bidPrice)
        {
            var contract = ContractValue(bidPrice);
            return Math.Round(((1 * Math.Pow(10, 0 - Precision + 1)) * (double)(volumen * contract)) / bidPrice * (double)CalculateExchangeRate(Currency), 2);
        }

        private decimal CalculateExchangeRate(string symbol)
        {
            var firstChars = symbol.Substring(0, 3);
            switch (firstChars)
            {
                case "USD":
                    return ExchangeRates.GetUsd();
                case "GOL":
                    return ExchangeRates.GetUsd();
                case "AUD":
                    return ExchangeRates.GetAud();
                case "JPY":
                    return ExchangeRates.GetJpy();
                case "NZD":
                    return ExchangeRates.GetNzd();
                case "EUR":
                    return ExchangeRates.GetEur();
                case "GBP":
                    return ExchangeRates.GetGbp();
                case "CHF":
                    return ExchangeRates.GetChf();
                case "CAD":
                    return ExchangeRates.GetCad();
                case "PLN":
                    return 1;
                default:
                    Console.WriteLine("nieznana waluta");
                    return 0;
            }
        }

        // Temp
        public static class ExchangeRates
        {
            public static decimal GetGbp()
                => 5.049M;
            public static decimal GetUsd()
                => 3.94M;
            public static decimal GetJpy()
                => 0.34M;
            public static decimal GetAud()
                => 2.69M;
            public static decimal GetNzd()
                => 2.60M;
            public static decimal GetEur()
                => 4.30M;
            public static decimal GetChf()
                => 3.9M;
            public static decimal GetCad()
                => 2.92M;
        }
    }
}
