using System;
using System.Threading.Tasks;
using DevIO.Business.Models;

namespace DevIO.Business.Interfaces
{
	public interface IFornecedorRepository : IRepository<Fornecedor>
	{
		Task<Fornecedor> GetFornecedorAddressAsync(Guid id);

		Task<Fornecedor> GetFornecedorProductsAndAddress(Guid id);
	}
}

