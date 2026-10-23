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
        var produto = await _produtoRepository.BuscarPorIdAsync(produtoId);

        if (produto == null)
            throw new NotFoundException("Produto não encontrado.");

        if (dto.Quantidade <= 0)
            throw new DomainException("A quantidade deve ser maior que zero.");

        if (dto.Quantidade > produto.QuantidadeEstoque)
            throw new DomainException("Estoque insuficiente.");

        produto.QuantidadeEstoque -= dto.Quantidade;

        await _produtoRepository.AtualizarAsync(produto);
    }

    public async Task ReabastecerAsync(int produtoId, MovimentarEstoqueDto dto)
    {
        var produto = await _produtoRepository.BuscarPorIdAsync(produtoId);

        if (produto == null)
            throw new NotFoundException("Produto não encontrado.");

        if (dto.Quantidade <= 0)
            throw new DomainException("A quantidade deve ser maior que zero.");

        produto.QuantidadeEstoque += dto.Quantidade;

        await _produtoRepository.AtualizarAsync(produto);
    }


}