namespace EcommercePiorApi.Application.DTOs.Requests;

public class PedidoRequest
{
    public string ClienteNome { get; set; }
    public string ClienteEmail { get; set; }
    public string FormaPagamento { get; set; }
    public List<ItemRequest> Itens { get; set; }
}

public class ItemRequest
{
    public int ProdutoId { get; set; }
    public int Quantidade { get; set; }
}
