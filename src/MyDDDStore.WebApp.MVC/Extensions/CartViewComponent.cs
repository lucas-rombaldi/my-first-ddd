using Microsoft.AspNetCore.Mvc;
using MyDDDStore.Vendas.Application.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyDDDStore.WebApp.MVC.Extensions
{
    public class CartViewComponent : ViewComponent
    {
        private readonly IPedidoQueries _pedidoQueries;

        //TODO: Obter cliente logado
        protected Guid ClienteId = Guid.Parse("918811AC-3B41-4143-8E94-E8F955986F13");

        public CartViewComponent(IPedidoQueries pedidoQueries)
        {
            _pedidoQueries = pedidoQueries;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var carrinho = await _pedidoQueries.ObterCarrinhoCliente(ClienteId);
            var itens = carrinho?.Items.Count ?? 0;

            return View(itens);
        }
    }
}
