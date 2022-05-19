using System;
using System.Threading.Tasks;
using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using DevIO.Business.Models.Validations;

namespace DevIO.Business.Services
{
    public class ProdutoService : BaseService, IProdutoService
    {
        public async Task AddAsync(Produto produto)
        {
            if (!RunValidation(new ProdutoValidation(), produto))
                return;
        }

        public async Task UpdateAsync(Produto produto)
        {
            if (!RunValidation(new ProdutoValidation(), produto))
                return;
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}

