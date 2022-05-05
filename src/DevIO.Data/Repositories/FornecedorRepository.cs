using System;
using System.Threading.Tasks;
using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using DevIO.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace DevIO.Data.Repositories
{
    public class FornecedorRepository : Repository<Fornecedor>, IFornecedorRepository
    {
        public FornecedorRepository(MyDbContext db) : base(db)
        {
        }

        public async Task<Fornecedor> GetFornecedorAddressAsync(Guid id)
        {
            return await Db.Fornecedores
                .AsNoTracking()
                .Include(e => e.Endereco)
                .FirstOrDefaultAsync(f => id == f.Id);
        }

        public async Task<Fornecedor> GetFornecedorProductsAndAddress(Guid id)
        {
            return await Db.Fornecedores
                .AsNoTracking()
                .Include(p => p.Produtos)
                .Include(e => e.Endereco)
                .FirstOrDefaultAsync(f => id == f.Id);
        }
    }
}

