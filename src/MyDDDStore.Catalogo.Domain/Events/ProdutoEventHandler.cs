using MediatR;
using MyDDDStore.Core.Communication.Mediator;
using MyDDDStore.Core.Messages.CommomMessages.IntegrationEvents;
using System.Threading;
using System.Threading.Tasks;

namespace MyDDDStore.Catalogo.Domain.Events
{
    public class ProdutoEventHandler : 
        INotificationHandler<ProdutoAbaixoEstoqueEvent>,
        INotificationHandler<PedidoIniciadoEvent>,
        INotificationHandler<PedidoProcessamentoCanceladoEvent>
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IMediatrHandler _mediatrHandler;
        private readonly IEstoqueService _estoqueService;

        public ProdutoEventHandler(IProdutoRepository produtoRepository, IEstoqueService estoqueService, IMediatrHandler mediatrHandler)
        {
            _produtoRepository = produtoRepository;
            _estoqueService = estoqueService;
            _mediatrHandler = mediatrHandler;
        }

        public async Task Handle(ProdutoAbaixoEstoqueEvent notification, CancellationToken cancellationToken)
        {
            var produto = await _produtoRepository.FindByID(notification.AggregateId);

            //Ações para quando o estoque estiver abaixo
        }

        public async Task Handle(PedidoIniciadoEvent message, CancellationToken cancellationToken)
        {
            var result = await _estoqueService.DebitarListaProdutosPedido(message.ProdutosPedido);

            if (result)
            {
                await _mediatrHandler.PublishEvent(new PedidoEstoqueConfirmadoEvent(message.PedidoId, message.ClienteId, message.Total, message.ProdutosPedido, message.NomeCartao, message.NumeroCartao, message.ExpiracaoCartao, message.CvvCartao));
            }
            else
            {
                await _mediatrHandler.PublishEvent(new PedidoEstoqueRejeitadoEvent(message.PedidoId, message.ClienteId));
            }
        }

        public async Task Handle(PedidoProcessamentoCanceladoEvent message, CancellationToken cancellationToken)
        {
            await _estoqueService.ReporListaProdutosPedido(message.ProdutosPedido);
        }
    }
}
