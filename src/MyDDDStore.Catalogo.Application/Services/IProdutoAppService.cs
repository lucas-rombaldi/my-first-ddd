using MyDDDStore.Catalogo.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyDDDStore.Catalogo.Application.Services
{
    public interface IProdutoAppService
    {
        Task<IEnumerable<ProdutoViewModel>> FindByCategory(int codigo);
        Task<ProdutoViewModel> FindByID(Guid id);
        Task<IEnumerable<ProdutoViewModel>> FindAll();
        Task<IEnumerable<CategoriaViewModel>> FindCategories();

        Task AdicionarProduto(ProdutoViewModel produtoViewModel);
        Task AtualizarProduto(ProdutoViewModel produtoViewModel);

        Task<ProdutoViewModel> DebitarEstoque(Guid id, int quantidade);
        Task<ProdutoViewModel> ReporEstoque(Guid id, int quantidade);
    }
}
