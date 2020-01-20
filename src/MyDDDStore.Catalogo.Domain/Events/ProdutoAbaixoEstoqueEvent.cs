using MyDDDStore.Core.DomainObjects;
using System;

namespace MyDDDStore.Catalogo.Domain.Events
{
    public class ProdutoAbaixoEstoqueEvent : DomainEvent
    {
        public decimal QuantidadeRestante { get; private set; }

        public ProdutoAbaixoEstoqueEvent(Guid aggregateId, decimal quantidadeRestante) 
            : base(aggregateId)
        {
            QuantidadeRestante = quantidadeRestante;
        }
    }
}
