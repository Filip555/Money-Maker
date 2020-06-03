using System;

namespace Domain.Common
{
    public class DiscountDividendModel
    {
        public DiscountDividendModel(double[] dividends, double discountRate = 0.072)
        {
            Value = Calculate(dividends, discountRate);
        }

        public double Value { get; }

        private double Calculate(double[] d, double s)
        {
            var v = 0.0;
            for (var i = 0; i < d.Length; i++)
            {
                v += d[i] / Math.Pow(1 + s, i);
            }
            return v;
        }
    }
}
