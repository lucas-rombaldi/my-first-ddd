using MediatR;
using MyDDDStore.Core.Messages;
using MyDDDStore.Core.Messages.CommomMessages.Notifications;
using System.Threading.Tasks;

namespace MyDDDStore.Core.Communication.Mediator
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

        public async Task PublishNotification<T>(T notification) where T : DomainNotification
        {
            await _mediator.Publish(notification);
        }

        public async Task<bool> SendCommand<T>(T comando) where T : Command
        {
            return await _mediator.Send(comando);
        }
    }
}
