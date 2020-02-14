using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyDDDStore.Pagamentos.Business;

namespace MyDDDStore.Pagamentos.Data.Mappings
{
    public class TransacaoMapping : IEntityTypeConfiguration<Transacao>
    {
        public void Configure(EntityTypeBuilder<Transacao> builder)
        {
            builder.HasKey(c => c.Id);

            builder.ToTable("Transacoes");
        }
    }
}