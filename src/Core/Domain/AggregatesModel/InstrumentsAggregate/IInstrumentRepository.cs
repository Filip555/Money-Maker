using System.Collections.Generic;

namespace Domain.AggregatesModel.InstrumentsAggregate
{
    using Domain.AggregatesModel.ChartAggregate;
    using System.Threading.Tasks;

    public interface IInstrumentRepository
    {
        Task<List<Instrument>> GetInstruments();
        Task<Instrument> GetInstrument(string symbol);
        Task<List<Dividend>> GetDividends(string isin);
        Task<List<Instrument>> GetSymbols();
        Task<List<Instrument>> GetIsinSymbols();
        Task<Pivot> GetPivot();
        Task<List<Instrument>> GetSymbolsParseHtml();
        Task<string[]> GetTechnicalAnalitycs(string symbol, int period = 60);
    }

}
