using System;
using System.Threading.Tasks;
using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using DevIO.Business.Models.Validations;

namespace DevIO.Business.Services
{
    public class ProdutoService : BaseService, IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;

        public ProdutoService(IProdutoRepository produtoRepository, INotifier notifier) : base(notifier)
        {
            _produtoRepository = produtoRepository;
        }

        public async Task AddAsync(Produto produto)
        {
            if (!RunValidation(new ProdutoValidation(), produto))
                return;

            await _produtoRepository.AddAsync(produto);
        }

        public async Task UpdateAsync(Produto produto)
        {
            if (!RunValidation(new ProdutoValidation(), produto))
                return;

            await _produtoRepository.UpdateAsync(produto);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _produtoRepository.DeleteAsync(id);
        }

        public void Dispose()
        {
            _produtoRepository?.Dispose();
        }
    }
}

