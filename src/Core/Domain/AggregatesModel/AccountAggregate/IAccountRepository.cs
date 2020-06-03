using System.Threading.Tasks;

namespace Domain.AggregatesModel.AccountAggregate
{
    using Domain.AggregatesModel.InstrumentsAggregate;

    public interface IAccountRepository
    {
        Task<Account> GetAccountAsync(string idAccount = "");
        Task MakeTransactionBuyAsync(double? stoploss, double? takeProfit, double volume, Instrument instrument, string commnet, string idAccount = "");
        Task MakeTransactionSellAsync(double? stoploss, double? takeProfit, double volume, Instrument instrument, string commnet, string idAccount = "");
        Task MakeCloseTransactionBuyAsync(string symbol, long orderId, decimal volume, string idAccount = "");
        Task MakeCloseTransactionSellAsync(string symbol, long orderId, decimal volume, string idAccount = "");
        Task ModifyTransactionAsync(Account account, Transaction transaction);
    }
}
