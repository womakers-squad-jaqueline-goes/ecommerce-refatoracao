// Application/Interfaces/IProdutoRepository.cs
using EcommercePiorApi.Domain.Entities;

namespace EcommercePiorApi.Application.Interfaces;

public interface IProdutoRepository
{
    Task<List<Produto>> ListarAsync();
    Task<Produto?> BuscarPorIdAsync(int id);
    void Adicionar(Produto produto);
    Task SalvarAsync();
}