using System.Collections.Generic;
using MediatR;

namespace BackgroundTasks.Application.Commands
{
    using Api.Application.Models;


    public class PlayInsideBarCommand : IRequest
    {
        public List<InsideBarView> InsideBars { get; set; }
    }
}
