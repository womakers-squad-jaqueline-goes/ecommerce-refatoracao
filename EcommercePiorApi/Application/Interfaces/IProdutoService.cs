

using EcommercePiorApi.Application.DTOs.Requests;
using EcommercePiorApi.Application.DTOs.Responses;

namespace EcommercePiorApi.Application.Interfaces;
public interface IProdutoService
{
    Task<List<ProdutoResponseDto>> ListarAsync();
    Task<ProdutoResponseDto> BuscarPorIdAsync(int id);
    Task<ProdutoResponseDto> CriarAsync(CriarProdutoDto dto);
    Task<ProdutoResponseDto> AtualizarAsync(int id, AtualizarProdutoDto dto);
    Task DesativarAsync(int id);
}