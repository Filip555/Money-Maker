using Domain.AggregatesModel.ChartAggregate;
using Domain.AggregatesModel.InstrumentsAggregate;
using Domain.Common;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Application.Queries
{
    using Api.Application.Models;

    public class SignalQueries : ISignalQueries
    {
        private readonly IInstrumentRepository _instrumentRepository;
        private readonly IChartRepository _chartRepository;
        private readonly ILogger<SignalQueries> _logger;

        public SignalQueries(IInstrumentRepository instrumentRepository, IChartRepository chartRepository, ILogger<SignalQueries> logger)
        {
            _instrumentRepository = instrumentRepository;
            _chartRepository = chartRepository;
            _logger = logger;
        }

        public async Task<InsideBarView> GetInsideBar(string symbol)
        {
            var instrument = await _instrumentRepository.GetInstrument(symbol);
            var chart = await _chartRepository.GetChartAsync(symbol, "D1", 10, instrument.Precision);
            chart.SetInsideBar(DateTime.Now.AddDays(-1));
            if (chart.InsideBar != null)
            {
                return new InsideBarView
                {
                    HighInsideBar = chart.InsideBar.HighInsideBar,
                    LowInsideBar = chart.InsideBar.LowInsideBar,
                    HighMotherInsideBar = chart.InsideBar.HighMotherInsideBar,
                    LowMotherInsideBar = chart.InsideBar.LowMotherInsideBar,
                    Symbol = chart.Symbol,
                    Side = chart.InsideBar.Side
                };
            }
            return new InsideBarView();
        }

        public async Task<IEnumerable<InsideBarView>> GetInsideBars()
        {
            var instrumentsAsync = await _instrumentRepository.GetInstruments();
            var instruments = instrumentsAsync.Where(x => (x.GroupName == "Major" || x.GroupName == "Minor")
                                                              || x.Symbol == "DE30" || x.Symbol == "US500" || x.Symbol == "US100" || x.Symbol == "GOLD" || x.Symbol == "W20").ToList();
            var chartsAsync = instruments.Select(async x => await _chartRepository.GetChartAsync(x.Symbol, "D1", 10, x.Precision)).ToList();
            var charts = await Task.WhenAll(chartsAsync);
            var list = new List<InsideBarView>();
            foreach (var chart in charts)
            {
                chart.SetInsideBar(DateTime.Now.AddDays(-1));
                if (chart.InsideBar != null)
                {
                    list.Add(new InsideBarView
                    {
                        HighInsideBar = chart.InsideBar.HighInsideBar,
                        LowInsideBar = chart.InsideBar.LowInsideBar,
                        HighMotherInsideBar = chart.InsideBar.HighMotherInsideBar,
                        LowMotherInsideBar = chart.InsideBar.LowMotherInsideBar,
                        Symbol = chart.Symbol,
                        Side = chart.InsideBar.Side
                    });
                }
            }
            return list;
        }

        public async Task<GapList> GetGaps()
        {
            try
            {
                var instruments = await _instrumentRepository.GetInstruments();
                instruments = instruments.Where(x => (x.GroupName == "Major") || x.Symbol == "DE30" || x.Symbol == "US500" || x.Symbol == "US100" || x.Symbol == "GOLD").ToList();
                var chartsAsync = instruments.Select(async x => await _chartRepository.GetChartAsync(x.Symbol, "M1", 2250, x.Precision));
                var charts = await Task.WhenAll(chartsAsync);

                var bars = charts.Select(x => new
                {
                    symbol = x.Symbol,
                    highUs = x.Quotations.Where(w => w.IsInUsTimeFrame())?.DefaultIfEmpty().Max(m => m?.High ?? 0) ?? 0,
                    lowUs = x.Quotations.Where(w => w.IsInUsTimeFrame())?.DefaultIfEmpty().Min(m => m?.Low ?? 0) ?? 0,
                    highEu = x.Quotations.Where(w => w.IsInEuropeTimeFrame())?.DefaultIfEmpty().Max(m => m?.High ?? 0) ?? 0,
                    lowEu = x.Quotations.Where(w => w.IsInEuropeTimeFrame())?.DefaultIfEmpty().Min(m => m?.Low ?? 0) ?? 0,
                    lastQuotation = x.Quotations[x.Quotations.Count - 1]
                }).ToList();

                var model = new GapList
                {
                    TopGapSymbolEu = bars.Where(x => x.highEu < x.lastQuotation.Close).Select(x => x.symbol).ToList() ?? new List<string>(),
                    BottomGapSymbolEu = bars.Where(x => x.lowEu > x.lastQuotation.Close).Select(x => x.symbol).ToList() ?? new List<string>(),
                    TopGapSymbolUs = bars.Where(x => x.highUs < x.lastQuotation.Close).Select(x => x.symbol).ToList() ?? new List<string>(),
                    BottomGapSymbolUs = bars.Where(x => x.lowUs > x.lastQuotation.Close).Select(x => x.symbol).ToList() ?? new List<string>()
                };
                return await Task.FromResult(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetGaps");

                throw ex;
            }
        }

        public async Task<IEnumerable<string>> GetTheCheapestPolandCompanies(int periodInDays)
        {
            var instrumentsAsync = await _instrumentRepository.GetInstruments();
            var instruments = instrumentsAsync.Where(x => (x.CategoryName.Equals(CategoryName.Stock) && x.GroupName == "Poland"));
            var chartsAsync = instruments.Select(async x => await _chartRepository.GetChartAsync(x.Symbol, "D1", periodInDays + 20, x.Precision));
            var charts = await Task.WhenAll(chartsAsync);
            var cheapestSymbols = new List<string>();

            foreach (var item in charts)
            {
                var orderedItems = item.Quotations.OrderByDescending(x => x.Time).ToList();
                var daysToMinus = DateTime.Now.DayOfWeek == DayOfWeek.Sunday ? -2 : (DateTime.Now.DayOfWeek == DayOfWeek.Monday ? -3 : -1);
                var currentItem = orderedItems.FirstOrDefault(x => x.Time.Date == DateTime.Now.AddDays(daysToMinus).Date);
                if (currentItem != null)
                {
                    var lastElements = orderedItems.Take(periodInDays);
                    if (currentItem.Low < (lastElements.OrderBy(x => x.Low).Take(3).LastOrDefault()?.Low ?? 0))
                    {
                        cheapestSymbols.Add(item.Symbol);
                    }
                }
            }

            return await Task.FromResult(cheapestSymbols.AsEnumerable());
        }

        public async Task<IEnumerable<string>> GetTheMostExpensivePolandCompanies(int periodInDays)
        {
            var instrumentsAsync = await _instrumentRepository.GetInstruments();
            var instruments = instrumentsAsync.Where(x => (x.CategoryName.Equals(CategoryName.Stock) && x.GroupName == "Poland"));
            var chartsAsync = instruments.Select(async x => await _chartRepository.GetChartAsync(x.Symbol, "D1", periodInDays + 20, x.Precision));
            var charts = await Task.WhenAll(chartsAsync);
            var highestSymbols = new List<string>();
            foreach (var item in charts)
            {
                var orderedItems = item.Quotations.OrderByDescending(x => x.Time).ToList();
                var daysToMinus = DateTime.Now.DayOfWeek == DayOfWeek.Sunday ? -2 : (DateTime.Now.DayOfWeek == DayOfWeek.Monday ? -3 : -1);
                var currentItem = orderedItems.FirstOrDefault(x => x.Time.Date == DateTime.Now.AddDays(daysToMinus).Date);
                if (currentItem != null)
                {
                    var lastElements = orderedItems.Take(periodInDays);
                    if (currentItem.High > lastElements.OrderByDescending(x => x.High).Take(3).LastOrDefault().High)
                    {
                        highestSymbols.Add(item.Symbol);
                    }
                }
            }
            return await Task.FromResult(highestSymbols.AsEnumerable());
        }

        public async Task<IEnumerable<Instrument>> GetTheBestDiscountDividendCompanies(double discountRate)
        {
            try
            {
                var instruments = await _instrumentRepository.GetSymbolsParseHtml();
                var costEffectiveInstruments = new List<Instrument>();
                foreach (var instrument in instruments)
                {
                    var dividend = await _instrumentRepository.GetDividends(instrument.Isin);
                    instrument.AddDividends(dividend);
                    var priceRequired = new DiscountDividendModel(instrument.Dividends.Select(x => x.Price).ToArray(), discountRate).Value;
                    if (instrument.Valuation.BidPrice < priceRequired)
                    {
                        costEffectiveInstruments.Add(instrument);
                    }
                }
                return costEffectiveInstruments;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "GetTheBestDiscountDividendCompanies");
                throw e;
            }
        }

        public async Task<IEnumerable<Instrument>> GetTheBestSimpleDiscountDividendCompanies(double discountRate)
        {
            try
            {
                var instruments = await _instrumentRepository.GetSymbolsParseHtml();
                var costEffectiveInstruments = new List<Instrument>();
                foreach (var instrument in instruments)
                {
                    var dividend = await _instrumentRepository.GetDividends(instrument.Isin);
                    instrument.AddDividends(dividend);
                    var thisYearDywidend = instrument.GetLastDividend();
                    if (thisYearDywidend != null)
                    {
                        var priceRequired = new SimpleDiscountDividendModel(thisYearDywidend.Price, discountRate);
                        if (instrument.Valuation.BidPrice < priceRequired.Value)
                        {
                            costEffectiveInstruments.Add(instrument);
                        }
                    }
                }
                return costEffectiveInstruments;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "GetTheBestSimpleDiscountDividendCompanies");

                throw e;
            }
        }

        public async Task<Pivot> GetPivot()
            => await _instrumentRepository.GetPivot();

        public async Task<string[]> GetTechnicalAnalitycs(string symbol, int period)
            => await _instrumentRepository.GetTechnicalAnalitycs(symbol, period);
    }
}
