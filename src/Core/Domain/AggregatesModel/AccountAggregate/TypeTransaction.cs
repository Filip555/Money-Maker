using Domain.SeedWork;

namespace Domain.AggregatesModel.AccountAggregate
{
    public class TypeTransaction : Enumeration
    {
        public static TypeTransaction Buy = new TypeTransaction(1, "Buy");
        public static TypeTransaction Sell = new TypeTransaction(2, "Sell");

        public TypeTransaction(int id, string name)
                  : base(id, name)
        {
        }
    }
}
