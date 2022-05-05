using System;
using System.Threading.Tasks;
using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using DevIO.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace DevIO.Data.Repositories
{
    public class EnderecoRepository : Repository<Endereco>, IEnderecoRepository
    {
        public EnderecoRepository(MyDbContext db) : base(db)
        {
        }

        public async Task<Endereco> GetEnderecoByFornecedorIdAsync(Guid id)
        {
            return await Db.Enderecos
                .AsNoTracking()
                .Include(e => e.Fornecedor)
                .FirstOrDefaultAsync(e => id == e.Id);
        }
    }
}

