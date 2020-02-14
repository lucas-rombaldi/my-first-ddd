using MediatR;
using MyDDDStore.Core.Communication.Mediator;
using MyDDDStore.Core.DomainObjects.DTO;
using MyDDDStore.Core.Extensions;
using MyDDDStore.Core.Messages;
using MyDDDStore.Core.Messages.CommomMessages.IntegrationEvents;
using MyDDDStore.Core.Messages.CommomMessages.Notifications;
using MyDDDStore.Vendas.Application.Events;
using MyDDDStore.Vendas.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyDDDStore.Vendas.Application.Commands
{
    public class PedidoCommandHandler :
        IRequestHandler<AdicionarItemPedidoCommand, bool>,
        IRequestHandler<AtualizarItemPedidoCommand, bool>,
        IRequestHandler<RemoverItemPedidoCommand, bool>,
        IRequestHandler<AplicarVoucherPedidoCommand, bool>,
        IRequestHandler<IniciarPedidoCommand, bool>,
        IRequestHandler<FinalizarPedidoCommand, bool>,
        IRequestHandler<CancelarProcessamentoPedidoEstornarEstoqueCommand, bool>,
        IRequestHandler<CancelarProcessamentoPedidoCommand, bool>
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IMediatrHandler _mediatorHandler;

        public PedidoCommandHandler(IPedidoRepository pedidoRepository, IMediatrHandler mediatorHandler)
        {
            _pedidoRepository = pedidoRepository;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<bool> Handle(AdicionarItemPedidoCommand message, CancellationToken cancellationToken)
        {
            if (!ValidarComando(message)) return false;

            var pedido = await _pedidoRepository.FindDraftByClienteID(message.ClienteId);
            var pedidoItem = new PedidoItem(message.ProdutoId, message.Nome, message.Quantidade, message.ValorUnitario);

            if (pedido == null)
            {
                pedido = Pedido.PedidoFactory.NovoPedidoRascunho(message.ClienteId);
                pedido.AdicionarItem(pedidoItem);

                _pedidoRepository.Insert(pedido);
                pedido.AddEvent(new PedidoRascunhoIniciadoEvent(message.ClienteId, message.ProdutoId));
            }
            else
            {
                var pedidoItemExistente = pedido.PedidoItemExistente(pedidoItem);
                pedido.AdicionarItem(pedidoItem);

                if (pedidoItemExistente)
                {
                    _pedidoRepository.UpdateItem(pedido.PedidoItems.FirstOrDefault(p => p.ProdutoId == pedidoItem.ProdutoId));
                }
                else
                {
                    _pedidoRepository.InsertItem(pedidoItem);
                }

                pedido.AddEvent(new PedidoAtualizadoEvent(pedido.ClienteId, pedido.Id, pedido.ValorTotal));
            }

            pedido.AddEvent(new PedidoItemAdicionadoEvent(pedido.ClienteId, pedido.Id, message.ProdutoId, message.Quantidade, message.ValorUnitario, message.Nome));
            return await _pedidoRepository.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(AtualizarItemPedidoCommand message, CancellationToken cancellationToken)
        {
            if (!ValidarComando(message)) return false;

            var pedido = await _pedidoRepository.FindDraftByClienteID(message.ClienteId);

            if (pedido == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification("pedido", "Pedido não encontrado!"));
                return false;
            }

            var pedidoItem = await _pedidoRepository.FindItemByPedido(pedido.Id, message.ProdutoId);

            if (!pedido.PedidoItemExistente(pedidoItem))
            {
                await _mediatorHandler.PublishNotification(new DomainNotification("pedido", "Item do Pedido não encontrado!"));
                return false;
            }

            pedido.AtualizarUnidades(pedidoItem, message.Quantidade);

            pedido.AddEvent(new PedidoAtualizadoEvent(pedido.ClienteId, pedido.Id, pedido.ValorTotal));

            pedido.AddEvent(new PedidoItemPedidoAtualizadoEvent(pedido.ClienteId, pedido.Id, pedidoItem.ProdutoId, pedidoItem.Quantidade));

            _pedidoRepository.UpdateItem(pedidoItem);
            _pedidoRepository.Update(pedido);

            return await _pedidoRepository.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(RemoverItemPedidoCommand message, CancellationToken cancellationToken)
        {
            if (!ValidarComando(message)) return false;

            var pedido = await _pedidoRepository.FindDraftByClienteID(message.ClienteId);

            if (pedido == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification("pedido", "Pedido não encontrado!"));
                return false;
            }

            var pedidoItem = await _pedidoRepository.FindItemByPedido(pedido.Id, message.ProdutoId);

            if (!pedido.PedidoItemExistente(pedidoItem))
            {
                await _mediatorHandler.PublishNotification(new DomainNotification("pedido", "Item do Pedido não encontrado!"));
                return false;
            }

            pedido.RemoverItem(pedidoItem);

            pedido.AddEvent(new PedidoAtualizadoEvent(pedido.ClienteId, pedido.Id, pedido.ValorTotal));
            pedido.AddEvent(new PedidoItemRemovidoEvent(pedido.ClienteId, pedido.Id, pedidoItem.ProdutoId));

            _pedidoRepository.RemoveItem(pedidoItem);
            _pedidoRepository.Update(pedido);

            return await _pedidoRepository.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(AplicarVoucherPedidoCommand message, CancellationToken cancellationToken)
        {
            if (!ValidarComando(message)) return false;

            var pedido = await _pedidoRepository.FindDraftByClienteID(message.ClienteId);

            if (pedido == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification("pedido", "Pedido não encontrado!"));
                return false;
            }

            var voucher = await _pedidoRepository.FindVoucherByCodigo(message.CodigoVoucher);

            if (voucher == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification("pedido", "Voucher não encontrado!"));
                return false;
            }

            var voucherAplicacaoValidation = pedido.AplicarVoucher(voucher);
            if (!voucherAplicacaoValidation.IsValid)
            {
                foreach (var error in voucherAplicacaoValidation.Errors)
                {
                    await _mediatorHandler.PublishNotification(new DomainNotification(error.ErrorCode, error.ErrorMessage));
                }

                return false;
            }

            pedido.AddEvent(new PedidoAtualizadoEvent(message.ClienteId, pedido.Id, pedido.ValorTotal));
            pedido.AddEvent(new VoucherAplicadoPedidoEvent(message.ClienteId, pedido.Id, voucher.Id));            

            _pedidoRepository.Update(pedido);

            return await _pedidoRepository.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(IniciarPedidoCommand message, CancellationToken cancellationToken)
        {
            if (!ValidarComando(message)) return false;

            var pedido = await _pedidoRepository.FindDraftByClienteID(message.ClienteId);
            pedido.IniciarPedido();

            var itensList = new List<Item>();
            pedido.PedidoItems.ForEach(i => itensList.Add(new Item { Id = i.ProdutoId, Quantidade = i.Quantidade }));
            var listaProdutosPedido = new ListaProdutosPedido { PedidoId = pedido.Id, Itens = itensList };

            pedido.AddEvent(new PedidoIniciadoEvent(pedido.ClienteId, pedido.Id, pedido.ValorTotal, listaProdutosPedido, message.NomeCartao, message.NumeroCartao, message.ExpiracaoCartao, message.CvvCartao));

            _pedidoRepository.Update(pedido);
            return await _pedidoRepository.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(FinalizarPedidoCommand message, CancellationToken cancellationToken)
        {
            var pedido = await _pedidoRepository.FindByID(message.PedidoId);

            if (pedido == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification("pedido", "Pedido não encontrado!"));
                return false;
            }

            pedido.FinalizarPedido();

            pedido.AddEvent(new PedidoFinalizadoEvent(message.PedidoId));
            return await _pedidoRepository.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(CancelarProcessamentoPedidoEstornarEstoqueCommand message, CancellationToken cancellationToken)
        {
            var pedido = await _pedidoRepository.FindByID(message.PedidoId);

            if (pedido == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification("pedido", "Pedido não encontrado!"));
                return false;
            }

            var itensList = new List<Item>();
            pedido.PedidoItems.ForEach(i => itensList.Add(new Item { Id = i.ProdutoId, Quantidade = i.Quantidade }));
            var listaProdutosPedido = new ListaProdutosPedido { PedidoId = pedido.Id, Itens = itensList };

            pedido.AddEvent(new PedidoProcessamentoCanceladoEvent(pedido.Id, pedido.ClienteId, listaProdutosPedido));
            pedido.TornarRascunho();

            return await _pedidoRepository.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(CancelarProcessamentoPedidoCommand message, CancellationToken cancellationToken)
        {
            var pedido = await _pedidoRepository.FindByID(message.PedidoId);

            if (pedido == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification("pedido", "Pedido não encontrado!"));
                return false;
            }

            pedido.TornarRascunho();

            return await _pedidoRepository.UnitOfWork.Commit();
        }

        private bool ValidarComando(Command message)
        {
            if (message.IsValid()) return true;

            foreach (var error in message.ValidationResult.Errors)
            {
                _mediatorHandler.PublishNotification(new DomainNotification(message.MessageType, error.ErrorMessage));
            }

            return false;
        }
    }
}
