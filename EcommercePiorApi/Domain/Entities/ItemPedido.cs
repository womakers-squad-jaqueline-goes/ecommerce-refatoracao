using PiorEcommerceDoMundo;

namespace EcommercePiorApi.Domain.Entities;

public class ItemPedido
{
    public int Id { get; private set; }
    public int PedidoId { get; private set; }
    public Pedido Pedido { get; private set; } = null!;
    public int ProdutoId { get; private set; }
    public Produto Produto { get; private set; } = null!;
    public int Quantidade { get; private set; }
    public decimal PrecoUnitario { get; private set; }

    public decimal CalcularSubtotal()
    {
        return Quantidade * PrecoUnitario;
    }

    private ItemPedido() { }

    internal ItemPedido(Produto produto, int quantidade)
    {
        if (produto is null)
            throw new DomainException("O produto é obrigatório.");

        if (quantidade <= 0)
            throw new DomainException("A quantidade deve ser maior que zero.");

        Produto = produto;
        ProdutoId = produto.Id;
        Quantidade = quantidade;
        PrecoUnitario = produto.Preco;
    }


}
