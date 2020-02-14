using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyDDDStore.Core.Communication.Mediator;
using MyDDDStore.Core.Messages.CommomMessages.Notifications;
using MyDDDStore.Vendas.Application.Queries;

namespace MyDDDStore.WebApp.MVC.Controllers
{
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoQueries _pedidoQueries;

        public PedidoController(IPedidoQueries pedidoQueries,
            INotificationHandler<DomainNotification> notifications,
            IMediatrHandler mediatorHandler) : base(notifications, mediatorHandler)
        {
            _pedidoQueries = pedidoQueries;
        }

        [Route("meus-pedidos")]
        public async Task<IActionResult> Index()
        {
            return View(await _pedidoQueries.ObterPedidosCliente(ClienteId));
        }
    }
}