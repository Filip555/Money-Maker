namespace Domain.AggregatesModel.IndicatorAggregate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class Indicators
    {
        public static double[] SMA(double[] values, double period)
        {
            var result = new double[values.Length];
            return result;
        }

        public static double[] EMA(double[] values, double period)
        {
            var result = new double[values.Length];
            result[0] = values[0];
            var k = 2 / (period + 1);
            for (var i = 1; i < values.Length; i++)
            {
                result[i] = (values[i] - result[i - 1]) * k + result[i - 1];
            }
            return result;
        }

        public static double[] WMA(double[] values, int period)
        {
            var periodRange = Enumerable.Range(1, period);
            var periodSum = (double)periodRange.Sum();
            var periodWeights = periodRange.Select(x => x / periodSum).ToArray();
            List<double> priceList = values.ToList();
            var wma = new double[values.Length];
            for (var i = 0; i < period; i++)
            {
                wma[i] = values[i];
            }
            for (var i = period; i < values.Length; i++)
            {
                var priceRange = priceList.GetRange(i - period, period);
                wma[i] = priceRange.Select((price, k) => periodWeights[k] * price).Sum();
            }
            return wma;
        }

        public static double[] Hull(double[] values, int period)
        {
            var wma1 = WMA(values, period);
            var wma2 = WMA(values, period / 2);
            var wmaSub = new double[values.Length];
            for (var i = 0; i < values.Length; i++)
            {
                wmaSub[i] = 2 * wma2[i] - wma1[i];
            }
            return WMA(wmaSub, (int)Math.Sqrt(period));
        }

        public static double[] LRMA(double[] values, double period)
        {
            var result = new double[values.Length];
            return result;
        }

        public static double[] RSI(double[] values, double period)
        {
            var result = new double[values.Length];
            return result;
        }

        public static double[] MACD(double[] values, double period)
        {
            var result = new double[values.Length];
            return result;
        }

        public static double[] SSA(double[] values, double period)
        {
            var result = new double[values.Length];
            return result;
        }
    }
}
