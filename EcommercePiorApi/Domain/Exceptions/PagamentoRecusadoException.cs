namespace EcommercePiorApi.Domain.Exceptions;

public class PagamentoRecusadoException : DomainException
{
    public int PedidoId { get; }

    public PagamentoRecusadoException(int pedidoId)
        : base($"O pagamento do pedido {pedidoId} foi recusado.")
    {
        PedidoId = pedidoId;
    }
}