using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Application.Queries
{
    using Api.Application.Models;
    using Domain.AggregatesModel.ChartAggregate;
    using Domain.AggregatesModel.InstrumentsAggregate;

    public interface ISignalQueries
    {
        Task<IEnumerable<InsideBarView>> GetInsideBars();
        Task<InsideBarView> GetInsideBar(string symbol);
        Task<GapList> GetGaps();
        Task<IEnumerable<string>> GetTheCheapestPolandCompanies(int periodInDays);
        Task<IEnumerable<string>> GetTheMostExpensivePolandCompanies(int periodInDays);
        Task<IEnumerable<Instrument>> GetTheBestDiscountDividendCompanies(double discountRate);
        Task<IEnumerable<Instrument>> GetTheBestSimpleDiscountDividendCompanies(double discountRate);
        Task<Pivot> GetPivot();
        Task<string[]> GetTechnicalAnalitycs(string symbol, int period);
    }
}
