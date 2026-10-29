using EcommercePiorApi.Application.Interfaces;
using EcommercePiorApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcommercePiorApi.Infrastructure.Persistence;

public class PedidoRepository : IPedidoRepository
{
    private readonly AppDbContext _context;

    public PedidoRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<List<Pedido>> ListarAsync()
        => _context.Pedidos
            .AsNoTracking()
            .Include(p => p.Itens).ThenInclude(i => i.Produto)
            .OrderBy(p => p.Id)
            .ToListAsync();

    public Task<Pedido?> BuscarComItensAsync(int id)
        => _context.Pedidos
            .Include(p => p.Itens).ThenInclude(i => i.Produto)
            .FirstOrDefaultAsync(p => p.Id == id);

    public void Adicionar(Pedido pedido) => _context.Pedidos.Add(pedido);

    public Task SalvarAsync() => _context.SalvarAsync();

    public async Task RecarregarAsync(Pedido pedido)
    {
        await _context.Entry(pedido).ReloadAsync();

        foreach (var item in pedido.Itens)
            await _context.Entry(item.Produto).ReloadAsync();
    }
}