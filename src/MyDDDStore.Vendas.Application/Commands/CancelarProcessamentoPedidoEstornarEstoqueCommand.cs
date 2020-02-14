using System;
using FluentValidation;
using MyDDDStore.Core.Messages;

namespace MyDDDStore.Vendas.Application.Commands
{
    public class CancelarProcessamentoPedidoEstornarEstoqueCommand : Command
    {
        public Guid ClienteId { get; private set; }
        public Guid PedidoId { get; private set; }

        public CancelarProcessamentoPedidoEstornarEstoqueCommand(Guid clienteId, Guid pedidoId)
        {
            AggregateId = pedidoId;
            ClienteId = clienteId;
            PedidoId = pedidoId;
        }

        public override bool IsValid()
        {
            ValidationResult = new CancelarProcessamentoPedidoEstornarEstoqueValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class CancelarProcessamentoPedidoEstornarEstoqueValidation : AbstractValidator<CancelarProcessamentoPedidoEstornarEstoqueCommand>
    {
        public CancelarProcessamentoPedidoEstornarEstoqueValidation()
        {
            RuleFor(c => c.ClienteId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id do cliente inválido");

            RuleFor(c => c.PedidoId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id do pedido inválido");
        }
    }


}