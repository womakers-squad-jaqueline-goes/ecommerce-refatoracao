namespace EcommercePiorApi.Infrastructure.Integrations;

public class PagamentoMockOptions
{
    public const string Secao = "PagamentoMock";
    public double TaxaRecusaCartao { get; set; } = 0.10;
}