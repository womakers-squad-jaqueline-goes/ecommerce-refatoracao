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
    public async Task<ActionResult<List<EstoqueResponseDto>>> ListarAsync()
    {
        var estoque = await _estoqueService.ListarAsync();

        return Ok(estoque);
    }

    [HttpPost("{produtoId:int}/baixar")]
    public async Task<IActionResult> BaixarAsync(
    int produtoId,
    [FromBody] MovimentarEstoqueDto dto)
    {
        await _estoqueService.BaixarAsync(produtoId, dto);

        return NoContent();
    }

    [HttpPost("{produtoId:int}/reabastecer")]
    public async Task<IActionResult> ReabastecerAsync(
    int produtoId,
    [FromBody] MovimentarEstoqueDto dto)
    {
        await _estoqueService.ReabastecerAsync(produtoId, dto);

        return NoContent();
    }

    
}