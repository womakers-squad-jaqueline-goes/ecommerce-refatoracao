using Microsoft.EntityFrameworkCore;

namespace PiorEcommerceDoMundo;

public class ProdutoRepository : IProdutoRepository
{
    private readonly AppDbContext _context;

    public ProdutoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Produto>> ListarAsync()
    {
        return await _context.Produtos.ToListAsync();
    }
    public async Task<Produto?> BuscarPorIdAsync(int id)
    {
        return await _context.Produtos.FirstOrDefaultAsync(p => p.Id == id);
    }
   public Task AdicionarAsync(Produto produto)
    {
        _context.Produtos.Add(produto);
        return _context.SaveChangesAsync();
    }
   public  Task AtualizarAsync(Produto produto)
    {
        _context.Produtos.Update(produto);
        return _context.SaveChangesAsync();
    }
   public async Task RemoverAsync(Produto produto)
    {
        _context.Produtos.Remove(produto);
        await _context.SaveChangesAsync();
    }
}