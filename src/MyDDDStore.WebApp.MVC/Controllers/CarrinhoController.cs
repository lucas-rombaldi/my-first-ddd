using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyDDDStore.Catalogo.Application.Services;
using MyDDDStore.Core.Communication.Mediator;
using MyDDDStore.Core.Messages.CommomMessages.Notifications;
using MyDDDStore.Vendas.Application.Commands;
using MyDDDStore.Vendas.Application.Queries;
using MyDDDStore.Vendas.Application.Queries.ViewModels;

namespace MyDDDStore.WebApp.MVC.Controllers
{
    public class CarrinhoController : ControllerBase
    {
        private readonly IProdutoAppService _produtoAppService;
        private readonly IMediatrHandler _mediatrHandler;
        private readonly IPedidoQueries _pedidoQueries;

        public CarrinhoController(INotificationHandler<DomainNotification> notifications, IProdutoAppService produtoAppService, IMediatrHandler mediatrHandler, IPedidoQueries pedidoQueries)
            : base(notifications, mediatrHandler)
        {
            _produtoAppService = produtoAppService;
            _mediatrHandler = mediatrHandler;
            _pedidoQueries = pedidoQueries;
        }

        [Route("meu-carrinho")]
        public async Task<IActionResult> Index()
        {
            return View(await _pedidoQueries.ObterCarrinhoCliente(ClienteId));
        }

        [HttpPost]
        [Route("meu-carrinho")]
        public async Task<IActionResult> AdicionarItem(Guid id, int quantidade)
        {
            var produto = await _produtoAppService.FindByID(id);
            if (produto == null) return BadRequest();

            if (produto.QuantidadeEstoque < quantidade)
            {
                TempData["Erro"] = "Produto com estoque insuficiente";
                return RedirectToAction("ProdutoDetalhe", "Vitrine", new { id });
            }

            var command = new AdicionarItemPedidoCommand(ClienteId, produto.Id, produto.Nome, quantidade, produto.Valor);
            await _mediatrHandler.SendCommand(command);

            if (ValidOperation())
            {
                return RedirectToAction("Index");
            }

            TempData["Erros"] = GetErrorMessages();
            return RedirectToAction("ProdutoDetalhe", "Vitrine", new { id });
        }
        
        [HttpPost]
        [Route("remover-item")]
        public async Task<IActionResult> RemoverItem(Guid id)
        {
            var produto = await _produtoAppService.FindByID(id);
            if (produto == null) return BadRequest();

            var command = new RemoverItemPedidoCommand(ClienteId, id);
            await _mediatrHandler.SendCommand(command);

            if (ValidOperation())
            {
                return RedirectToAction("Index");
            }

            return View("Index", await _pedidoQueries.ObterCarrinhoCliente(ClienteId));
        }

        [HttpPost]
        [Route("atualizar-item")]
        public async Task<IActionResult> AtualizarItem(Guid id, int quantidade)
        {
            var produto = await _produtoAppService.FindByID(id);
            if (produto == null) return BadRequest();

            var command = new AtualizarItemPedidoCommand(ClienteId, id, quantidade);
            await _mediatrHandler.SendCommand(command);

            if (ValidOperation())
            {
                return RedirectToAction("Index");
            }

            return View("Index", await _pedidoQueries.ObterCarrinhoCliente(ClienteId));
        }


        [HttpPost]
        [Route("aplicar-voucher")]
        public async Task<IActionResult> AplicarVoucher(string voucherCodigo)
        {
            var command = new AplicarVoucherPedidoCommand(ClienteId, voucherCodigo);
            await _mediatrHandler.SendCommand(command);

            if (ValidOperation())
            {
                return RedirectToAction("Index");
            }

            return View("Index", await _pedidoQueries.ObterCarrinhoCliente(ClienteId));
        }

        [Route("resumo-da-compra")]
        public async Task<IActionResult> ResumoDaCompra()
        {
            return View(await _pedidoQueries.ObterCarrinhoCliente(ClienteId));
        }

        [HttpPost]
        [Route("iniciar-pedido")]
        public async Task<IActionResult> IniciarPedido(CarrinhoViewModel carrinhoViewModel)
        {
            var carrinho = await _pedidoQueries.ObterCarrinhoCliente(ClienteId);

            var command = new IniciarPedidoCommand(carrinho.PedidoId, ClienteId, carrinho.ValorTotal, carrinhoViewModel.Pagamento.NomeCartao,
                carrinhoViewModel.Pagamento.NumeroCartao, carrinhoViewModel.Pagamento.ExpiracaoCartao, carrinhoViewModel.Pagamento.CvvCartao);

            await _mediatrHandler.SendCommand(command);

            if (ValidOperation())
            {
                return RedirectToAction("Index", "Pedido");
            }

            return View("ResumoDaCompra", await _pedidoQueries.ObterCarrinhoCliente(ClienteId));
        }
    }
}