using System;
using System.Linq;
using System.Threading.Tasks;
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

    public class PlayGapCommandHandler : RequestHandler<PlayGapCommand>
    {
        private readonly INotifier _mail;
        private readonly ILogger<PlayGapCommandHandler> _logger;
        private readonly IInstrumentRepository _instrumentRepository;
        private readonly IChartRepository _chartRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IMediator _mediator;

        public PlayGapCommandHandler(INotifier mail, ILogger<PlayGapCommandHandler> logger, IInstrumentRepository instrumentRepository, IChartRepository chartRepository, IAccountRepository accountRepository, IMediator mediator)
        {
            _mail = mail;
            _logger = logger;
            _instrumentRepository = instrumentRepository;
            _chartRepository = chartRepository;
            _accountRepository = accountRepository;
            _mediator = mediator;
        }
        protected async override void Handle(PlayGapCommand request)
        {
            if ((DateTime.Now.Hour == 8 && DateTime.Now.Minute > 56) && DateTime.Now.Hour < 9) //EU
            {
                try
                {
                    var account = await _accountRepository.GetAccountAsync("11181613");
                    var instruments = await _instrumentRepository.GetInstruments();
                    var symbolsEu = request.SymbolsToPlay.BottomGapSymbolEu.Union(request.SymbolsToPlay.TopGapSymbolEu);
                    var instrumentsEu = instruments.Where(x => symbolsEu.Contains(x.Symbol)).ToList();
                    if (instrumentsEu.Any())
                    {
                        var chartsAsync = instrumentsEu.Select(async x => await _chartRepository.GetChartAsync(x.Symbol, "M1", 2250 * 5, x.Precision));
                        var charts = await Task.WhenAll(chartsAsync);
                        foreach (var chart in charts)
                        {
                            var instrument = instruments.FirstOrDefault(x => x.Symbol == chart.Symbol);
                            var gapStrategy = new GapStrategy(instrument, 1000, account);
                            gapStrategy.Play(chart);
                        }
                        _logger.LogInformation($"Play eu gap - {instrumentsEu.Aggregate("", (current, next) => current + ", " + next.Symbol)}");
                    }
                    await _mediator.DispatchDomainEventsAsync(account);
                    _logger.LogInformation($"No symbols eu to play gap");
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error gap");
                }
            }
            if ((DateTime.Now.Hour == 15 && DateTime.Now.Minute > 26) && (DateTime.Now.Hour >= 15 && DateTime.Now.Minute > 30)) // US
            {
                try
                {
                    var account = await _accountRepository.GetAccountAsync("11181613");
                    var instruments = await _instrumentRepository.GetInstruments();
                    var symbolsEu = request.SymbolsToPlay.BottomGapSymbolUs.Union(request.SymbolsToPlay.TopGapSymbolUs);
                    var instrumentsUs = instruments.Where(x => symbolsEu.Contains(x.Symbol)).ToList();
                    if (instrumentsUs.Any())
                    {
                        var chartsAsync = instrumentsUs.Select(async x => await _chartRepository.GetChartAsync(x.Symbol, "M1", 2250 * 5, x.Precision));
                        var charts = await Task.WhenAll(chartsAsync);
                        foreach (var chart in charts)
                        {
                            var instrument = instruments.FirstOrDefault(x => x.Symbol == chart.Symbol);
                            var gapStrategy = new GapStrategy(instrument, 1000, account);
                            gapStrategy.Play(chart);
                        }
                        _logger.LogInformation($"Play us gap - {instrumentsUs.Aggregate("", (current, next) => current + ", " + next.Symbol)}");
                    }
                    await _mediator.DispatchDomainEventsAsync(account);
                    _logger.LogInformation($"No symbols us to play gap");

                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error gap");
                }
            }
        }
    }
}
