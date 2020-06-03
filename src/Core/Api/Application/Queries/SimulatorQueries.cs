using Api.Application.Models;
using Domain.AggregatesModel.AccountAggregate;
using Domain.AggregatesModel.ChartAggregate;
using Domain.AggregatesModel.IndicatorAggregate;
using Domain.AggregatesModel.InstrumentsAggregate;
using Domain.Strategies;
using Infrastructure.Helpers.DateTimeConverter;
using StrategySimulator;
using StrategySimulator.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Application.Queries
{
    public class SimulatorQueries : ISimulatorQueries
    {
        private readonly IChartRepository _chartRepository;
        private readonly IInstrumentRepository _instrumentRepository;

        public SimulatorQueries(IChartRepository chartRepository, IInstrumentRepository instrumentRepository)
        {
            _chartRepository = chartRepository;
            _instrumentRepository = instrumentRepository;
        }

        public Task<ChartView> NeuralNetworkView()
        {
            try
            {
                var listB = new List<BuySell>();
                var lines = 0;
                using (var reader = new StreamReader(@"resultsOhlc.csv"))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(';');

                        var a = values[0].Split(',');
                        if (!int.TryParse(a[0], out var sad))
                        {
                            continue;
                        }
                        var time = long.Parse(a[3]);
                        if (int.Parse(a[0]) == 1)
                        {
                            listB.Add(new BuySell
                            {
                                Position = "B",
                                Time = time
                            });
                        }
                        else if (int.Parse(a[1]) == 1)
                        {
                            listB.Add(new BuySell
                            {
                                Position = "S",
                                Time = time
                            });
                        }
                        lines++;
                    }
                }
                var closeArray = new double[lines];
                var lowArray = new double[lines];
                var highArray = new double[lines];
                var openArray = new double[lines];
                var timeArray = new long[lines];
                using (var reader = new StreamReader(@"cloePriceOhlc.csv"))
                {
                    var i = 0;
                    var j = 0;
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(';');
                        if (i < lines)
                        {
                            var row = values[0].Split(',');
                            if (!double.TryParse(row[0].Replace(".", ","), out var a))
                            {
                                continue;
                            }
                            closeArray[j] = double.Parse(row[3]);
                            openArray[j] = double.Parse(row[4]);
                            lowArray[j] = double.Parse(row[5]);
                            highArray[j] = double.Parse(row[6]);
                            timeArray[j] = long.Parse(row[8]); ;
                            j++;
                        }
                        i++;
                    }
                }
                var buyTrades = listB.Where(x => x.Position == "B").Take(500).ToList();
                var sellTrades = listB.Where(x => x.Position == "S").Take(500).ToList();
                var b = CombineArrays(timeArray, closeArray, closeArray, closeArray, closeArray);
                var series = new List<dynamic>()
                        {
                            CombineArrays(timeArray, openArray, highArray, lowArray, closeArray).Take(10000),
                            CombineArrays(timeArray, closeArray).Take(10000),
                            GetTradeSignals(buyTrades.Select(x => (long)x.Time).ToArray(), "B"),
                            GetTradeSignals(sellTrades.Select(x => (long)x.Time).ToArray(), "S")
                        };
                var chart = new ChartView { Series = series };
                return Task.FromResult(chart);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public Task<ChartView> NeuralNetworkViewSecond()
        {
            try
            {
                var listB = new List<BuySell>();
                var lines = 0;
                using (var reader = new StreamReader(@"predictionDecisionNew.csv"))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(';');

                        var a = values[0].Split(',');
                        if (!double.TryParse((a[0]), out var sad))
                        {
                            continue;
                        }
                        var time = long.Parse(a[3]);
                        var a1 = double.Parse(a[0]);
                        var a2 = double.Parse(a[1]);
                        if (a1 > 0.8)
                        {
                            listB.Add(new BuySell
                            {
                                Position = "B",
                                Time = time
                            });
                        }
                        else if (a2 > 0.8)
                        {
                            listB.Add(new BuySell
                            {
                                Position = "S",
                                Time = time
                            });
                        }
                        lines++;
                    }
                }
                var closeArray = new double[lines];
                var lowArray = new double[lines];
                var highArray = new double[lines];
                var openArray = new double[lines];
                var timeArray = new long[lines];
                var totalLines = 0;
                using (var reader = new StreamReader(@"cloePriceOhlc.csv"))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        totalLines++;
                    }
                }
                using (var reader = new StreamReader(@"cloePriceOhlc.csv"))
                {
                    var i = 0;
                    var j = 0;
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(';');
                        if (i >= totalLines - lines)
                        {
                            var row = values[0].Split(',');
                            if (!double.TryParse((row[3]), out var a))
                            {
                                continue;
                            }
                            closeArray[j] = double.Parse(row[3]);
                            openArray[j] = double.Parse(row[4]);
                            lowArray[j] = double.Parse(row[5]);
                            highArray[j] = double.Parse(row[6]);
                            timeArray[j] = long.Parse(row[8]);
                            j++;
                        }
                        i++;
                    }
                }
                var buyTrades = listB.Where(x => x.Position == "B").Take(500).ToList();
                var sellTrades = listB.Where(x => x.Position == "S").Take(500).ToList();
                var b = CombineArrays(timeArray, closeArray, closeArray, closeArray, closeArray);
                var series = new List<dynamic>()
                        {
                            CombineArrays(timeArray, openArray, highArray, lowArray, closeArray),
                            CombineArrays(timeArray, closeArray),
                            GetTradeSignals(buyTrades.Select(x => (long)x.Time).ToArray(), "B"),
                            GetTradeSignals(sellTrades.Select(x => (long)x.Time).ToArray(), "S")
                        };
                var chart = new ChartView { Series = series };
                return Task.FromResult(chart);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<ChartView> SimulateView(string symbol, string interval)
        {
            var listModel = new List<ChartWithEma>();

            var symbols = await _instrumentRepository.GetInstruments();

            var instrument = symbols.FirstOrDefault(x => x.Symbol == symbol);

            var chartTakeProfit = await _chartRepository.GetChartAsync(instrument.Symbol, interval, 150, instrument.Precision);
            var takeProfit = (decimal)Math.Round((chartTakeProfit.Quotations
                .Sum(x =>
                {
                    var low = x.Low * Math.Pow(10, instrument.Precision);
                    var high = x.High * Math.Pow(10, instrument.Precision);
                    var pipsDiff = Math.Round(Math.Abs(low - high), 1);
                    return pipsDiff / 10;
                }) / chartTakeProfit.Quotations.Count()), 2);
            var chart = await _chartRepository.GetChartAsync(instrument.Symbol, interval, 10000, instrument.Precision);
            var emaValue = 50;
            var emasH = Indicators.EMA(chart.Quotations.Select(x => x.Close).ToArray(), emaValue);
            for (int i = 0; i < emasH.Count(); i++)
            {
                chart.Quotations[i].AddEma(emasH[i]);
            }

            var quotations = chart.Quotations.ToArray();

            var timeArray = quotations.Select(x => x.Time.ConvertDateTimeToTicks()).ToArray();
            var openArray = quotations.Select(x => x.Open).ToArray();
            var highArray = quotations.Select(x => x.High).ToArray();
            var lowArray = quotations.Select(x => x.Low).ToArray();
            var closeArray = quotations.Select(x => x.Close).ToArray();

            listModel.AddRange(quotations.Select(
                x => new ChartWithEma
                {
                    Time = x.Time.ConvertDateTimeToTicks(),
                    Open = x.Open,
                    High = x.High,
                    Low = x.Low,
                    Close = x.Close,
                }));

            var buyTrades = listModel.Where(x => x.TypeAction == "B").ToList();
            var sellTrades = listModel.Where(x => x.TypeAction == "S").ToList();
            var c = CombineArrays(timeArray, openArray, highArray, lowArray, closeArray);

            var series = new List<dynamic>()
                        {
                            CombineArrays(timeArray, openArray, highArray, lowArray, closeArray),
                            CombineArrays(timeArray, emasH),
                                GetTradeSignals(buyTrades.Select(x => x.Time).ToArray(), "B"),
                                GetTradeSignals(sellTrades.Select(x => x.Time).ToArray(), "S")
                        };
            var chartView = new ChartView { Series = series };
            return chartView;
        }

        public async Task<ChartView> EmaView(string symbol, string interval, int candles, DateTime dateFrom, int ema)
        {
            var listModel = new List<ChartWithEma>();

            var instrument = await _instrumentRepository.GetInstrument(symbol);

            var chart = await _chartRepository.GetChartRangeTimeAsync(symbol, interval, dateFrom, candles, instrument.Precision);

            var timeArray = chart.Quotations.Select(x => x.Time.ConvertDateTimeToTicks()).ToArray();
            var openArray = chart.Quotations.Select(x => x.Open).ToArray();
            var highArray = chart.Quotations.Select(x => x.High).ToArray();
            var lowArray = chart.Quotations.Select(x => x.Low).ToArray();
            var closeArray = chart.Quotations.Select(x => x.Close).ToArray();

            var emasH = Indicators.EMA(closeArray, ema);

            listModel.AddRange(chart.Quotations.Select(
                x => new ChartWithEma
                {
                    Time = x.Time.ConvertDateTimeToTicks(),
                    Open = x.Open,
                    High = x.High,
                    Low = x.Low,
                    Close = x.Close,
                }));

            var buyTrades = listModel.Where(x => x.TypeAction == "B").ToList();
            var sellTrades = listModel.Where(x => x.TypeAction == "S").ToList();
            var c = CombineArrays(timeArray, openArray, highArray, lowArray, closeArray);

            var series = new List<dynamic>()
                        {
                            CombineArrays(timeArray, openArray, highArray, lowArray, closeArray),
                            CombineArrays(timeArray.Skip(ema).ToArray(), emasH.Skip(ema).ToArray()),
                        };
            var chartView = new ChartView { Series = series };
            return chartView;
        }

        public async Task<HistorySimulate> SimulateBuyEndDayStrategy(string symbol, string interval)
        {
            var instrument = await _instrumentRepository.GetInstrument(symbol);
            var chart = await _chartRepository.GetChartAsync(symbol, interval, 10000, instrument.Precision);
            var account = new Account(10000, "Test user");
            var model = new Simulator();
            var buyOnEnd = new BuyOnEndDayStrategy(instrument, account, 1000);
            return model.Simulate(chart, buyOnEnd, account);
        }

        private double[][] CombineArrays(long[] keys, params double[][] values)
        {
            var result = new List<double[]>();
            for (var k = 0; k < keys.Length; k++)
            {
                var data = new List<double>
                                       {
                                           keys[k]
                                       };
                for (var v = 0; v < values.Length; v++)
                {
                    data.Add(values[v][k]);
                }
                result.Add(data.ToArray());
            }
            return result.ToArray();
        }
        private object[] GetTradeSignals(long[] keys, string name)
        {
            var signals = new List<object>();
            for (var i = 0; i < keys.Length; i++)
            {
                signals.Add(new
                {
                    x = keys[i],
                    title = name
                });
            }
            return signals.ToArray();
        }
    }
}
