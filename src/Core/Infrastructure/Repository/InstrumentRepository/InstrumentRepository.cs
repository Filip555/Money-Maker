
namespace Infrastructure.Repository.InstrumentRepository
{
    using Domain.AggregatesModel.ChartAggregate;
    using Domain.AggregatesModel.InstrumentsAggregate;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class InstrumentRepository : IInstrumentRepository
    {
        public Task<List<Dividend>> GetDividends(string isin)
        {
            throw new System.NotImplementedException();
        }

        public Task<Instrument> GetInstrument(string symbol)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<Instrument>> GetInstruments()
        {
            throw new System.NotImplementedException();
        }

        public Task<List<Instrument>> GetIsinSymbols()
        {
            throw new System.NotImplementedException();
        }

        public Task<Pivot> GetPivot()
        {
            throw new System.NotImplementedException();
        }

        public Task<List<Instrument>> GetSymbols()
        {
            throw new System.NotImplementedException();
        }

        public Task<List<Instrument>> GetSymbolsParseHtml()
        {
            throw new System.NotImplementedException();
        }

        public Task<string[]> GetTechnicalAnalitycs(string symbol, int period = 60)
        {
            throw new System.NotImplementedException();
        }
    }
}
