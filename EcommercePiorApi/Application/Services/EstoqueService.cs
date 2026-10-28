namespace PiorEcommerceDoMundo;

public class EstoqueService : IEstoqueService
{
    private readonly IProdutoRepository _produtoRepository;

    public EstoqueService(IProdutoRepository produtoRepository)
    {
        _produtoRepository = produtoRepository;
    }

    public async Task<List<EstoqueResponseDto>> ListarAsync()
    {
        var produtos = await _produtoRepository.ListarAsync();

        return produtos.Select(produto => new EstoqueResponseDto
        {
            ProdutoId = produto.Id,
            Nome = produto.Nome,
            QuantidadeEstoque = produto.QuantidadeEstoque
        }).ToList();
    }

    public async Task BaixarAsync(int produtoId, MovimentarEstoqueDto dto)
    {
        var produto = await ObterProdutoAsync(produtoId);

        produto.DiminuirEstoque(dto.Quantidade);

        await _produtoRepository.AtualizarAsync(produto);
    }

    public async Task ReabastecerAsync(int produtoId, MovimentarEstoqueDto dto)
    {
        var produto = await ObterProdutoAsync(produtoId);

        produto.ReabastecerEstoque(dto.Quantidade);

        await _produtoRepository.AtualizarAsync(produto);
    }

    private async Task<Produto> ObterProdutoAsync(int produtoId)
    {
        var produto = await _produtoRepository.BuscarPorIdAsync(produtoId);

        if (produto == null)
            throw new NotFoundException("Produto não encontrado.");

        return produto;
    }


}