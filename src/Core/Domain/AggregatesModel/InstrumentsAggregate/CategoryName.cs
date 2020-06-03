using Domain.SeedWork;

namespace Domain.AggregatesModel.InstrumentsAggregate
{
    public class CategoryName : Enumeration
    {
        public static CategoryName Forex = new CategoryName(1, "FX");
        public static CategoryName Index = new CategoryName(2, "IND");
        public static CategoryName Commodity = new CategoryName(3, "CMD");
        public static CategoryName Crypto = new CategoryName(3, "CRT");
        public static CategoryName Stock = new CategoryName(3, "STC");
        public static CategoryName Etf = new CategoryName(3, "ETF");

        public CategoryName(int id, string name)
               : base(id, name)
        {
        }
    }
}
