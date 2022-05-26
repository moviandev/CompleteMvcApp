using System;
using System.Linq;
using System.Threading.Tasks;
using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using DevIO.Business.Models.Validations;

namespace DevIO.Business.Services
{
    public class FornecedorService : BaseService, IFornecedorService
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IEnderecoRepository _enderecoRepository;

        public FornecedorService(IFornecedorRepository fornecedorRepository, IEnderecoRepository enderecoRepository, INotifier notifier) : base(notifier)
        {
            _fornecedorRepository = fornecedorRepository;
            _enderecoRepository = enderecoRepository;
        }

        public async Task AddAsync(Fornecedor fornecedor)
        {
            // Validate entity state
            if (!RunValidation(new FornecedorValidation(), fornecedor)
                || !RunValidation(new EnderecoValidation(), fornecedor.Endereco))
                return;

            if (_fornecedorRepository.SearchAsync(f => f.Documento == fornecedor.Documento).Result.Any())
            {
                Notificar("Já existe um fornecedor com esse documento");
                return;
            }

            await _fornecedorRepository.AddAsync(fornecedor);
        }

        public async Task UpdateAsync(Fornecedor fornecedor)
        {
            if (!RunValidation(new FornecedorValidation(), fornecedor))
                return;

            if (_fornecedorRepository.SearchAsync(f => f.Documento == fornecedor.Documento && f.Id != fornecedor.Id).Result.Any())
            {
                Notificar("Já existe um forncedor com esse documento");
                return;
            }

            await _fornecedorRepository.UpdateAsync(fornecedor);
        }

        public async Task UpdateEnderecosync(Endereco endereco)
        {
            if (!RunValidation(new EnderecoValidation(), endereco))
                return;

            await _enderecoRepository.UpdateAsync(endereco);
        }

        public async Task DeleteAsync(Guid id)
        {
            if(_fornecedorRepository.GetFornecedorProductsAndAddress(id).Result.Produtos.Any())
            {
                Notificar("O Fornecedor possui produtos cadastrados");
                return;
            }

            await _fornecedorRepository.DeleteAsync(id);
        }

        public void Dispose()
        {
            _fornecedorRepository?.Dispose();
            _enderecoRepository?.Dispose();
        }
    }
}

