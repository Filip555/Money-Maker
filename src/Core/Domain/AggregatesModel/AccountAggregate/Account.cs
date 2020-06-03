using Domain.AggregatesModel.InstrumentsAggregate;
using Domain.Events;
using Domain.Exceptions;
using Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.AggregatesModel.AccountAggregate
{
    public class Account : Entity, IAggregateRoot
    {
        public decimal Deposite { get; private set; }
        public List<Transaction> Transactions { get; private set; }
        public string UserName { get; private set; }
        public Account(decimal deposite, string userName, List<Transaction> transactions = null)
        {
            Transactions = transactions ?? new List<Transaction>();
            Deposite = deposite;
            UserName = userName;
        }
        public void AddTransaction(long orderId, Instrument instrument, double openPrice, double? stopLoss, double? takeProfit, decimal volumen, TypeTransaction typeTransaction, DateTime dateOpen, string info)
        {
            var position = new Position(openPrice, volumen);
            if (orderId <= 0) orderId = GetMaxOrderId();
            var transaction = new Transaction(orderId, instrument, position, typeTransaction, dateOpen, info);
            if (stopLoss != null && stopLoss > 0) transaction.Position.SetStopLoss(stopLoss ?? 0);
            if (takeProfit != null && takeProfit > 0) transaction.Position.SetTakeProfit(takeProfit ?? 0);
            Transactions.Add(transaction);
            AddDomainEvent(new AccountAddedNewTransactionDomainEvent(transaction, this));
        }

        public void ModifyTransaction(long orderId, double? stopLoss, double? takeProfit, decimal volumen)
        {
            var transaction = Transactions.FirstOrDefault(x => x.OrderId == orderId);
            if (transaction == null) throw new DomainException($"There is no transaction with id {orderId}");
            transaction.Position.ChangeStopLoss(stopLoss);
            transaction.Position.ChangeTakeProfit(takeProfit);
            transaction.Position.ChangeVolumen(volumen);
            AddDomainEvent(new AccountModifiedTransactionDomainEvent(this, transaction));
        }

        private long GetMaxOrderId()
            => Transactions.Count > 0 ? Transactions.Max(x => x.OrderId) + 1 : 1;

        public void CloseTransaction(long transactionID, double closePrice, DateTime dateTime)
        {
            var transaction = Transactions.FirstOrDefault(x => x.OrderId == transactionID);
            transaction.CloseTransaction(closePrice, dateTime);
            Deposite += transaction.Profit;

            AddDomainEvent(new AccountClosedTransactionDomainEvent(this, transactionID, closePrice));
        }

        public List<Transaction> GetClosedTransactions()
            => Transactions.Where(x => !x.IsOpen()).ToList();

        public List<Transaction> GetOpenTransactions()
            => Transactions.Where(x => x.IsOpen()).ToList();

        public Transaction GetOpenTransaction(string symbol)
            => Transactions.FirstOrDefault(x => x.Instrument?.Symbol == symbol && x.IsOpen());
    }
}
