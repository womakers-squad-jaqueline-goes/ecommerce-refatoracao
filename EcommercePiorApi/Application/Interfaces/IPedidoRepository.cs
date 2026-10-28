// Application/Interfaces/IPedidoRepository.cs
using EcommercePiorApi.Domain.Entities;

namespace EcommercePiorApi.Application.Interfaces;

public interface IPedidoRepository
{
    Task<List<Pedido>> ListarAsync();

    Task<Pedido?> BuscarComItensAsync(int id);

    void Adicionar(Pedido pedido);

    Task SalvarAsync();

    Task RecarregarAsync(Pedido pedido);
}