using System;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BackgroundTasks.Application.Commands
{
    using Domain.AggregatesModel.AccountAggregate;
    using Domain.Strategies;
    using Infrastructure;
    using Infrastructure.Services;

    public class PlayWalkingStopLossCommandHandler : RequestHandler<PlayWalkingStopLossCommand>
    {
        private readonly INotifier _mail;
        private readonly ILogger<PlayWalkingStopLossCommandHandler> _logger;
        private readonly IAccountRepository _accountRepository;
        private readonly IMediator _mediator;

        public PlayWalkingStopLossCommandHandler(INotifier mail, ILogger<PlayWalkingStopLossCommandHandler> logger, IAccountRepository accountRepository, IMediator mediator)
        {
            _mail = mail;
            _logger = logger;
            _accountRepository = accountRepository;
            _mediator = mediator;
        }

        protected async override void Handle(PlayWalkingStopLossCommand request)
        {
            if (DateTime.Now.Second >= 58)
            {
                try
                {
                    string[] ids = { "11181613" };
                    for (int i = 0; i < ids.Length; i++)
                    {
                        var account = await _accountRepository.GetAccountAsync(ids[i]);
                        var walkingStopLossStrategy = new WalkingStopLossStrategy(account, 4);
                        walkingStopLossStrategy.Play();
                        await _mediator.DispatchDomainEventsAsync(account);
                        _logger.LogInformation($"Play walking stop loss. Account - {ids[i]}");
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "PlayWalkingStopLossCommandHandler");
                }
            }
        }
    }
}
