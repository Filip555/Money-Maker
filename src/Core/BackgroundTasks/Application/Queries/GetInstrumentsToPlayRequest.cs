using System.Collections.Generic;
using MediatR;

namespace BackgroundTasks.Application.Queries
{
    using Domain.AggregatesModel.InstrumentsAggregate;

    public class GetInstrumentsToPlayRequest : IRequest<List<Instrument>>
    {
    }
}
