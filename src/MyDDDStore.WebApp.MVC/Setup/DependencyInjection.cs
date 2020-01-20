using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MyDDDStore.Catalogo.Application.Services;
using MyDDDStore.Catalogo.Data;
using MyDDDStore.Catalogo.Data.Repository;
using MyDDDStore.Catalogo.Domain;
using MyDDDStore.Catalogo.Domain.Events;
using MyDDDStore.Core.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyDDDStore.WebApp.MVC.Setup
{
    public static class DependencyInjection
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            // Domain Bus (Mediator)
            services.AddScoped<IMediatrHandler, MediatrHandler>();

            // Catálogo
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<IProdutoAppService, ProdutoAppService>();
            services.AddScoped<IEstoqueService, EstoqueService>();
            services.AddScoped<CatalogoContext>();

            // Events
            services.AddScoped<INotificationHandler<ProdutoAbaixoEstoqueEvent>, ProdutoEventHandler>();
        }
    }
}
