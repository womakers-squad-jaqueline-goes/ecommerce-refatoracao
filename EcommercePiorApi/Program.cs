using EcommercePiorApi.Domain.Entities;
using EcommercePiorApi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using PiorEcommerceDoMundo;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    // Mantém o comportamento atual da serialização enquanto as entidades são expostas pela API.
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<IProdutoService, ProdutoService>();
builder.Services.AddScoped<IEstoqueService, EstoqueService>();

var connectionString = builder.Configuration.GetConnectionString("Ecommerce")
    ?? throw new InvalidOperationException(
        "A connection string 'Ecommerce' não foi configurada.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

var app = builder.Build();

// Inicializa o banco e mantém os produtos de exemplo na primeira execução.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();

    if (!db.Produtos.Any())
    {
        db.Produtos.AddRange(
            new Produto { Nome = "Camiseta", Descricao = "Camiseta basica", Preco = 49.90m, QuantidadeEstoque = 100, Ativo = true },
            new Produto { Nome = "Tenis", Descricao = "Tenis esportivo", Preco = 299.90m, QuantidadeEstoque = 50, Ativo = true },
            new Produto { Nome = "Bone", Descricao = "Bone ajustavel", Preco = 39.90m, QuantidadeEstoque = 200, Ativo = true },
            new Produto { Nome = "Mochila", Descricao = "Mochila para notebook", Preco = 159.90m, QuantidadeEstoque = 30, Ativo = true }
        );
        db.SaveChanges();
    }
}

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
