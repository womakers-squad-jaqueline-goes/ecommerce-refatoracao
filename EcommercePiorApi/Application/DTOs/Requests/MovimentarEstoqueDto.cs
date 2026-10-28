using System.ComponentModel.DataAnnotations;

namespace PiorEcommerceDoMundo;

public class MovimentarEstoqueDto
{
    [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser maior que zero.")]
    public int Quantidade { get; set; }
}