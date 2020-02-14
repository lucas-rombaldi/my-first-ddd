using MediatR;
using MyDDDStore.Core.Communication.Mediator;
using MyDDDStore.Core.Messages.CommomMessages.IntegrationEvents;
using MyDDDStore.Vendas.Application.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyDDDStore.Vendas.Application.Events
{
    public class PedidoEventHandler :
        INotificationHandler<PedidoItemAdicionadoEvent>,
        INotificationHandler<PedidoRascunhoIniciadoEvent>,
        INotificationHandler<PedidoAtualizadoEvent>,
        INotificationHandler<VoucherAplicadoPedidoEvent>,
        INotificationHandler<PedidoEstoqueRejeitadoEvent>,
        INotificationHandler<PedidoPagamentoRealizadoEvent>,
        INotificationHandler<PedidoPagamentoRecusadoEvent>
    {
        private readonly IMediatrHandler _mediatrHandler;

        public PedidoEventHandler(IMediatrHandler mediatrHandler)
        {
            _mediatrHandler = mediatrHandler;
        }

        public Task Handle(PedidoItemAdicionadoEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;            
        }

        public Task Handle(PedidoRascunhoIniciadoEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task Handle(PedidoAtualizadoEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task Handle(VoucherAplicadoPedidoEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public async Task Handle(PedidoEstoqueRejeitadoEvent message, CancellationToken cancellationToken)
        {
            await _mediatrHandler.SendCommand(new CancelarProcessamentoPedidoCommand(message.PedidoId, message.ClienteId));
        }

        public async Task Handle(PedidoPagamentoRealizadoEvent message, CancellationToken cancellationToken)
        {
            await _mediatrHandler.SendCommand(new FinalizarPedidoCommand(message.PedidoId, message.ClienteId));            
        }

        public async Task Handle(PedidoPagamentoRecusadoEvent message, CancellationToken cancellationToken)
        {
            await _mediatrHandler.SendCommand(new CancelarProcessamentoPedidoEstornarEstoqueCommand(message.PedidoId, message.ClienteId));
        }
    }
}
