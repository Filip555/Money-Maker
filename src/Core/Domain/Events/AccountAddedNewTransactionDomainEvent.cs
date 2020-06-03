using Domain.AggregatesModel.AccountAggregate;
using MediatR;

namespace Domain.Events
{
    public class AccountAddedNewTransactionDomainEvent : INotification
    {
        public Transaction Transaction { get; private set; }
        public Account Account { get; private set; }

        public AccountAddedNewTransactionDomainEvent(Transaction transaction, Account account)
        {
            Transaction = transaction;
            Account = account;
        }
    }
}
