using System;
using System.Threading.Tasks;
using DevIO.Business.Models;

namespace DevIO.Business.Interfaces
{
	public interface IFornecedorService : IDisposable
	{
		Task AddAsync(Fornecedor fornecedor);

		Task UpdateAsync(Fornecedor fornecedor);

		Task DeleteAsync(Guid id);

		Task UpdateEnderecosync(Endereco endereco);
	}
}

