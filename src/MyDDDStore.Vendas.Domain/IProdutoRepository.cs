using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyDDDStore.Core.Data;

namespace MyDDDStore.Vendas.Domain
{
    public interface IPedidoRepository : IRepository<Pedido>
    {
        Task<Pedido> FindByID(Guid id);
        Task<IEnumerable<Pedido>> FindByClientID(Guid clienteId);
        Task<Pedido> FindDraftByClienteID(Guid clienteId);
        void Insert(Pedido pedido);
        void Update(Pedido pedido);

        Task<PedidoItem> FindItemByID(Guid id);
        Task<PedidoItem> FindItemByPedido(Guid pedidoId, Guid produtoId);
        void InsertItem(PedidoItem pedidoItem);
        void UpdateItem(PedidoItem pedidoItem);
        void RemoveItem(PedidoItem pedidoItem);

        Task<Voucher> FindVoucherByCodigo(string codigo);
    }
}