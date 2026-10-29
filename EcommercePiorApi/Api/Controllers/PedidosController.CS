using EcommercePiorApi.Application.DTOs.Requests;
using EcommercePiorApi.Application.DTOs.Responses;
using EcommercePiorApi.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EcommercePiorApi.Controllers;

[ApiController]
[Route("api/pedidos")]
public class PedidosController : ControllerBase
{
    private readonly IPedidoService _pedidos;

    public PedidosController(IPedidoService pedidos)
    {
        _pedidos = pedidos;
    }

    [HttpGet]
    public async Task<ActionResult<List<PedidoResponseDto>>> Listar()
        => Ok(await _pedidos.ListarAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PedidoResponseDto>> BuscarPorId(int id)
        => Ok(await _pedidos.BuscarPorIdAsync(id));

    /// <remarks>Pagamento recusado devolve 402 com o "pedidoId", para reprocessar depois.</remarks>
    [HttpPost]
    public async Task<ActionResult<PedidoResponseDto>> Criar([FromBody] PedidoRequest request)
    {
        var pedido = await _pedidos.CriarAsync(request);
        return CreatedAtAction(nameof(BuscarPorId), new { id = pedido.Id }, pedido);
    }

    /// <summary>Nova tentativa de pagamento (somente pedidos com status Falhou).</summary>
    [HttpPost("{id:int}/reprocessar-pagamento")]
    public async Task<ActionResult<PedidoResponseDto>> ReprocessarPagamento(int id)
        => Ok(await _pedidos.ReprocessarPagamentoAsync(id));

    /// <summary>Reenvia à transportadora (somente pedidos Pago ou Enviado).</summary>
    [HttpPost("{id:int}/reenviar-logistica")]
    public async Task<ActionResult<PedidoResponseDto>> ReenviarLogistica(int id)
        => Ok(await _pedidos.ReenviarLogisticaAsync(id));
}