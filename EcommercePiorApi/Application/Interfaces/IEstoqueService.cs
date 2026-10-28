using EcommercePiorApi.Application.DTOs.Responses;

namespace EcommercePiorApi.Application.Interfaces;

public interface IEstoqueService
{
    Task<List<EstoqueResponseDto>> ListarAsync();
    Task<EstoqueResponseDto> ReabastecerAsync(int produtoId, int quantidade);
    Task<EstoqueResponseDto> BaixarAsync(int produtoId, int quantidade);
}