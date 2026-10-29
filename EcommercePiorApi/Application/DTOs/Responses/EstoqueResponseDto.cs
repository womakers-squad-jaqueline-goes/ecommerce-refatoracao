namespace EcommercePiorApi.Application.DTOs.Responses;
public class EstoqueResponseDto
{
    public int ProdutoId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int QuantidadeEstoque { get; set; }
}