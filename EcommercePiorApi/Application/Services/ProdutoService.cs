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
        var produto = await _produtoRepository.BuscarPorIdAsync(id);

        if (produto == null)
        {
            throw new NotFoundException("Produto não encontrado.");
        }

        return MapearParaResponse(produto);
    }

    public async Task<ProdutoResponseDto> CriarAsync(CriarProdutoDto criarProdutoDto)
    {
        var produto = new Produto
        {
            Nome = criarProdutoDto.Nome,
            Descricao = criarProdutoDto.Descricao,
            Preco = criarProdutoDto.Preco,
            QuantidadeEstoque = criarProdutoDto.QuantidadeEstoque,
            Ativo = criarProdutoDto.Ativo
        };

        await _produtoRepository.AdicionarAsync(produto);

        return MapearParaResponse(produto);
    }

    public async Task<ProdutoResponseDto> AtualizarAsync(int id, AtualizarProdutoDto atualizarProdutoDto)
    {
        var produto = await _produtoRepository.BuscarPorIdAsync(id);

        if (produto == null)
        {
            throw new NotFoundException("Produto não encontrado.");
        }

        produto.Nome = atualizarProdutoDto.Nome;
        produto.Descricao = atualizarProdutoDto.Descricao;
        produto.Preco = atualizarProdutoDto.Preco;
        produto.QuantidadeEstoque = atualizarProdutoDto.QuantidadeEstoque;
        produto.Ativo = atualizarProdutoDto.Ativo;

        await _produtoRepository.AtualizarAsync(produto);

        return MapearParaResponse(produto);
    }

    public async Task RemoverAsync(int id)
    {
        var produto = await _produtoRepository.BuscarPorIdAsync(id);

        if (produto == null)
        {
            throw new NotFoundException("Produto não encontrado.");
        }

        await _produtoRepository.RemoverAsync(produto);
    }
}