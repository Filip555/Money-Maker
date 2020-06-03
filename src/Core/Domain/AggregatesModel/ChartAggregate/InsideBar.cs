namespace Domain.AggregatesModel.ChartAggregate
{
    using Domain.AggregatesModel.AccountAggregate;

    public class InsideBar
    {
        public double HighInsideBar { get; private set; }
        public double LowInsideBar { get; private set; }
        public double HighMotherInsideBar { get; private set; }
        public double LowMotherInsideBar { get; private set; }
        public TypeTransaction Side { get; private set; }

        public InsideBar(double highInsideBar, double lowInsideBar, double highMotherInsideBar, double lowMotherInsideBar, TypeTransaction side)
        {
            HighInsideBar = highInsideBar;
            LowInsideBar = lowInsideBar;
            HighMotherInsideBar = highMotherInsideBar;
            LowMotherInsideBar = lowMotherInsideBar;
            Side = side;
        }
    }
}
