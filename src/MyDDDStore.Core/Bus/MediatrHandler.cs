using MediatR;
using MyDDDStore.Core.Messages;
using System.Threading.Tasks;

namespace MyDDDStore.Core.Bus
{
    public class MediatrHandler : IMediatrHandler
    {
        private readonly IMediator _mediator;

        public MediatrHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task PublishEvent<T>(T evento) where T : Event
        {
            await _mediator.Publish(evento);
        }
    }

    public interface IMediatrHandler
    {
        Task PublishEvent<T>(T evento) where T : Event;
    }
}
