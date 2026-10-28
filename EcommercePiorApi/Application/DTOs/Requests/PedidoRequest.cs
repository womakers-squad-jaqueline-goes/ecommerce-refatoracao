using System.ComponentModel.DataAnnotations;
using EcommercePiorApi.Domain.Enums;

namespace EcommercePiorApi.Application.DTOs.Requests;

public class PedidoRequest
{
    [Required(ErrorMessage = "O nome do cliente é obrigatório.")]
    public string ClienteNome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "E-mail inválido.")]
    public string ClienteEmail { get; set; } = string.Empty;

    [EnumDataType(typeof(FormaPagamento), ErrorMessage = "Forma de pagamento inválida.")]
    public FormaPagamento FormaPagamento { get; set; }

    [Required, MinLength(1, ErrorMessage = "O pedido deve ter ao menos um item.")]
    public List<ItemRequest> Itens { get; set; } = new();
}

public class ItemRequest
{
    [Range(1, int.MaxValue, ErrorMessage = "Produto inválido.")]
    public int ProdutoId { get; set; }

    [Range(1, 10_000, ErrorMessage = "A quantidade deve ficar entre 1 e 10.000.")]
    public int Quantidade { get; set; }
}