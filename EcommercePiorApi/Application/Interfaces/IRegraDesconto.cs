using EcommercePiorApi.Domain.Entities;

namespace EcommercePiorApi.Application.Interfaces;

public interface IRegraDesconto
{
    decimal Calcular(Pedido pedido);
}