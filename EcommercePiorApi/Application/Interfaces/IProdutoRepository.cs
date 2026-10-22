using EcommercePiorApi.Domain.Entities;

namespace PiorEcommerceDoMundo;

public interface IProdutoRepository
{
    Task<List<Produto>> ListarAsync();
    Task<Produto?> BuscarPorIdAsync(int id);
    Task AdicionarAsync(Produto produto);
    Task AtualizarAsync(Produto produto);
    Task RemoverAsync(Produto produto);
}