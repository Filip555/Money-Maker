namespace Api.Application.Models
{
    using Domain.AggregatesModel.AccountAggregate;

    public class InsideBarView
    {
        public double HighInsideBar { get; set; }
        public double LowInsideBar { get; set; }
        public double HighMotherInsideBar { get; set; }
        public double LowMotherInsideBar { get; set; }
        public TypeTransaction Side { get; set; }
        public string Symbol { get; set; }
    }
}
