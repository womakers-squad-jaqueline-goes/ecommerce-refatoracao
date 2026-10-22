namespace PiorEcommerceDoMundo;

public interface IProdutoService
{
    Task<List<ProdutoResponseDto>> ListarAsync();
    Task<ProdutoResponseDto> BuscarPorIdAsync(int id);
    Task<ProdutoResponseDto> CriarAsync(CriarProdutoDto dto);
    Task<ProdutoResponseDto> AtualizarAsync(int id, AtualizarProdutoDto dto);
    Task RemoverAsync(int id);
}