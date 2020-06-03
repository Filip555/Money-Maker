using Domain.SeedWork;
using MediatR;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class MediatorExtension
    {
        public static async Task DispatchDomainEventsAsync(this IMediator mediator, Entity entity)
        {
            var domainEvents = entity.DomainEvents.ToList();
            entity.ClearDomainEvents();
            foreach (var domainEvent in domainEvents)
                await mediator.Publish(domainEvent);
        }
    }
}
