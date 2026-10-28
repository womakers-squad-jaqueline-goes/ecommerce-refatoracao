using EcommercePiorApi.Domain.Entities;

namespace PiorEcommerceDoMundo;

public class ProdutoService : IProdutoService
{
    private readonly IProdutoRepository _produtoRepository;

    public ProdutoService(IProdutoRepository produtoRepository)
    {
        _produtoRepository = produtoRepository;
    }

    private static ProdutoResponseDto MapearParaResponse(Produto produto)
    {
        return new ProdutoResponseDto
        {
            Id = produto.Id,
            Nome = produto.Nome,
            Descricao = produto.Descricao,
            Preco = produto.Preco,
            QuantidadeEstoque = produto.QuantidadeEstoque,
            Ativo = produto.Ativo
        };
    }

    public async Task<List<ProdutoResponseDto>> ListarAsync()
    {
        var produtos = await _produtoRepository.ListarAsync();

        return produtos.Select(MapearParaResponse).ToList();
    }

    public async Task<ProdutoResponseDto> BuscarPorIdAsync(int id)
    {
        var produto = await ObterProdutoAsync(id);

        return MapearParaResponse(produto);
    }

    public async Task<ProdutoResponseDto> CriarAsync(CriarProdutoDto criarProdutoDto)
    {
        var produto = new Produto
        (
            criarProdutoDto.Nome,
            criarProdutoDto.Descricao,
            criarProdutoDto.Preco,
            criarProdutoDto.QuantidadeEstoque
        );

        if (!criarProdutoDto.Ativo)
            produto.Desativar();

        await _produtoRepository.AdicionarAsync(produto);

        return MapearParaResponse(produto);
    }

    public async Task<ProdutoResponseDto> AtualizarAsync(int id, AtualizarProdutoDto atualizarProdutoDto)
    {
        var produto = await ObterProdutoAsync(id);

        produto.Atualizar(atualizarProdutoDto.Nome, atualizarProdutoDto.Descricao, atualizarProdutoDto.Preco);

        if (atualizarProdutoDto.Ativo)
            produto.Ativar();
        else
            produto.Desativar();

        await _produtoRepository.AtualizarAsync(produto);

        return MapearParaResponse(produto);
    }

    public async Task RemoverAsync(int id)
    {
        var produto = await ObterProdutoAsync(id);

        await _produtoRepository.RemoverAsync(produto);
    }

    private async Task<Produto> ObterProdutoAsync(int id)
    {
        var produto = await _produtoRepository.BuscarPorIdAsync(id);

        if (produto is null)
            throw new NotFoundException("Produto não encontrado.");

        return produto;
    }
}