using DevIO.App.Extensions;
using DevIO.Business.Interfaces;
using DevIO.Data.Contexts;
using DevIO.Data.Repositories;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;

namespace DevIO.App.Configurations
{
	public static class DependencyInjectionConfiguration
	{
		public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
			services.AddScoped<MyDbContext>();
			services.AddScoped<IProdutoRepository, ProdutoRepository>();
			services.AddScoped<IFornecedorRepository, FornecedorRepository>();
			services.AddScoped<IEnderecoRepository, EnderecoRepository>();
			services.AddSingleton<IValidationAttributeAdapterProvider, CurrencyValidationAttributeAdapterProvider>();

			return services;
		}
	}
}

