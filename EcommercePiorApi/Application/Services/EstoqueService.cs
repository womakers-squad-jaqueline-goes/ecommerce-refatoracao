using EcommercePiorApi.Application.DTOs.Responses;
using EcommercePiorApi.Application.Interfaces;
using EcommercePiorApi.Application.Mappings;
using EcommercePiorApi.Domain.Entities;
using EcommercePiorApi.Domain.Exceptions;

namespace EcommercePiorApi.Application.Services;

public class EstoqueService : IEstoqueService
{
    private readonly IProdutoRepository _produtos;
    private readonly ILogger<EstoqueService> _logger;

    public EstoqueService(IProdutoRepository produtos, ILogger<EstoqueService> logger)
    {
        _produtos = produtos;
        _logger = logger;
    }

    public async Task<List<EstoqueResponseDto>> ListarAsync()
        => (await _produtos.ListarAsync()).Select(p => p.ToEstoqueResponse()).ToList();

    public async Task<EstoqueResponseDto> ReabastecerAsync(int produtoId, int quantidade)
    {
        var produto = await ObterOuFalharAsync(produtoId);

        produto.ReabastecerEstoque(quantidade);
        await _produtos.SalvarAsync();

        _logger.LogInformation("Estoque do produto {ProdutoId} reabastecido em {Quantidade}", produtoId, quantidade);
        return produto.ToEstoqueResponse();
    }

    public async Task<EstoqueResponseDto> BaixarAsync(int produtoId, int quantidade)
    {
        var produto = await ObterOuFalharAsync(produtoId);

        produto.DiminuirEstoque(quantidade);
        await _produtos.SalvarAsync();

        _logger.LogInformation("Baixa de {Quantidade} no estoque do produto {ProdutoId}", quantidade, produtoId);
        return produto.ToEstoqueResponse();
    }

    private async Task<Produto> ObterOuFalharAsync(int id)
        => await _produtos.BuscarPorIdAsync(id)
           ?? throw new NotFoundException($"Produto {id} não encontrado.");
}