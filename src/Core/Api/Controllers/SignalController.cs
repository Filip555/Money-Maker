using Api.Application.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Controllers
{
    using Api.Application.Models;
    using Domain.AggregatesModel.ChartAggregate;
    using Domain.AggregatesModel.InstrumentsAggregate;

    public class SignalController : Controller
    {
        private readonly ISignalQueries _signalQueries;

        public SignalController(ISignalQueries signalQueries)
        {
            _signalQueries = signalQueries;
        }

        [HttpGet("getInsideBars")]
        public async Task<IEnumerable<InsideBarView>> GetInsideBars()
         => await _signalQueries.GetInsideBars();

        [HttpGet("getInsideBar/symbol/{symbol}")]
        public async Task<InsideBarView> GetInsideBar(string symbol)
         => await _signalQueries.GetInsideBar(symbol);

        [HttpGet("getGaps")]
        public async Task<GapList> GetGaps()
            => await _signalQueries.GetGaps();

        [HttpGet("getTheMostExpensivePolandCompanies/periodInDays/{periodInDays}")]
        public async Task<IEnumerable<string>> GetTheMostExpensivePolandCompanies(int periodInDays)
            => await _signalQueries.GetTheMostExpensivePolandCompanies(periodInDays);

        [HttpGet("getTheCheapestPolandCompanies/periodInDays/{periodInDays}")]
        public async Task<IEnumerable<string>> GetTheCheapestPolandCompanies(int periodInDays)
            => await _signalQueries.GetTheCheapestPolandCompanies(periodInDays);

        [HttpGet("getTheBestDiscountDividendCompanies/{discountRate}")]
        public async Task<IEnumerable<Instrument>> GetTheBestDiscountDividendCompanies(double discountRate)
            => await _signalQueries.GetTheBestDiscountDividendCompanies(discountRate);

        [HttpGet("getTheBestSimpleDiscountDividendCompanies/{discountRate}")]
        public async Task<IEnumerable<Instrument>> GetTheBestSimpleDiscountDividendCompanies(double discountRate)
            => await _signalQueries.GetTheBestSimpleDiscountDividendCompanies(discountRate);

        [HttpGet("getPivot")]
        public async Task<Pivot> GetPivot()
            => await _signalQueries.GetPivot();

        /// Works only for indeks - DE30, EURJPY, EURGBP, USDCAD, EURUSD, AUDUSD
        [HttpGet("technicalAnalitycs/{symbol}/{period}")]
        public async Task<string[]> GetTechnicalAnalitycs(string symbol, int period = 60)
            => await _signalQueries.GetTechnicalAnalitycs(symbol, period);
    }
}