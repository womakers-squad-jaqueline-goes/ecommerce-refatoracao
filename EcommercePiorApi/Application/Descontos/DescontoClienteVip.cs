using EcommercePiorApi.Application.Interfaces;
using EcommercePiorApi.Domain.Entities;

namespace EcommercePiorApi.Application.Descontos;

public class DescontoClienteVip : IRegraDesconto
{
    private const string PalavraChave = "vip"; 
    private const decimal Percentual = 0.20m;

    public decimal Calcular(Pedido pedido) =>
        pedido.ClienteEmail.Contains(PalavraChave, StringComparison.OrdinalIgnoreCase)
            ? Math.Round(pedido.Subtotal * Percentual, 2)
            : 0m;
}