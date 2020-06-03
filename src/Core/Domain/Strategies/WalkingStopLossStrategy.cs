using Domain.AggregatesModel.AccountAggregate;

namespace Domain.Strategies
{
    public class WalkingStopLossStrategy
    {
        public Account Account { get; private set; }
        public decimal WalkingStopLossPercent { get; private set; } // 4 optimal

        public WalkingStopLossStrategy(Account account, decimal walkingStopLossPercent)
        {
            Account = account;
            WalkingStopLossPercent = walkingStopLossPercent;
        }

        public void Play()
        {
            var transactions = Account.GetOpenTransactions();
            foreach (var item in transactions)
            {
                var investedValue = item.Instrument.ComputeValueTransaction(item.Position.Volumen, item.Instrument.Valuation.BidPrice);
                var profitValuePercent = (decimal)item.Profit / investedValue * 100m;
                if (profitValuePercent > WalkingStopLossPercent)
                {
                    var stopLossShouldBe = profitValuePercent - (decimal)WalkingStopLossPercent; // jest 5 procen ustawic na 3 % stoploss
                    var pipsy = item.TypeTransaction.Equals(TypeTransaction.Buy) ? item.ComputeProfitInPips(item.Instrument.Valuation.BidPrice)
                                                                                 : item.ComputeProfitInPips(item.Instrument.Valuation.AskPrice);


                    var stopLoss = WalkingStopLossPercent * pipsy / profitValuePercent;
                    var wartoscPipsowOdKupnaPrognoza = stopLossShouldBe * pipsy / profitValuePercent;

                    if ((double)wartoscPipsowOdKupnaPrognoza > (item.Position.StopLoss * -1d))
                    {
                        Account.ModifyTransaction(item.OrderId, (double?)stopLoss, null, item.Position.Volumen);
                    }
                }
            }
        }
    }
}

