using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevIO.Business.Models;

namespace DevIO.Business.Interfaces
{
	public interface IProdutoRepository : IRepository<Produto>
	{
		Task<IEnumerable<Produto>> GetProdutosByFornecedorAsync(Guid fornecedorId);

		Task<IEnumerable<Produto>> GetProdutosFornecedoresAsync();

		Task<Produto> GetProdutoForncedor(Guid id);
	}
}

