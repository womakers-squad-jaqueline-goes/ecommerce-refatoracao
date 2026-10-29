// Infrastructure/Persistence/AppDbContext.cs
using EcommercePiorApi.Domain.Entities;
using EcommercePiorApi.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace EcommercePiorApi.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Produto> Produtos => Set<Produto>();
    public DbSet<Pedido> Pedidos => Set<Pedido>();
    public DbSet<ItemPedido> ItensPedido => Set<ItemPedido>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Produto>(e =>
        {
            e.Property(p => p.Nome).IsRequired().HasMaxLength(120);
            e.Property(p => p.Descricao).HasMaxLength(500);
            e.Property(p => p.Preco).HasPrecision(18, 2);

            e.Property(p => p.QuantidadeEstoque).IsConcurrencyToken();
        });

        modelBuilder.Entity<Pedido>(e =>
        {
            e.Property(p => p.ClienteNome).IsRequired().HasMaxLength(120);
            e.Property(p => p.ClienteEmail).IsRequired().HasMaxLength(254);
            e.Property(p => p.Status).HasConversion<string>().HasMaxLength(20);          
            e.Property(p => p.FormaPagamento).HasConversion<string>().HasMaxLength(20);
            e.Property(p => p.Desconto).HasPrecision(18, 2);
            e.Property(p => p.CodigoRastreio).HasMaxLength(30);

           
            e.Property(p => p.Status).IsConcurrencyToken();

            e.HasMany(p => p.Itens).WithOne(i => i.Pedido).HasForeignKey(i => i.PedidoId);
            e.Navigation(p => p.Itens).UsePropertyAccessMode(PropertyAccessMode.Field); 
        });

        modelBuilder.Entity<ItemPedido>()
            .Property(i => i.PrecoUnitario).HasPrecision(18, 2);
    }

    public async Task SalvarAsync()
    {
        try
        {
            await SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new ConcorrenciaException(ex);
        }
    }
}