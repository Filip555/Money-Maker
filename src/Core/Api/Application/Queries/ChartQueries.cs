using System.Threading.Tasks;

namespace Api.Application.Queries
{
    using Domain.AggregatesModel.ChartAggregate;
    using Domain.AggregatesModel.InstrumentsAggregate;

    public class ChartQueries : IChartQueries
    {
        private readonly IChartRepository _chartRepository;
        private readonly IInstrumentRepository _instrumentRepository;

        public ChartQueries(IChartRepository chartRepository, IInstrumentRepository instrumentRepository)
        {
            _chartRepository = chartRepository;
            _instrumentRepository = instrumentRepository;
        }
        public async Task<Chart> GetChart(string symbol, string interval, int candles)
        {
            var instrument = await _instrumentRepository.GetInstrument(symbol);
            return await _chartRepository.GetChartAsync(symbol, interval, candles, instrument.Precision);
        }
    }
}
