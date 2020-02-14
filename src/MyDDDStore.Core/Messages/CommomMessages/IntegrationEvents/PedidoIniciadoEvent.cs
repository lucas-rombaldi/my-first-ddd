using MyDDDStore.Core.DomainObjects.DTO;
using System;

namespace MyDDDStore.Core.Messages.CommomMessages.IntegrationEvents
{
    public class PedidoIniciadoEvent : IntegrationEvent
    {
        public Guid ClienteId { get; private set; }
        public Guid PedidoId { get; private set; }
        public decimal Total { get; private set; }
        public ListaProdutosPedido ProdutosPedido { get; private set; }
        public string NomeCartao { get; private set; }
        public string NumeroCartao { get; private set; }
        public string ExpiracaoCartao { get; private set; }
        public string CvvCartao { get; private set; }

        public PedidoIniciadoEvent(Guid clienteId, Guid pedidoId, decimal total, ListaProdutosPedido itens, string nomeCartao, string numeroCartao, string expiracaoCartao, string cvvCartao)
        {
            AggregateId = pedidoId;
            ClienteId = clienteId;
            PedidoId = pedidoId;
            ProdutosPedido = itens;
            Total = total;
            NomeCartao = nomeCartao;
            NumeroCartao = numeroCartao;
            ExpiracaoCartao = expiracaoCartao;
            CvvCartao = cvvCartao;
        }
    }
}
