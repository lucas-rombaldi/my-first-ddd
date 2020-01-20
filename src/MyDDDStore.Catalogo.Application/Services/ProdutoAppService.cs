using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MyDDDStore.Catalogo.Application.ViewModels;
using MyDDDStore.Catalogo.Domain;
using MyDDDStore.Core.DomainObjects;

namespace MyDDDStore.Catalogo.Application.Services
{
    public class ProdutoAppService : IProdutoAppService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IEstoqueService _estoqueService;
        private readonly IMapper _mapper;

        public ProdutoAppService(IProdutoRepository produtoRepository,
                                 IMapper mapper,
                                 IEstoqueService estoqueService)
        {
            _produtoRepository = produtoRepository;
            _mapper = mapper;
            _estoqueService = estoqueService;
        }

        public async Task<IEnumerable<ProdutoViewModel>> FindByCategory(int codigo)
        {
            return _mapper.Map<IEnumerable<ProdutoViewModel>>(await _produtoRepository.FindByCategory(codigo));
        }

        public async Task<ProdutoViewModel> FindByID(Guid id)
        {
            return _mapper.Map<ProdutoViewModel>(await _produtoRepository.FindByID(id));
        }

        public async Task<IEnumerable<ProdutoViewModel>> FindAll()
        {
            return _mapper.Map<IEnumerable<ProdutoViewModel>>(await _produtoRepository.FindAll());
        }

        public async Task<IEnumerable<CategoriaViewModel>> FindCategories()
        {
            return _mapper.Map<IEnumerable<CategoriaViewModel>>(await _produtoRepository.FindCategories());
        }

        public async Task AdicionarProduto(ProdutoViewModel produtoViewModel)
        {
            var produto = _mapper.Map<Produto>(produtoViewModel);
            _produtoRepository.Insert(produto);

            await _produtoRepository.UnitOfWork.Commit();
        }

        public async Task AtualizarProduto(ProdutoViewModel produtoViewModel)
        {
            var produto = _mapper.Map<Produto>(produtoViewModel);
            _produtoRepository.Update(produto);

            await _produtoRepository.UnitOfWork.Commit();
        }

        public async Task<ProdutoViewModel> DebitarEstoque(Guid id, int quantidade)
        {
            if (!_estoqueService.DebitarEstoque(id, quantidade).Result)
            {
                throw new DomainException("Falha ao debitar estoque");
            }

            return _mapper.Map<ProdutoViewModel>(await _produtoRepository.FindByID(id));
        }

        public async Task<ProdutoViewModel> ReporEstoque(Guid id, int quantidade)
        {
            if (!_estoqueService.ReporEstoque(id, quantidade).Result)
            {
                throw new DomainException("Falha ao repor estoque");
            }

            return _mapper.Map<ProdutoViewModel>(await _produtoRepository.FindByID(id));
        }

        public void Dispose()
        {
            _produtoRepository?.Dispose();
            _estoqueService?.Dispose();
        }
    }
}