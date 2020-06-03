using System;
using System.Collections.Generic;

namespace StrategySimulator.Models
{
    public class HistorySimulate
    {
        public string Strategy { get; set; }
        public List<Entry> Entries { get; set; }
        public List<Exit> Exits { get; set; }
        public decimal Profit { get; set; }
        public decimal ProfitPips { get; set; }
        public decimal Effectiveness { get; set; }
    }

    public class Entry
    {
        public DateTime Date { get; set; }
        public string Info { get; set; }
    }

    public class Exit
    {
        public DateTime Date { get; set; }
        public string Info { get; set; }
    }
}
