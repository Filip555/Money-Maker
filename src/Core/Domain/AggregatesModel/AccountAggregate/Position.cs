
namespace Domain.AggregatesModel.AccountAggregate
{
    public class Position
    {
        public double OpenPrice { get; protected set; }
        public double? TakeProfit { get; protected set; }
        public double? StopLoss { get; protected set; }
        public decimal Volumen { get; protected set; }

        public Position(double openPrice, decimal volumen)
        {
            OpenPrice = openPrice;
            Volumen = volumen;
        }

        public void SetTakeProfit(double takeProfit)
        {
            TakeProfit = takeProfit;
        }

        public void SetStopLoss(double stopLoss)
        {
            StopLoss = stopLoss;
        }

        public void ChangeTakeProfit(double? takeProfit)
        {
            TakeProfit = takeProfit;
        }

        public void ChangeStopLoss(double? stopLoss)
        {
            StopLoss = stopLoss;
        }

        public void ChangeVolumen(decimal volumen)
        {
            Volumen = volumen;
        }
    }
}
