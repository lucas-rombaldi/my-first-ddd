using System;
using FluentValidation;
using MyDDDStore.Core.Messages;

namespace MyDDDStore.Vendas.Application.Commands
{
    public class FinalizarPedidoCommand : Command
    {
        public Guid ClienteId { get; private set; }
        public Guid PedidoId { get; private set; }

        public FinalizarPedidoCommand(Guid clienteId, Guid pedidoId)
        {
            AggregateId = pedidoId;
            ClienteId = clienteId;
            PedidoId = pedidoId;
        }

        public override bool IsValid()
        {
            ValidationResult = new FinalizarPedidoValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class FinalizarPedidoValidation : AbstractValidator<FinalizarPedidoCommand>
    {
        public FinalizarPedidoValidation()
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