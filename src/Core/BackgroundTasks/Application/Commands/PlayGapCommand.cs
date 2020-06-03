using Api.Application.Models;
using MediatR;

namespace BackgroundTasks.Application.Commands
{
    public class PlayGapCommand : IRequest
    {
        public GapList SymbolsToPlay { get; set; }
    }
}
