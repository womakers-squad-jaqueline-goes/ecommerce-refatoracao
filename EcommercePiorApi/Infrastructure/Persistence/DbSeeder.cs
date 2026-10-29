using EcommercePiorApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcommercePiorApi.Infrastructure.Persistence;
public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await db.Database.EnsureCreatedAsync();

        if (await db.Produtos.AnyAsync())
            return;

        db.Produtos.AddRange(
            Produto.Criar("Camiseta", "Camiseta básica", 49.90m, 100),
            Produto.Criar("Tênis", "Tênis esportivo", 299.90m, 50),
            Produto.Criar("Boné", "Boné ajustável", 39.90m, 200),
            Produto.Criar("Mochila", "Mochila para notebook", 159.90m, 30));

        await db.SaveChangesAsync();
    }
}