using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BackgroundTasks.Application.DomainEventHandlers.AccountAddedNewTransaction
{
    using Domain.AggregatesModel.AccountAggregate;
    using Domain.Events;

    public class AccountAddedNewTransactionDomainEventHandler : INotificationHandler<AccountAddedNewTransactionDomainEvent>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ILoggerFactory _logger;

        public AccountAddedNewTransactionDomainEventHandler(IAccountRepository accountRepository, ILoggerFactory logger)
        {
            _accountRepository = accountRepository;
            _logger = logger;
        }
        public async Task Handle(AccountAddedNewTransactionDomainEvent accountAddedNewTransactionDomainEvent, CancellationToken cancellationToken)
        {
            _logger.CreateLogger<AccountAddedNewTransactionDomainEvent>()
                .LogTrace($"Account with Id: {accountAddedNewTransactionDomainEvent.Account.UserName} has been successfully added new transaction with symbol {accountAddedNewTransactionDomainEvent.Transaction.Instrument.Symbol})");



            if (accountAddedNewTransactionDomainEvent.Transaction.TypeTransaction.Equals(TypeTransaction.Buy))
            {
                await _accountRepository.MakeTransactionBuyAsync(accountAddedNewTransactionDomainEvent.Transaction.Position.StopLoss,
                                             accountAddedNewTransactionDomainEvent.Transaction.Position.TakeProfit,
                                             (double)accountAddedNewTransactionDomainEvent.Transaction.Position.Volumen,
                                             accountAddedNewTransactionDomainEvent.Transaction.Instrument,
                                             "Automat buy",
                                             accountAddedNewTransactionDomainEvent.Account.UserName);
            }
            else if (accountAddedNewTransactionDomainEvent.Transaction.TypeTransaction.Equals(TypeTransaction.Sell))
            {
                await _accountRepository.MakeTransactionSellAsync(accountAddedNewTransactionDomainEvent.Transaction.Position.StopLoss,
                                                    accountAddedNewTransactionDomainEvent.Transaction.Position.TakeProfit,
                                                    (double)accountAddedNewTransactionDomainEvent.Transaction.Position.Volumen,
                                                    accountAddedNewTransactionDomainEvent.Transaction.Instrument,
                                                    "Automat sell",
                                                    accountAddedNewTransactionDomainEvent.Account.UserName);
            }
        }
    }
}
