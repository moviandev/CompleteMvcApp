using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using Microsoft.EntityFrameworkCore;

namespace DevIO.Data.Repositories
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRpository
    {
        public async Task<Produto> GetProdutoForncedor(Guid id)
        {
            return await Db.Produtos
                .AsNoTracking()
                .Include(f => f.Fornecedor)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Produto>> GetProdutosByFornecedorAsync(Guid fornecedorId)
        {
            return await SearchAsync(p => p.FornecedorId == fornecedorId);
        }

        public async Task<IEnumerable<Produto>> GetProdutosFornecedoresAsync()
        {
            return await Db.Produtos
                .AsNoTracking()
                .Include(f => f.Fornecedor)
                .OrderBy(p => p.Nome)
                .ToListAsync();
        }
    }
}

