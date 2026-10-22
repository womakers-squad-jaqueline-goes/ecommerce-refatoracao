namespace PiorEcommerceDoMundo;

public interface IEstoqueService
{
    Task<List<EstoqueResponseDto>> ListarAsync();
    Task BaixarAsync(int produtoId, MovimentarEstoqueDto dto);
    Task ReabastecerAsync(int produtoId, MovimentarEstoqueDto dto);
}