using System;
using System.Threading.Tasks;

namespace Infrastructure.Repository.Account
{
    using Domain.AggregatesModel.AccountAggregate;
    using Domain.AggregatesModel.InstrumentsAggregate;

    public class AccountRepository : IAccountRepository
    {
        public Task<Account> GetAccountAsync(string idAccount = "")
        {
            throw new NotImplementedException();
        }

        public Task MakeCloseTransactionBuyAsync(string symbol, long orderId, decimal volume, string idAccount = "")
        {
            throw new NotImplementedException();
        }

        public Task MakeCloseTransactionSellAsync(string symbol, long orderId, decimal volume, string idAccount = "")
        {
            throw new NotImplementedException();
        }

        public Task MakeTransactionBuyAsync(double? stoploss, double? takeProfit, double volume, Instrument instrument, string commnet, string idAccount = "")
        {
            throw new NotImplementedException();
        }

        public Task MakeTransactionSellAsync(double? stoploss, double? takeProfit, double volume, Instrument instrument, string commnet, string idAccount = "")
        {
            throw new NotImplementedException();
        }

        public Task ModifyTransactionAsync(Account account, Transaction transaction)
        {
            throw new NotImplementedException();
        }
    }
}
