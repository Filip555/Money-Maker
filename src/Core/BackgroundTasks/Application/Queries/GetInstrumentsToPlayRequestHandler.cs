using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BackgroundTasks.Application.Queries
{
    using Domain.AggregatesModel.InstrumentsAggregate;

    public class GetInstrumentsToPlayRequestHandler : IRequestHandler<GetInstrumentsToPlayRequest, List<Instrument>>
    {
        private readonly IInstrumentRepository _instrumentRepository;
        private readonly ILogger<GetInstrumentsToPlayRequestHandler> _logger;

        public GetInstrumentsToPlayRequestHandler(IInstrumentRepository instrumentRepository, ILogger<GetInstrumentsToPlayRequestHandler> logger)
        {
            _instrumentRepository = instrumentRepository;
            _logger = logger;
        }
        public async Task<List<Instrument>> Handle(GetInstrumentsToPlayRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var instruments = await _instrumentRepository.GetInstruments();
                return instruments.Where(
                        x => (x.GroupName == "Major" || x.GroupName == "Minor") || x.Symbol == "DE30" || x.Symbol == "US500" || x.Symbol == "US100" || x.Symbol == "GOLD"
                             || x.Symbol == "W20")
                            .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetInstrumentsToPlayRequestHandler");
                throw ex;
            }
        }
    }
}
