// Program.cs
using System.Text.Json.Serialization;
using EcommercePiorApi.Application.Descontos;
using EcommercePiorApi.Application.Interfaces;
using EcommercePiorApi.Application.Services;
using EcommercePiorApi.Infrastructure.Integrations;
using EcommercePiorApi.Infrastructure.Persistence;
using EcommercePiorApi.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())); // "Pix" em vez de 2

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var connectionString = builder.Configuration.GetConnectionString("Ecommerce")
    ?? throw new InvalidOperationException("A connection string 'Ecommerce' não foi configurada.");

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));

// repositórios
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();

// serviços de aplicação
builder.Services.AddScoped<IProdutoService, ProdutoService>();
builder.Services.AddScoped<IEstoqueService, EstoqueService>();
builder.Services.AddScoped<IPedidoService, PedidoService>();

// regras de desconto: a ORDEM de registro define a prioridade (pedido grande antes de VIP, como no original)
builder.Services.AddScoped<IRegraDesconto, DescontoPedidoGrande>();
builder.Services.AddScoped<IRegraDesconto, DescontoClienteVip>();

// integrações externas (mocks): para usar um gateway real, troque só estas duas linhas
builder.Services.Configure<PagamentoMockOptions>(builder.Configuration.GetSection(PagamentoMockOptions.Secao));
builder.Services.AddScoped<IPagamentoService, PagamentoMock>();
builder.Services.AddScoped<ILogisticaService, LogisticaMock>();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    await DbSeeder.SeedAsync(app.Services);
}

app.MapControllers();

app.Run();