// Controllers/ProdutosController.cs
using EcommercePiorApi.Application.DTOs.Requests;
using EcommercePiorApi.Application.DTOs.Responses;
using EcommercePiorApi.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EcommercePiorApi.Controllers;

[ApiController]
[Route("api/produtos")]
public class ProdutosController : ControllerBase
{
    private readonly IProdutoService _produtos;

    public ProdutosController(IProdutoService produtos)
    {
        _produtos = produtos;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProdutoResponseDto>>> Listar()
        => Ok(await _produtos.ListarAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProdutoResponseDto>> BuscarPorId(int id)
        => Ok(await _produtos.BuscarPorIdAsync(id));

    [HttpPost]
    public async Task<ActionResult<ProdutoResponseDto>> Criar([FromBody] CriarProdutoDto dto)
    {
        var produto = await _produtos.CriarAsync(dto);
        return CreatedAtAction(nameof(BuscarPorId), new { id = produto.Id }, produto);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ProdutoResponseDto>> Atualizar(int id, [FromBody] AtualizarProdutoDto dto)
        => Ok(await _produtos.AtualizarAsync(id, dto));

    /// <summary>Exclusão lógica: o produto é desativado, para não perder o histórico dos pedidos.</summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Desativar(int id)
    {
        await _produtos.DesativarAsync(id);
        return NoContent();
    }
}