using System;
using System.Linq;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BackgroundTasks.Application.Commands
{
    using Domain.AggregatesModel.AccountAggregate;
    using Domain.AggregatesModel.ChartAggregate;
    using Domain.AggregatesModel.InstrumentsAggregate;
    using Domain.Strategies;
    using Infrastructure;
    using Infrastructure.Services;

    public class PlayInsideBarCommandHandler : RequestHandler<PlayInsideBarCommand>
    {
        private readonly INotifier _mail;
        private readonly ILogger<PlayInsideBarCommandHandler> _logger;
        private readonly IAccountRepository _accountRepository;
        private readonly IChartRepository _chartRepository;
        private readonly IInstrumentRepository _instrumentRepository;
        private readonly IMediator _mediator;

        public PlayInsideBarCommandHandler(INotifier mail, ILogger<PlayInsideBarCommandHandler> logger, IAccountRepository accountRepository, IChartRepository chartRepository, IInstrumentRepository instrumentRepository, IMediator mediator)
        {
            _accountRepository = accountRepository;
            _chartRepository = chartRepository;
            _instrumentRepository = instrumentRepository;
            _mediator = mediator;
            _mail = mail;
            _logger = logger;
        }

        protected async override void Handle(PlayInsideBarCommand request)
        {
            if (DateTime.Now.Second >= 58)
            {
                try
                {
                    if (request.InsideBars.Count > 0)
                    {
                        var account = await _accountRepository.GetAccountAsync("11181613");
                        var instruments = await _instrumentRepository.GetInstruments();
                        foreach (var insideBars in request.InsideBars)
                        {
                            var instrument = instruments.FirstOrDefault(x => x.Symbol == insideBars.Symbol);
                            var insideBar = new InsideBarStrategy(instrument, 1000, account);
                            var chart = await _chartRepository.GetChartAsync(insideBars.Symbol, "M1", 4000, instrument.Precision);
                            insideBar.Play(chart);
                            await _mediator.DispatchDomainEventsAsync(account); 
                            _logger.LogInformation($"Play inside bar {insideBar.Instrument.Symbol}");
                        }
                    }
                    else
                    {
                        _logger.LogInformation("No inside bars");
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error Inside bar");
                }
            }
        }
    }
}
