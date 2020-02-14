using MyDDDStore.Core.Messages;
using MyDDDStore.Core.Messages.CommomMessages.Notifications;
using System.Threading.Tasks;

namespace MyDDDStore.Core.Communication.Mediator
{
    public interface IMediatrHandler
    {
        Task PublishEvent<T>(T evento) where T : Event;

        Task<bool> SendCommand<T>(T comando) where T : Command;

        Task PublishNotification<T>(T notification) where T : DomainNotification;
    }
}
