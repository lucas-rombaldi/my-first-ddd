using MyDDDStore.Catalogo.Domain.Events;
using MyDDDStore.Core.Communication.Mediator;
using MyDDDStore.Core.DomainObjects.DTO;
using System;
using System.Threading.Tasks;

namespace MyDDDStore.Catalogo.Domain
{
    public class EstoqueService : IDisposable, IEstoqueService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IMediatrHandler _mediatrHandler;

        public EstoqueService(IProdutoRepository produtoRepository, IMediatrHandler mediatrHandler)
        {
            _produtoRepository = produtoRepository;
            _mediatrHandler = mediatrHandler;
        }

        public async Task<bool> DebitarEstoque(Guid produtoId, int quantidade)
        {
            if (!await DebitarItemEstoque(produtoId, quantidade)) return false;
            return await _produtoRepository.UnitOfWork.Commit();
        }

        public async Task<bool> DebitarListaProdutosPedido(ListaProdutosPedido lista)
        {
            foreach (var item in lista.Itens)
            {
                if (!await DebitarItemEstoque(item.Id, item.Quantidade)) return false;
            }

            return await _produtoRepository.UnitOfWork.Commit();
        }

        private async Task<bool> DebitarItemEstoque(Guid produtoId, int quantidade)
        {
            var produto = await _produtoRepository.FindByID(produtoId);

            if (produto == null) return false;

            if (!produto.PossuiEstoque(quantidade)) return false;

            produto.DebitarEstoque(quantidade);

            if (produto.QuantidadeEstoque < 10)
            {
                await _mediatrHandler.PublishEvent(new ProdutoAbaixoEstoqueEvent(produto.Id, produto.QuantidadeEstoque));
            }

            _produtoRepository.Update(produto);
            return await _produtoRepository.UnitOfWork.Commit();
        }

        public async Task<bool> ReporListaProdutosPedido(ListaProdutosPedido lista)
        {
            foreach (var item in lista.Itens)
            {
                await ReporItemEstoque(item.Id, item.Quantidade);
            }

            return await _produtoRepository.UnitOfWork.Commit();
        }

        public async Task<bool> ReporEstoque(Guid produtoId, int quantidade)
        {
            var sucesso = await ReporItemEstoque(produtoId, quantidade);

            if (!sucesso) return false;

            return await _produtoRepository.UnitOfWork.Commit();
        }

        private async Task<bool> ReporItemEstoque(Guid produtoId, int quantidade)
        {
            var produto = await _produtoRepository.FindByID(produtoId);

            if (produto == null) return false;
            produto.ReporEstoque(quantidade);

            _produtoRepository.Update(produto);

            return true;
        }

        public void Dispose()
        {
            _produtoRepository.Dispose();
        }
    }
}
