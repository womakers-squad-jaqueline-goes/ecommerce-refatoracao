using EcommercePiorApi.Domain.Enums;
using PiorEcommerceDoMundo;

namespace EcommercePiorApi.Domain.Entities;

public class Pedido
{
    public int Id { get; private set; }
    public string ClienteNome { get; private set; } = string.Empty;
    public string ClienteEmail { get; private set; } = string.Empty;
    public DateTime DataPedido { get; private set; }
    public StatusPedido StatusPedido { get; private set; } = StatusPedido.Pendente;
    public decimal ValorTotal { get; private set; }
    public FormaPagamento FormaPagamento { get; private set; }
    public string? CodigoRastreio { get; private set; }
    public List<ItemPedido> Itens { get; private set; } = new();

    private Pedido() { }

    internal Pedido(string clienteNome, string clienteEmail, FormaPagamento formaPagamento)
    {
        if (string.IsNullOrWhiteSpace(clienteNome))
            throw new DomainException("O nome do cliente é obrigatório.");

        if (string.IsNullOrWhiteSpace(clienteEmail))
            throw new DomainException("O email do cliente é obrigatório.");

        if (!Enum.IsDefined(typeof(FormaPagamento), formaPagamento))
            throw new DomainException("A forma de pagamento é inválida.");

        ClienteNome = clienteNome;
        ClienteEmail = clienteEmail;
        DataPedido = DateTime.UtcNow;
        FormaPagamento = formaPagamento;
    }

    public void AdicionarItem(Produto produto, int quantidade)
    {
        if (StatusPedido != StatusPedido.Pendente)
            throw new DomainException("Não é possível adicionar itens a um pedido que não está pendente.");

        var itemExistente = new ItemPedido(produto, quantidade);
        Itens.Add(itemExistente);
        RecalcularValorTotal();
    }

    private void RecalcularValorTotal()
    {
        ValorTotal = Itens.Sum(i => i.CalcularSubtotal());
    }
}
