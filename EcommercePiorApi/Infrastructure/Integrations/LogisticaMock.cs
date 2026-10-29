using EcommercePiorApi.Application.Interfaces;
using EcommercePiorApi.Domain.Entities;

namespace EcommercePiorApi.Infrastructure.Integrations;

public class LogisticaMock : ILogisticaService
{
    private const int LatenciaMs = 300;

    private readonly ILogger<LogisticaMock> _logger;

    public LogisticaMock(ILogger<LogisticaMock> logger)
    {
        _logger = logger;
    }

    public async Task<string> EnviarAsync(Pedido pedido)
    {
        _logger.LogInformation("Enviando pedido {PedidoId} para a transportadora", pedido.Id);

        await Task.Delay(LatenciaMs);

        return "BR" + Random.Shared.Next(100_000, 1_000_000); 
    }
}