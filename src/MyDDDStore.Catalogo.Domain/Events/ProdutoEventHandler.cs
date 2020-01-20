using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace MyDDDStore.Catalogo.Domain.Events
{
    public class ProdutoEventHandler : INotificationHandler<ProdutoAbaixoEstoqueEvent>
    {
        private readonly IProdutoRepository _produtoRepository;

        public ProdutoEventHandler(IProdutoRepository produtoRepository)
        {
            produtoRepository = _produtoRepository;
        }

        public async Task Handle(ProdutoAbaixoEstoqueEvent notification, CancellationToken cancellationToken)
        {
            var produto = await _produtoRepository.FindByID(notification.AggregateId);

            //Ações para quando o estoque estiver abaixo
        }
    }
}
