using Domain.AggregatesModel.AccountAggregate;
using MediatR;

namespace Domain.Events
{
    public class AccountClosedTransactionDomainEvent : INotification
    {
        public Account Account { get; private set; }
        public long TransactionId { get; private set; }
        public double ClosePrice { get; private set; }

        public AccountClosedTransactionDomainEvent(Account account, long transactionId, double closePrice)
        {
            Account = account;
            TransactionId = transactionId;
            ClosePrice = closePrice;
        }
    }
}
