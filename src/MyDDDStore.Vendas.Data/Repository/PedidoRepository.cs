using Microsoft.EntityFrameworkCore;
using MyDDDStore.Core.Data;
using MyDDDStore.Vendas.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyDDDStore.Vendas.Data.Repository
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly VendasContext _context;

        public PedidoRepository(VendasContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<Pedido> FindByID(Guid id)
        {
            return await _context.Pedidos.FindAsync(id);
        }

        public async Task<IEnumerable<Pedido>> FindByClientID(Guid clienteId)
        {
            return await _context.Pedidos.AsNoTracking().Where(p => p.ClienteId == clienteId).ToListAsync();
        }

        public async Task<Pedido> FindDraftByClienteID(Guid clienteId)
        {
            var pedido = await _context.Pedidos.FirstOrDefaultAsync(p => p.ClienteId == clienteId && p.PedidoStatus == PedidoStatus.Rascunho);
            if (pedido == null) return null;

            await _context.Entry(pedido)
                .Collection(i => i.PedidoItems).LoadAsync();

            if (pedido.VoucherId != null)
            {
                await _context.Entry(pedido)
                    .Reference(i => i.Voucher).LoadAsync();
            }

            return pedido;
        }

        public void Insert(Pedido pedido)
        {
            _context.Pedidos.Add(pedido);
        }

        public void Update(Pedido pedido)
        {
            _context.Pedidos.Update(pedido);
        }


        public async Task<PedidoItem> FindItemByID(Guid id)
        {
            return await _context.PedidoItems.FindAsync(id);
        }

        public async Task<PedidoItem> FindItemByPedido(Guid pedidoId, Guid produtoId)
        {
            return await _context.PedidoItems.FirstOrDefaultAsync(p => p.ProdutoId == produtoId && p.PedidoId == pedidoId);
        }

        public void InsertItem(PedidoItem pedidoItem)
        {
            _context.PedidoItems.Add(pedidoItem);
        }

        public void UpdateItem(PedidoItem pedidoItem)
        {
            _context.PedidoItems.Update(pedidoItem);
        }

        public void RemoveItem(PedidoItem pedidoItem)
        {
            _context.PedidoItems.Remove(pedidoItem);
        }

        public async Task<Voucher> FindVoucherByCodigo(string codigo)
        {
            return await _context.Vouchers.FirstOrDefaultAsync(p => p.Codigo == codigo);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}