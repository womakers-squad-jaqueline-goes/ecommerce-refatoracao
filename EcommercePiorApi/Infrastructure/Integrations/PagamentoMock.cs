using EcommercePiorApi.Application.Interfaces;

namespace EcommercePiorApi.Infrastructure.Integrations;

public class PagamentoMock : IPagamentoService
{
    public async Task<bool> ProcessarPagamentoAsync(
        decimal valor,
        string formaPagamento)
    {
        Console.WriteLine(
            $"Processando pagamento de {valor} via {formaPagamento}");

        await Task.Delay(500);

        return !string.Equals(
            formaPagamento,
            "recusar",
            StringComparison.OrdinalIgnoreCase);
    }
}