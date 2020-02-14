using MyDDDStore.Core.Messages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyDDDStore.Core.DomainObjects;
using MyDDDStore.Pagamentos.Data;

namespace MyDDDStore.Core.Communication.Mediator
{
    public static class MediatorExtension
    {
        public static async Task PublicarEventos(this IMediatrHandler mediator, PagamentoContext ctx)
        {
            var domainEntities = ctx.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.Notifications != null && x.Entity.Notifications.Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.Notifications)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.ClearEvents());

            var tasks = domainEvents
                .Select(async (domainEvent) => {
                    await mediator.PublishEvent(domainEvent);
                });

            await Task.WhenAll(tasks);
        }
    }
}