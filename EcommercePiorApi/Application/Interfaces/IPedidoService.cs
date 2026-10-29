
using EcommercePiorApi.Application.DTOs.Requests;
using EcommercePiorApi.Application.DTOs.Responses;

namespace EcommercePiorApi.Application.Interfaces;

public interface IPedidoService
{
    Task<PedidoResponseDto> CriarAsync(PedidoRequest request);
    Task<PedidoResponseDto> BuscarPorIdAsync(int id);
    Task<List<PedidoResponseDto>> ListarAsync();
    Task<PedidoResponseDto> ReprocessarPagamentoAsync(int pedidoId);
    Task<PedidoResponseDto> ReenviarLogisticaAsync(int pedidoId);
}