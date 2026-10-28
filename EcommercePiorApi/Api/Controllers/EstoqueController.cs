// Controllers/EstoqueController.cs
using EcommercePiorApi.Application.DTOs.Requests;
using EcommercePiorApi.Application.DTOs.Responses;
using EcommercePiorApi.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EcommercePiorApi.Controllers;

[ApiController]
[Route("api/estoque")]
public class EstoqueController : ControllerBase
{
    private readonly IEstoqueService _estoque;

    public EstoqueController(IEstoqueService estoque)
    {
        _estoque = estoque;
    }

    [HttpGet]
    public async Task<ActionResult<List<EstoqueResponseDto>>> Listar()
        => Ok(await _estoque.ListarAsync());

    [HttpPost("{produtoId:int}/reabastecer")]
    public async Task<ActionResult<EstoqueResponseDto>> Reabastecer(
        int produtoId, [FromBody] MovimentarEstoqueDto dto)
        => Ok(await _estoque.ReabastecerAsync(produtoId, dto.Quantidade));

    [HttpPost("{produtoId:int}/baixar")]
    public async Task<ActionResult<EstoqueResponseDto>> Baixar(
        int produtoId, [FromBody] MovimentarEstoqueDto dto)
        => Ok(await _estoque.BaixarAsync(produtoId, dto.Quantidade));
}