using System.Collections.Generic;
using MediatR;

namespace BackgroundTasks.Application.Queries
{
    using Api.Application.Models;
    using Domain.AggregatesModel.InstrumentsAggregate;

    public class GetGapsRequest : IRequest<GapList>
    {
        public List<Instrument> Instruments { get; set; }
    }
}
