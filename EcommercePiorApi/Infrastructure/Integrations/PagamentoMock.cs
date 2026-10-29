// Infrastructure/Integrations/PagamentoMock.cs
using EcommercePiorApi.Application.Interfaces;
using EcommercePiorApi.Domain.Enums;
using Microsoft.Extensions.Options;

namespace EcommercePiorApi.Infrastructure.Integrations;

/// <summary>Simula o gateway de pagamento: espera um pouco e pode recusar compras no cartão.</summary>
public class PagamentoMock : IPagamentoService
{
    private const int LatenciaMs = 500;

    private readonly ILogger<PagamentoMock> _logger;
    private readonly PagamentoMockOptions _options;

    public PagamentoMock(ILogger<PagamentoMock> logger, IOptions<PagamentoMockOptions> options)
    {
        _logger = logger;
        _options = options.Value;
    }

    public async Task<bool> ProcessarPagamentoAsync(decimal valor, FormaPagamento formaPagamento)
    {
        _logger.LogInformation("Processando pagamento de {Valor} via {Forma}", valor, formaPagamento);

        await Task.Delay(LatenciaMs); 

        if (formaPagamento != FormaPagamento.Cartao)
            return true;

        return Random.Shared.NextDouble() >= _options.TaxaRecusaCartao;
    }
}