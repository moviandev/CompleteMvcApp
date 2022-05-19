using System;
using System.Threading.Tasks;
using DevIO.Business.Models;

namespace DevIO.Business.Interfaces
{
	public interface IProdutoService
	{
		Task AddAsync(Produto produto);

		Task UpdateAsync(Produto produto);

		Task DeleteAsync(Guid id);
	}
}

