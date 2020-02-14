using System.Threading.Tasks;
using MyDDDStore.Core.DomainObjects.DTO;

namespace MyDDDStore.Pagamentos.Business
{
    public interface IPagamentoService
    {
        Task<Transacao> RealizarPagamentoPedido(PagamentoPedido pagamentoPedido);
    }
}