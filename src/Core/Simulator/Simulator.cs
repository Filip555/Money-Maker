
using Domain.AggregatesModel.AccountAggregate;
using Domain.AggregatesModel.ChartAggregate;
using Domain.Strategies;
using StrategySimulator.Models;
using System;
using System.Linq;

namespace StrategySimulator
{
    public class Simulator : ISimulator
    {
        public HistorySimulate Simulate(Chart chart, IStrategy strategyMain, Account account)
        {
            for (int i = 0; i < chart.Quotations.Count; i++)
            {
                var quotations = chart.Quotations.Take(i + 1).ToList();
                var tempoChart = new Chart(quotations, chart.Interval, chart.Symbol);
                strategyMain.Play(tempoChart);
            }
            var entries = account.Transactions.Select(x => new Entry
            {
                Date = x.DateOpen,
                Info = "a"
            }).ToList();

            var exits = account.GetClosedTransactions().Select(x => new Exit
            {
                Date = x.DateClose ?? throw new NullReferenceException(),
                Info = "b"
            }).ToList();

            var successful = account.Transactions.Where(x => x.Profit > 0).Count();
            var unsuccessful = account.Transactions.Where(x => x.Profit < 0).Count();

            return new HistorySimulate
            {
                Entries = entries,
                Exits = exits,
                Strategy = typeof(BuyOnEndDayStrategy).Name,
                Profit = account.Transactions.Sum(x => x.Profit),
                ProfitPips = account.Transactions.Sum(x => x.ComputeProfitInPips(x.ClosePrice ?? 0)),
                Effectiveness = ((double)successful + (double)unsuccessful) > 0 ? ((decimal)Math.Round((double)((double)successful / ((double)successful + (double)unsuccessful) * 100d), 2)) : 0
            };
        }
    }
}
