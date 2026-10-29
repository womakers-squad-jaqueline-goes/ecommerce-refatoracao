using EcommercePiorApi.Application.DTOs.Responses;
using EcommercePiorApi.Domain.Entities;

namespace EcommercePiorApi.Application.Mappings;

public static class MapeamentoExtensions
{
    public static ProdutoResponseDto ToResponse(this Produto p) => new()
    {
        Id = p.Id,
        Nome = p.Nome,
        Descricao = p.Descricao,
        Preco = p.Preco,
        QuantidadeEstoque = p.QuantidadeEstoque,
        Ativo = p.Ativo
    };

    public static EstoqueResponseDto ToEstoqueResponse(this Produto p) => new()
    {
        ProdutoId = p.Id,
        Nome = p.Nome,
        QuantidadeEstoque = p.QuantidadeEstoque
    };

    public static PedidoResponseDto ToResponse(this Pedido p) => new()
    {
        Id = p.Id,
        ClienteNome = p.ClienteNome,
        ClienteEmail = p.ClienteEmail,
        DataPedido = p.DataPedido,
        Status = p.Status,
        FormaPagamento = p.FormaPagamento,
        Subtotal = p.Subtotal,
        Desconto = p.Desconto,
        ValorTotal = p.ValorTotal,
        CodigoRastreio = p.CodigoRastreio,
        Itens = p.Itens.Select(i => new ItemPedidoResponseDto
        {
            ProdutoId = i.ProdutoId,
            Produto = i.Produto.Nome,
            Quantidade = i.Quantidade,
            PrecoUnitario = i.PrecoUnitario,
            Subtotal = i.CalcularSubtotal()
        }).ToList()
    };
}