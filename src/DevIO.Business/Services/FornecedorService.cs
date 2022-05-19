using System;
using System.Threading.Tasks;
using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using DevIO.Business.Models.Validations;

namespace DevIO.Business.Services
{
    public class FornecedorService : BaseService, IFornecedorService
    {
        public async Task AddAsync(Fornecedor fornecedor)
        {
            // Validate entity state
            if (!RunValidation(new FornecedorValidation(), fornecedor)
                && !RunValidation(new EnderecoValidation(), fornecedor.Endereco))
                return;
        }

        public async Task UpdateAsync(Fornecedor fornecedor)
        {
            if (!RunValidation(new FornecedorValidation(), fornecedor))
                return;
        }

        public async Task UpdateEnderecosync(Fornecedor fornecedor)
        {
            if (!RunValidation(new EnderecoValidation(), fornecedor.Endereco))
                return;
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}

