
namespace Domain.Strategies
{
    using Domain.AggregatesModel.ChartAggregate;

    public interface IStrategy
    {
        void Play(Chart chart);
    }
}
