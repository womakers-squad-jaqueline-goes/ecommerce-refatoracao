using EcommercePiorApi.Domain.Enums;

namespace EcommercePiorApi.Application.DTOs.Responses;

public class PedidoResponseDto
{
    public int Id { get; set; }
    public string ClienteNome { get; set; } = string.Empty;
    public string ClienteEmail { get; set; } = string.Empty;
    public DateTime DataPedido { get; set; }
    public StatusPedido Status { get; set; }
    public FormaPagamento FormaPagamento { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Desconto { get; set; }
    public decimal ValorTotal { get; set; }
    public string? CodigoRastreio { get; set; }
    public List<ItemPedidoResponseDto> Itens { get; set; } = new();
}

public class ItemPedidoResponseDto
{
    public int ProdutoId { get; set; }
    public string Produto { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public decimal Subtotal { get; set; }
}