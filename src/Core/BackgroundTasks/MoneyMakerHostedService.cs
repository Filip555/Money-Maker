using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BackgroundTasks
{
    using Api.Application.Models;
    using BackgroundTasks.Application.Commands;
    using BackgroundTasks.Application.Queries;

    public class MoneyMakerHostedService : IHostedService
    {
        private List<Timer> _insideTimers = new List<Timer>();
        private readonly IMediator _mediator;
        ILogger<MoneyMakerHostedService> _logger;

        public MoneyMakerHostedService(IMediator mediator,
                                       ILogger<MoneyMakerHostedService> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                var instruments = await _mediator.Send(new GetInstrumentsToPlayRequest());
                var symbolsToPlayInsideBar = await _mediator.Send(new GetSymbolsToPlayInsideBarRequest { Instruments = instruments });
                var instrumentsGap = await _mediator.Send(new GetGapsRequest());
                _insideTimers.Add(new Timer(PlayInsideBar, symbolsToPlayInsideBar, 0, 2000));
                _insideTimers.Add(new Timer(PlayWalkingStopLoss, null, 0, 2000));
                _insideTimers.Add(new Timer(PlayGap, instrumentsGap, 0, 2000));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "MoneyMakerHostedService");
            }
        }

        async void PlayGap(object symbolsToPlay)
          => await _mediator.Send(new PlayGapCommand { SymbolsToPlay = symbolsToPlay as GapList });

        async void PlayInsideBar(object symbolsToPlay)
          => await _mediator.Send(new PlayInsideBarCommand { InsideBars = symbolsToPlay as List<InsideBarView> });

        async void PlayWalkingStopLoss(object obj)
          => await _mediator.Send(new PlayWalkingStopLossCommand());

        public Task StopAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;
        public void StopAsyncTimers()
        {
            foreach (var timer in _insideTimers)
            {
                timer?.Change(Timeout.Infinite, 0);
            }
            _insideTimers = new List<Timer>();
        }
    }
}