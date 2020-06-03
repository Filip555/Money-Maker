using Domain.SeedWork;

namespace Domain.AggregatesModel.ChartAggregate
{
    public class Inteval : Enumeration
    {
        public static Inteval M1 = new Inteval(1, "M1");
        public static Inteval M5 = new Inteval(2, "M5");
        public static Inteval M15 = new Inteval(3, "M15");
        public static Inteval M30 = new Inteval(4, "M30");
        public static Inteval H1 = new Inteval(5, "H1");
        public static Inteval H4 = new Inteval(6, "H4");
        public static Inteval D1 = new Inteval(7, "D1");

        public Inteval(int id, string name)
               : base(id, name)
        {
        }
    }
}
