
namespace Domain.Common
{
    public class SimpleDiscountDividendModel
    {
        public SimpleDiscountDividendModel(double dividendOnShares, double discountRate = 0.072)
        {
            Value = dividendOnShares / discountRate;
        }

        public double Value { get; }
    }
}
