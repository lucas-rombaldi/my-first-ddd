using MyDDDStore.Core.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyDDDStore.Catalogo.Domain
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<IEnumerable<Produto>> FindAll();

        Task<Produto> FindByID(Guid id);

        Task<IEnumerable<Produto>> FindByCategory(int codigo);

        Task<IEnumerable<Categoria>> FindCategories();

        void Insert(Produto produto);
        void Update(Produto produto);

        void Insert(Categoria categoria);
        void Update(Categoria categoria);
    }
}
