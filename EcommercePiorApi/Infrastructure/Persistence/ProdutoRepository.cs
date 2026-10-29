
using EcommercePiorApi.Application.Interfaces;
using EcommercePiorApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcommercePiorApi.Infrastructure.Persistence;

public class ProdutoRepository : IProdutoRepository
{
    private readonly AppDbContext _context;

    public ProdutoRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<List<Produto>> ListarAsync()
        => _context.Produtos.AsNoTracking().OrderBy(p => p.Id).ToListAsync();

    public Task<Produto?> BuscarPorIdAsync(int id)
        => _context.Produtos.FirstOrDefaultAsync(p => p.Id == id);

    public void Adicionar(Produto produto) => _context.Produtos.Add(produto);

    public Task SalvarAsync() => _context.SalvarAsync();
}