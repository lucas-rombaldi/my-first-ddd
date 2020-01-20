using Microsoft.EntityFrameworkCore;
using MyDDDStore.Catalogo.Domain;
using MyDDDStore.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyDDDStore.Catalogo.Data.Repository
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly CatalogoContext _context;

        public ProdutoRepository(CatalogoContext context)
        {
            _context = context;
        }
        public IUnitOfWork UnitOfWork => _context;

        public async Task<IEnumerable<Produto>> FindAll()
        {
            return await _context.Produtos.AsNoTracking().ToListAsync();
        }

        public async Task<Produto> FindByID(Guid id)
        {
            //return await _context.Produtos.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            return await _context.Produtos.FindAsync(id);
        }

        public async Task<IEnumerable<Produto>> FindByCategory(int codigo)
        {
            return await _context.Produtos.AsNoTracking().Include(p => p.Categoria).Where(c => c.Categoria.Codigo == codigo).ToListAsync();
        }

        public async Task<IEnumerable<Categoria>> FindCategories()
        {
            return await _context.Categorias.AsNoTracking().ToListAsync();
        }

        public void Insert(Produto produto)
        {
            _context.Produtos.Add(produto);
        }

        public void Update(Produto produto)
        {
            _context.Produtos.Update(produto);
        }

        public void Insert(Categoria categoria)
        {
            _context.Categorias.Add(categoria);
        }

        public void Update(Categoria categoria)
        {
            _context.Categorias.Update(categoria);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
