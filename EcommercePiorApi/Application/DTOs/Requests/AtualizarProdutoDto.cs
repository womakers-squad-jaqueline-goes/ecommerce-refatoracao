using System.ComponentModel.DataAnnotations;

namespace PiorEcommerceDoMundo;

public class AtualizarProdutoDto
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "A descrição é obrigatoria")]
    public string Descricao { get; set; } = string.Empty;
    
    [Range(typeof(decimal), "0.01", "79228162514264337593543950335",
    ErrorMessage = "O preço deve ser maior ou igual a R$ 0,01.")]
    public decimal Preco { get; set; }

    [Range(0, int.MaxValue,
        ErrorMessage = "A quantidade em estoque não pode ser negativa.")]
    public int QuantidadeEstoque { get; set; }
    public bool Ativo { get; set; } = true;
}