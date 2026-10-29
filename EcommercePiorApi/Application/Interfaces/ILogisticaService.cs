using EcommercePiorApi.Domain.Entities;

namespace EcommercePiorApi.Application.Interfaces;

public interface ILogisticaService
{
    Task<string> EnviarAsync(Pedido pedido);
}