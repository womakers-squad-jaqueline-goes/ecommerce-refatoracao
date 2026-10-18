using Microsoft.AspNetCore.Mvc;

namespace PiorEcommerceDoMundo;

[ApiController]
[Route("api/produtos")]
public class ProdutosController : ControllerBase
{
    private readonly IProdutoService _produtoService;

    public ProdutosController(IProdutoService produtoService)
    {
        _produtoService = produtoService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProdutoResponseDto>>> ListarAsync()
    {
        var produtos = await _produtoService.ListarAsync();
        return Ok(produtos);
    }

    [HttpPost]
    public async Task<ActionResult<ProdutoResponseDto>> CriarAsync([FromBody] CriarProdutoDto criarProdutoDto)
    {
        var produto = await _produtoService.CriarAsync(criarProdutoDto);
        return CreatedAtRoute("BuscarProdutoPorId", new { id = produto.Id }, produto);
    }

    [HttpGet("{id:int}", Name = "BuscarProdutoPorId")]
    public async Task<ActionResult<ProdutoResponseDto>> BuscarPorIdAsync(int id)
    {
        var produto = await _produtoService.BuscarPorIdAsync(id);
        return Ok(produto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ProdutoResponseDto>> AtualizarAsync(int id, [FromBody] AtualizarProdutoDto atualizarProdutoDto)
    {
        var produto = await _produtoService.AtualizarAsync(id, atualizarProdutoDto);
        return Ok(produto);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> RemoverAsync(int id)
    {
        await _produtoService.RemoverAsync(id);
        return NoContent();
    }
}