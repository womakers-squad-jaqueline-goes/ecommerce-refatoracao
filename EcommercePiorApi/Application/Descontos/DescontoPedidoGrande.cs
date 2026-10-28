using EcommercePiorApi.Application.Interfaces;
using EcommercePiorApi.Domain.Entities;

namespace EcommercePiorApi.Application.Descontos;

public class DescontoPedidoGrande : IRegraDesconto
{
    private const decimal ValorMinimo = 500m;
    private const decimal Percentual = 0.10m;

    public decimal Calcular(Pedido pedido) =>
        pedido.Subtotal > ValorMinimo
            ? Math.Round(pedido.Subtotal * Percentual, 2)
            : 0m;
}