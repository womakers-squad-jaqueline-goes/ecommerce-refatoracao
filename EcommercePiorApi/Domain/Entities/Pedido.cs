using EcommercePiorApi.Domain.Enums;
using EcommercePiorApi.Domain.Exceptions;

namespace EcommercePiorApi.Domain.Entities;

public class Pedido
{
    private readonly List<ItemPedido> _itens = new();

    public int Id { get; private set; }
    public string ClienteNome { get; private set; } = string.Empty;
    public string ClienteEmail { get; private set; } = string.Empty;
    public DateTime DataPedido { get; private set; }
    public StatusPedido Status { get; private set; } = StatusPedido.Pendente;
    public FormaPagamento FormaPagamento { get; private set; }
    public decimal Desconto { get; private set; }
    public decimal ValorTotal { get; private set; }
    public string? CodigoRastreio { get; private set; }
    public IReadOnlyCollection<ItemPedido> Itens => _itens;

    private Pedido() { }

    public static Pedido Criar(string clienteNome, string clienteEmail, FormaPagamento formaPagamento)
    {
        if (string.IsNullOrWhiteSpace(clienteNome))
            throw new DomainException("O nome do cliente é obrigatório.");

        if (string.IsNullOrWhiteSpace(clienteEmail))
            throw new DomainException("O email do cliente é obrigatório.");

        if (!Enum.IsDefined(formaPagamento))
            throw new DomainException("A forma de pagamento é inválida.");

        return new Pedido
        {
            ClienteNome = clienteNome.Trim(),
            ClienteEmail = clienteEmail.Trim(),
            FormaPagamento = formaPagamento,
            DataPedido = DateTime.UtcNow,
            Status = StatusPedido.Pendente
        };
    }

    public void AdicionarItem(Produto produto, int quantidade)
    {
        if (Status != StatusPedido.Pendente)
            throw new DomainException("Não é possível adicionar itens a um pedido que não está pendente.");

        if (produto is null)
            throw new DomainException("O produto é obrigatório.");

        if (!produto.Ativo)
            throw new DomainException($"O produto {produto.Nome} está inativo.");

        produto.DiminuirEstoque(quantidade); // valida quantidade e estoque
        _itens.Add(new ItemPedido(produto, quantidade));
    }

    public void AplicarDesconto(decimal valor)
    {
        if (valor < 0 || valor > Subtotal)
            throw new DomainException("O desconto é inválido para o valor do pedido.");

        Desconto = valor;
    }

        public void MarcarComoPago()
    {
        GarantirStatus(StatusPedido.Pendente, "pago");
        Status = StatusPedido.Pago;
    }

    public void MarcarComoFalhou()
    {
        GarantirStatus(StatusPedido.Pendente, "marcado como falho");

        foreach (var item in _itens)
            item.Produto.ReabastecerEstoque(item.Quantidade);

        Status = StatusPedido.Falhou;
    }

    public void ReabrirParaPagamento()
    {
        GarantirStatus(StatusPedido.Falhou, "reaberto para pagamento");

        foreach (var item in _itens)
            item.Produto.DiminuirEstoque(item.Quantidade);

        Status = StatusPedido.Pendente;
    }

    public void GarantirPodeSerEnviado()
    {
        if (Status is not (StatusPedido.Pago or StatusPedido.Enviado))
            throw new DomainException($"O pedido está '{Status}' e não pode ser enviado.");
    }

    public void RegistrarEnvio(string codigoRastreio)
    {
        GarantirPodeSerEnviado();

        if (string.IsNullOrWhiteSpace(codigoRastreio))
            throw new DomainException("O código de rastreio é obrigatório.");

        CodigoRastreio = codigoRastreio;
        Status = StatusPedido.Enviado;
    }

    private void GarantirStatus(StatusPedido esperado, string acao)
    {
        if (Status != esperado)
            throw new DomainException($"O pedido está '{Status}' e não pode ser {acao}.");
    }

    public decimal Subtotal => _itens.Sum(i => i.CalcularSubtotal());
}