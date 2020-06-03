using CsvHelper;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SimulatorNew.Generator
{
    using Domain.AggregatesModel.ChartAggregate;
    using Domain.AggregatesModel.InstrumentsAggregate;

    public class CsvCreatorModelMm
    {
        private readonly IInstrumentRepository _instrumentRepository;
        private readonly IChartRepository _chartRepository;

        public CsvCreatorModelMm(IInstrumentRepository instrumentRepository, IChartRepository chartRepository)
        {
            _instrumentRepository = instrumentRepository;
            _chartRepository = chartRepository;
        }

        public async Task GenerateClosePriceWithVolume(string symbol, string interval)
        {
            var instrument = await _instrumentRepository.GetInstrument(symbol);
            var chart = await _chartRepository.GetChartMonthlyAsync(symbol, interval, 6, instrument.Precision);

            var maxhour = chart.Quotations.Max(x => x.Time.Hour);
            var minhour = chart.Quotations.Min(x => x.Time.Hour);

            var maxMinute = chart.Quotations.Max(x => x.Time.Minute);
            var minMinute = chart.Quotations.Min(x => x.Time.Minute);

            var maxDay = chart.Quotations.Max(x => (int)x.Time.DayOfWeek);
            var minDay = chart.Quotations.Min(x => (int)x.Time.DayOfWeek);

            using (var textWriter = new StreamWriter(@"cloePriceOhlc.csv"))
            {
                var writer = new CsvWriter(textWriter, CultureInfo.CurrentCulture);
                writer.Configuration.Delimiter = ",";
                writer.WriteField("Hour");
                writer.WriteField("Minute");
                writer.WriteField("Day");
                writer.WriteField("Close");
                writer.WriteField("Open");
                writer.WriteField("Low");
                writer.WriteField("High");
                writer.WriteField("Volume");
                writer.WriteField("Time");
                writer.NextRecord();

                foreach (var item in chart.Quotations)
                {
                    var hourNormalization = (item.Time.Hour - minhour) / (maxhour - minhour);
                    var minuteNormalization = (item.Time.Minute - minMinute) / (maxMinute - minMinute);
                    var dayNormalization = ((int)item.Time.DayOfWeek - minDay) / (maxDay - minDay);

                    writer.WriteField(hourNormalization);
                    writer.WriteField(minuteNormalization);
                    writer.WriteField(dayNormalization);
                    writer.WriteField(item.Close.ToString().Replace(",", "."));
                    writer.WriteField(item.Open.ToString().Replace(",", "."));
                    writer.WriteField(item.Low.ToString().Replace(",", "."));
                    writer.WriteField(item.High.ToString().Replace(",", "."));
                    writer.WriteField(item.Volume);
                    writer.WriteField(item.Time);
                    writer.NextRecord();
                }
            }
            using (var textWriter = new StreamWriter(@"resultsOhlc.csv"))
            {
                var writer = new CsvWriter(textWriter, CultureInfo.CurrentCulture);
                writer.Configuration.Delimiter = ",";
                writer.WriteField("Buy");
                writer.WriteField("Sell");
                writer.WriteField("None");
                writer.WriteField("Time");
                writer.NextRecord();

                for (var i = 120; i < chart.Quotations.Count - 15; i++)
                {
                    var futurePrices = chart.Quotations.GetRange(i, 15).Select(x => new { x.Close, x.Low, x.High }).ToArray();
                    var futureFirst = futurePrices.First();
                    var futureMin = futurePrices.Min(x => x.Low);
                    var futureMax = futurePrices.Max(x => x.High);


                    //bad signal high pin
                    var badSignal = false;
                    foreach (var item in futurePrices)
                    {
                        if (item.High - item.Low > 40)
                        {
                            badSignal = true;
                        }
                    }
                    for (int j = 0; j <= futurePrices.Count() - 2; j++)
                    {
                        if (Math.Abs(futurePrices[j].Close - futurePrices[j + 1].Close) > 40)
                        {
                            badSignal = true;
                        }
                    }


                    var time = chart.Quotations[i].Time;

                    if (badSignal)
                    {
                        writer.WriteField(false);
                        writer.WriteField(false);
                        writer.WriteField(true);
                        writer.WriteField(time);
                        writer.NextRecord();
                        continue;
                    }

                    if (futureMax - futureFirst.High > 20 && futureFirst.Low - futureMin < 10)
                    {
                        writer.WriteField(true);
                        writer.WriteField(false);
                        writer.WriteField(false);
                        writer.WriteField(time);
                        writer.NextRecord();
                    }
                    else if (futureFirst.Low - futureMin > 20 && futureMax - futureFirst.High < 10)
                    {
                        writer.WriteField(false);
                        writer.WriteField(true);
                        writer.WriteField(false);
                        writer.WriteField(time);
                        writer.NextRecord();
                    }
                    else
                    {
                        writer.WriteField(false);
                        writer.WriteField(false);
                        writer.WriteField(true);
                        writer.WriteField(time);
                        writer.NextRecord();
                    }
                }
            }
        }

        public async Task GenerateClosePriceWithVolumeWithHightAndLow(string symbol, string interval)
        {
            var instrument = await _instrumentRepository.GetInstrument("DE30");
            var chart = await _chartRepository.GetChartMonthlyAsync(symbol, interval, 6, instrument.Precision);

            var maxhour = chart.Quotations.Max(x => x.Time.Hour);
            var minhour = chart.Quotations.Min(x => x.Time.Hour);

            var maxMinute = chart.Quotations.Max(x => x.Time.Minute);
            var minMinute = chart.Quotations.Min(x => x.Time.Minute);

            var maxDay = chart.Quotations.Max(x => (int)x.Time.DayOfWeek);
            var minDay = chart.Quotations.Min(x => (int)x.Time.DayOfWeek);

            using (var textWriter = new StreamWriter(@"cloePriceOhlc.csv"))
            {
                var writer = new CsvWriter(textWriter, CultureInfo.CurrentCulture);
                writer.Configuration.Delimiter = ",";
                writer.WriteField("Hour");
                writer.WriteField("Minute");
                writer.WriteField("Day");
                writer.WriteField("Close");
                writer.WriteField("Open");
                writer.WriteField("Low");
                writer.WriteField("High");
                writer.WriteField("Volume");
                writer.WriteField("Time");
                writer.NextRecord();

                foreach (var item in chart.Quotations)
                {
                    var hourNormalization = (item.Time.Hour - minhour) / (maxhour - minhour);
                    var minuteNormalization = (item.Time.Minute - minMinute) / (maxMinute - minMinute);
                    var dayNormalization = ((int)item.Time.DayOfWeek - minDay) / (maxDay - minDay);

                    writer.WriteField(hourNormalization);
                    writer.WriteField(minuteNormalization);
                    writer.WriteField(dayNormalization);
                    writer.WriteField(item.Close.ToString().Replace(",", "."));
                    writer.WriteField(item.Open.ToString().Replace(",", "."));
                    writer.WriteField(item.Low.ToString().Replace(",", "."));
                    writer.WriteField(item.High.ToString().Replace(",", "."));
                    writer.WriteField(item.Volume);
                    writer.WriteField(item.Time);
                    writer.NextRecord();
                }
            }
            using (var textWriter = new StreamWriter(@"resultsOhlc.csv"))
            {
                var writer = new CsvWriter(textWriter, CultureInfo.CurrentCulture);
                writer.Configuration.Delimiter = ",";
                writer.WriteField("Buy");
                writer.WriteField("Sell");
                writer.WriteField("None");
                writer.WriteField("Time");
                writer.NextRecord();

                for (var i = 120; i < chart.Quotations.Count - 15; i++)
                {
                    //Buy
                    var futurePrices = chart.Quotations.GetRange(i, 15).Select(x => new { x.Close, x.Low, x.High }).ToArray();
                    var pastPricesLow = chart.Quotations.GetRange(i - 5, 5).Select(x => x.Low).ToArray();
                    var futurePricesLow = chart.Quotations.GetRange(i + 1, 5).Select(x => x.Low).ToArray();
                    var futureFirst = futurePrices.First();
                    var pastMinLow = pastPricesLow.Min();
                    var futureMinLow = futurePricesLow.Min();

                    //Sell
                    var pastPricesHigh = chart.Quotations.GetRange(i - 5, 5).Select(x => x.High).ToArray();
                    var futurePricesHigh = chart.Quotations.GetRange(i + 1, 5).Select(x => x.High).ToArray();
                    var pastMaxHigh = pastPricesHigh.Max();
                    var futureMaxHigh = futurePricesHigh.Max();

                    var time = chart.Quotations[i].Time;

                    if (futureFirst.Low < pastMinLow && futurePrices.Max(x => x.High) - futureFirst.Close > 12)
                    {
                        writer.WriteField(true);
                        writer.WriteField(false);
                        writer.WriteField(false);
                        writer.WriteField(time);
                        writer.NextRecord();
                    }
                    else if (futureFirst.High > pastMaxHigh && futureFirst.Close - futurePrices.Min(x => x.Low) > 12)
                    {
                        writer.WriteField(false);
                        writer.WriteField(true);
                        writer.WriteField(false);
                        writer.WriteField(time);
                        writer.NextRecord();
                    }
                    else
                    {
                        writer.WriteField(false);
                        writer.WriteField(false);
                        writer.WriteField(true);
                        writer.WriteField(time);
                        writer.NextRecord();
                    }
                }
            }
        }

        public async Task GenerateClosePriceLongTermProfit(string symbol, string interval)
        {
            var instrument = await _instrumentRepository.GetInstrument(symbol);
            var chart = await _chartRepository.GetChartMonthlyAsync(symbol, interval, 6, instrument.Precision);

            var maxhour = chart.Quotations.Max(x => x.Time.Hour);
            var minhour = chart.Quotations.Min(x => x.Time.Hour);

            var maxMinute = chart.Quotations.Max(x => x.Time.Minute);
            var minMinute = chart.Quotations.Min(x => x.Time.Minute);

            var maxDay = chart.Quotations.Max(x => (int)x.Time.DayOfWeek);
            var minDay = chart.Quotations.Min(x => (int)x.Time.DayOfWeek);

            using (var textWriter = new StreamWriter(@"cloePriceOhlc.csv"))
            {
                var writer = new CsvWriter(textWriter, CultureInfo.CurrentCulture);
                writer.Configuration.Delimiter = ",";
                writer.WriteField("Hour");
                writer.WriteField("Minute");
                writer.WriteField("Day");
                writer.WriteField("Close");
                writer.WriteField("Open");
                writer.WriteField("Low");
                writer.WriteField("High");
                writer.WriteField("Volume");
                writer.WriteField("Time");
                writer.NextRecord();

                foreach (var item in chart.Quotations)
                {
                    var hourNormalization = (item.Time.Hour - minhour) / (maxhour - minhour);
                    var minuteNormalization = (item.Time.Minute - minMinute) / (maxMinute - minMinute);
                    var dayNormalization = ((int)item.Time.DayOfWeek - minDay) / (maxDay - minDay);

                    writer.WriteField(hourNormalization);
                    writer.WriteField(minuteNormalization);
                    writer.WriteField(dayNormalization);
                    writer.WriteField(item.Close.ToString().Replace(",", "."));
                    writer.WriteField(item.Open.ToString().Replace(",", "."));
                    writer.WriteField(item.Low.ToString().Replace(",", "."));
                    writer.WriteField(item.High.ToString().Replace(",", "."));
                    writer.WriteField(item.Volume);
                    writer.WriteField(item.Time);
                    writer.NextRecord();
                }
            }
            using (var textWriter = new StreamWriter(@"resultsOhlc.csv"))
            {
                var writer = new CsvWriter(textWriter, CultureInfo.CurrentCulture);
                writer.Configuration.Delimiter = ",";
                writer.WriteField("Buy");
                writer.WriteField("Sell");
                writer.WriteField("None");
                writer.WriteField("Time");
                writer.NextRecord();

                for (var i = 120; i < chart.Quotations.Count - 90; i++)
                {
                    var futurePrices = chart.Quotations.GetRange(i, 90).Select(x => new { x.Close, x.Low, x.High }).ToArray();
                    var futureFirst = futurePrices.First();
                    var futureMin = futurePrices.Min(x => x.Low);
                    var futureMax = futurePrices.Max(x => x.High);


                    //bad signal high pin
                    var badSignal = false;
                    foreach (var item in futurePrices)
                    {
                        if (item.High - item.Low > 30)
                        {
                            badSignal = true;
                        }
                    }
                    for (int j = 0; j <= futurePrices.Count() - 2; j++)
                    {
                        if (Math.Abs(futurePrices[j].Close - futurePrices[j + 1].Close) > 40)
                        {
                            badSignal = true;
                        }
                    }


                    var time = chart.Quotations[i].Time;

                    if (badSignal)
                    {
                        writer.WriteField(false);
                        writer.WriteField(false);
                        writer.WriteField(true);
                        writer.WriteField(time);
                        writer.NextRecord();
                        continue;
                    }

                    if (futureMax - futureFirst.High > 60 && futureFirst.Low - futureMin < 20)
                    {
                        writer.WriteField(true);
                        writer.WriteField(false);
                        writer.WriteField(false);
                        writer.WriteField(time);
                        writer.NextRecord();
                    }
                    else if (futureFirst.Low - futureMin > 60 && futureMax - futureFirst.High < 20)
                    {
                        writer.WriteField(false);
                        writer.WriteField(true);
                        writer.WriteField(false);
                        writer.WriteField(time);
                        writer.NextRecord();
                    }
                    else
                    {
                        writer.WriteField(false);
                        writer.WriteField(false);
                        writer.WriteField(true);
                        writer.WriteField(time);
                        writer.NextRecord();
                    }
                }
            }
        }


        public async Task GenerateResults(string symbol)
        {
            var instrument = await _instrumentRepository.GetInstrument(symbol);

            var quotations = new List<Quotation>();
            using (var reader = new StreamReader(@"DEUIDXEUR.csv"))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');

                    var a = values[0].Split(',');

                    if (double.TryParse(a[2], out var ab))
                    {
                        var year = int.Parse(a[0].Substring(0, 4));
                        var month = int.Parse(a[0].Substring(4, 2));
                        var day = int.Parse(a[0].Substring(6, 2));
                        var date = a[0];
                        var timeStamp = a[1].Split(":");
                        var hour = int.Parse(timeStamp[0]);
                        var minutes = int.Parse(timeStamp[1]);
                        var unixTime = new DateTime(year, month, day, hour, minutes, 0);
                        var open = double.Parse(a[2]) * Math.Pow(10, instrument.Precision - 1);
                        var high = double.Parse(a[3]) * Math.Pow(10, instrument.Precision - 1);
                        var low = double.Parse(a[4]) * Math.Pow(10, instrument.Precision - 1);
                        var close = double.Parse(a[5]) * Math.Pow(10, instrument.Precision - 1);
                        var volumen = double.Parse(a[6]);
                        quotations.Add(new Quotation(unixTime, open, low, high, close, volumen));
                    }

                }
            }

            using (var textWriter = new StreamWriter(@"resultsOhlc.csv"))
            {
                var writer = new CsvWriter(textWriter, CultureInfo.CurrentCulture);
                writer.Configuration.Delimiter = ",";
                writer.WriteField("Buy");
                writer.WriteField("Sell");
                writer.WriteField("None");
                writer.WriteField("Time");
                writer.NextRecord();

                for (var i = 120; i < quotations.Count - 180; i++)
                {
                    var futurePrices = quotations.GetRange(i, 180).Select(x => new { x.Close, x.Low, x.High }).ToArray();
                    var futureFirst = futurePrices.First();
                    var futureMin = futurePrices.Min(x => x.Low);
                    var futureMax = futurePrices.Max(x => x.High);

                    var time = quotations[i].Time;

                    if (futureMax - futureFirst.High > 50 && futureFirst.Low - futureMin < 20)
                    {
                        writer.WriteField(1);
                        writer.WriteField(0);
                        writer.WriteField(0);
                        writer.WriteField(time);
                        writer.NextRecord();
                    }
                    else if (futureFirst.Low - futureMin > 50 && futureMax - futureFirst.High < 20)
                    {
                        writer.WriteField(0);
                        writer.WriteField(1);
                        writer.WriteField(0);
                        writer.WriteField(time);
                        writer.NextRecord();
                    }
                    else
                    {
                        writer.WriteField(0);
                        writer.WriteField(0);
                        writer.WriteField(1);
                        writer.WriteField(time);
                        writer.NextRecord();
                    }
                }
            }
            using (var textWriter = new StreamWriter(@"cloePriceOhlc.csv"))
            {
                var writer = new CsvWriter(textWriter, CultureInfo.CurrentCulture);
                writer.Configuration.Delimiter = ",";
                writer.WriteField("Hour");
                writer.WriteField("Minute");
                writer.WriteField("Day");
                writer.WriteField("Close");
                writer.WriteField("Open");
                writer.WriteField("Low");
                writer.WriteField("High");
                writer.WriteField("Volume");
                writer.WriteField("Time");
                writer.NextRecord();

                foreach (var item in quotations)
                {


                    writer.WriteField(0);
                    writer.WriteField(0);
                    writer.WriteField(0);
                    writer.WriteField(item.Close.ToString().Replace(",", "."));
                    writer.WriteField(item.Open.ToString().Replace(",", "."));
                    writer.WriteField(item.Low.ToString().Replace(",", "."));
                    writer.WriteField(item.High.ToString().Replace(",", "."));
                    writer.WriteField(item.Volume);
                    writer.WriteField(item.Time);
                    writer.NextRecord();
                }
            }
        }
    }
}
