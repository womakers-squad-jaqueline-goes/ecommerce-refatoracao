using EcommercePiorApi.Application.DTOs.Requests;
using EcommercePiorApi.Application.DTOs.Responses;
using EcommercePiorApi.Application.Interfaces;
using EcommercePiorApi.Application.Mappings;
using EcommercePiorApi.Domain.Entities;
using EcommercePiorApi.Domain.Exceptions;

namespace EcommercePiorApi.Application.Services;

public class ProdutoService : IProdutoService
{
    private readonly IProdutoRepository _produtos;
    private readonly ILogger<ProdutoService> _logger;

    public ProdutoService(IProdutoRepository produtos, ILogger<ProdutoService> logger)
    {
        _produtos = produtos;
        _logger = logger;
    }

    public async Task<List<ProdutoResponseDto>> ListarAsync()
        => (await _produtos.ListarAsync()).Select(p => p.ToResponse()).ToList();

    public async Task<ProdutoResponseDto> BuscarPorIdAsync(int id)
        => (await ObterOuFalharAsync(id)).ToResponse();

    public async Task<ProdutoResponseDto> CriarAsync(CriarProdutoDto dto)
    {
        var produto = Produto.Criar(dto.Nome, dto.Descricao, dto.Preco, dto.QuantidadeEstoque);

        _produtos.Adicionar(produto);
        await _produtos.SalvarAsync();

        _logger.LogInformation("Produto {ProdutoId} criado: {Nome}", produto.Id, produto.Nome);
        return produto.ToResponse();
    }

    public async Task<ProdutoResponseDto> AtualizarAsync(int id, AtualizarProdutoDto dto)
    {
        var produto = await ObterOuFalharAsync(id);

        produto.Atualizar(dto.Nome, dto.Descricao, dto.Preco);

        if (dto.Ativo)
            produto.Ativar();
        else
            produto.Desativar();

        await _produtos.SalvarAsync();

        _logger.LogInformation("Produto {ProdutoId} atualizado", id);
        return produto.ToResponse();
    }

    public async Task DesativarAsync(int id)
    {
        var produto = await ObterOuFalharAsync(id);

        produto.Desativar();
        await _produtos.SalvarAsync();

        _logger.LogInformation("Produto {ProdutoId} desativado", id);
    }

    private async Task<Produto> ObterOuFalharAsync(int id)
        => await _produtos.BuscarPorIdAsync(id)
           ?? throw new NotFoundException($"Produto não encontrado.");
}