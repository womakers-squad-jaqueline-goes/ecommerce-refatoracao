using Microsoft.AspNetCore.Mvc;

namespace PiorEcommerceDoMundo;

[ApiController]
[Route("api/estoque")]
public class EstoqueController : ControllerBase
{
    private readonly IEstoqueService _estoqueService;

    public EstoqueController(IEstoqueService estoqueService)
    {
        _estoqueService = estoqueService;
    }

    [HttpGet]
    [EndpointDescription("Lista todos os produtos com suas quantidades em estoque.")]
    [ProducesResponseType(typeof(List<EstoqueResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<EstoqueResponseDto>>> ListarAsync()
    {
        var estoque = await _estoqueService.ListarAsync();

        return Ok(estoque);
    }

    [HttpPost("{produtoId:int}/baixar")]
    [EndpointDescription("Baixa o estoque de um produto.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> BaixarAsync(
    int produtoId,
    [FromBody] MovimentarEstoqueDto dto)
    {
        await _estoqueService.BaixarAsync(produtoId, dto);

        return NoContent();
    }

    [HttpPost("{produtoId:int}/reabastecer")]
    [EndpointDescription("Reabastece o estoque de um produto.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ReabastecerAsync(
    int produtoId,
    [FromBody] MovimentarEstoqueDto dto)
    {
        await _estoqueService.ReabastecerAsync(produtoId, dto);

        return NoContent();
    }

    
}