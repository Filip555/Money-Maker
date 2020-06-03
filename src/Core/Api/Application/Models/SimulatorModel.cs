using System.Collections.Generic;

namespace Api.Application.Models
{
    public struct SimulatorModel
    {
        public string Symbol { get; set; }
        public List<ProfitGroup> ProfitGroups { get; set; }
    }
    public struct ProfitGroup
    {
        public string Ema { get; set; }
        public string Interval { get; set; }
        public double ProfitInValue { get; set; }
        public int ProfitableTransaction { get; set; }
        public int NonProfitableTransaction { get; set; }
        public decimal TakeProfit { get; set; }
        public decimal StopLoss { get; set; }
        public decimal RateOfReturn { get; set; }
        public decimal ValueOfInvest { get; set; }
    }
}
