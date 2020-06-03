using System.Threading;
using System.Threading.Tasks;
using Domain.AggregatesModel.AccountAggregate;
using Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BackgroundTasks.Application.DomainEventHandlers.AccountAddedNewTransaction
{
    public class AccountModifiedTransactionDomainEventHandler : INotificationHandler<AccountModifiedTransactionDomainEvent>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ILoggerFactory _logger;

        public AccountModifiedTransactionDomainEventHandler(IAccountRepository accountRepository, ILoggerFactory logger)
        {
            _accountRepository = accountRepository;
            _logger = logger;
        }
        public async Task Handle(AccountModifiedTransactionDomainEvent accountAddedNewTransactionDomainEvent, CancellationToken cancellationToken)
        {
            _logger.CreateLogger<AccountAddedNewTransactionDomainEvent>()
                .LogTrace($"Account with Id: {accountAddedNewTransactionDomainEvent.Account.UserName} has been successfully modified stop loss - {accountAddedNewTransactionDomainEvent.Transaction.Position.StopLoss} " +
                          $"take profit - - {accountAddedNewTransactionDomainEvent.Transaction.Position.TakeProfit}, volumen  {accountAddedNewTransactionDomainEvent.Transaction.Position.Volumen}");

            await _accountRepository.ModifyTransactionAsync(accountAddedNewTransactionDomainEvent.Account, accountAddedNewTransactionDomainEvent.Transaction);
        }
    }
}
