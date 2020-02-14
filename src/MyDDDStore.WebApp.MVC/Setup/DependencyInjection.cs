using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MyDDDStore.Catalogo.Application.Services;
using MyDDDStore.Catalogo.Data;
using MyDDDStore.Catalogo.Data.Repository;
using MyDDDStore.Catalogo.Domain;
using MyDDDStore.Catalogo.Domain.Events;
using MyDDDStore.Core.Communication.Mediator;
using MyDDDStore.Core.Messages.CommomMessages.IntegrationEvents;
using MyDDDStore.Core.Messages.CommomMessages.Notifications;
using MyDDDStore.Pagamentos.AntiCorruption;
using MyDDDStore.Pagamentos.Business;
using MyDDDStore.Pagamentos.Business.Events;
using MyDDDStore.Pagamentos.Data;
using MyDDDStore.Pagamentos.Data.Repository;
using MyDDDStore.Vendas.Application.Commands;
using MyDDDStore.Vendas.Application.Events;
using MyDDDStore.Vendas.Application.Queries;
using MyDDDStore.Vendas.Data;
using MyDDDStore.Vendas.Data.Repository;
using MyDDDStore.Vendas.Domain;

namespace MyDDDStore.WebApp.MVC.Setup
{
    public static class DependencyInjection
    {        
        public static void RegisterServices(this IServiceCollection services)
        {
            // Mediator
            services.AddScoped<IMediatrHandler, MediatrHandler>();

            // Notifications
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

            // Event Sourcing
            /*services.AddSingleton<IEventStoreService, EventStoreService>();
            services.AddSingleton<IEventSourcingRepository, EventSourcingRepository>();*/

            // Catalogo
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<IProdutoAppService, ProdutoAppService>();
            services.AddScoped<IEstoqueService, EstoqueService>();
            services.AddScoped<CatalogoContext>();

            services.AddScoped<INotificationHandler<ProdutoAbaixoEstoqueEvent>, ProdutoEventHandler>();
            services.AddScoped<INotificationHandler<PedidoIniciadoEvent>, ProdutoEventHandler>();
            services.AddScoped<INotificationHandler<PedidoProcessamentoCanceladoEvent>, ProdutoEventHandler>();

            // Vendas
            services.AddScoped<IPedidoRepository, PedidoRepository>();
            services.AddScoped<IPedidoQueries, PedidoQueries>();
            services.AddScoped<VendasContext>();

            services.AddScoped<IRequestHandler<AdicionarItemPedidoCommand, bool>, PedidoCommandHandler>();
            services.AddScoped<IRequestHandler<AtualizarItemPedidoCommand, bool>, PedidoCommandHandler>();
            services.AddScoped<IRequestHandler<RemoverItemPedidoCommand, bool>, PedidoCommandHandler>();
            services.AddScoped<IRequestHandler<AplicarVoucherPedidoCommand, bool>, PedidoCommandHandler>();
            services.AddScoped<IRequestHandler<IniciarPedidoCommand, bool>, PedidoCommandHandler>();
            services.AddScoped<IRequestHandler<FinalizarPedidoCommand, bool>, PedidoCommandHandler>();
            services.AddScoped<IRequestHandler<CancelarProcessamentoPedidoCommand, bool>, PedidoCommandHandler>();
            services.AddScoped<IRequestHandler<CancelarProcessamentoPedidoEstornarEstoqueCommand, bool>, PedidoCommandHandler>();

            services.AddScoped<INotificationHandler<PedidoRascunhoIniciadoEvent>, PedidoEventHandler>();
            services.AddScoped<INotificationHandler<PedidoEstoqueRejeitadoEvent>, PedidoEventHandler>();
            services.AddScoped<INotificationHandler<PedidoPagamentoRealizadoEvent>, PedidoEventHandler>();
            services.AddScoped<INotificationHandler<PedidoPagamentoRecusadoEvent>, PedidoEventHandler>();

            // Pagamento
            services.AddScoped<IPagamentoRepository, PagamentoRepository>();
            services.AddScoped<IPagamentoService, PagamentoService>();
            services.AddScoped<IPagamentoCartaoCreditoFacade, PagamentoCartaoCreditoFacade>();
            services.AddScoped<IPayPalGateway, PayPalGateway>();
            services.AddScoped<IConfigurationManager, ConfigurationManager>();
            services.AddScoped<PagamentoContext>();

            services.AddScoped<INotificationHandler<PedidoEstoqueConfirmadoEvent>, PagamentoEventHandler>();
        }
    }
}
