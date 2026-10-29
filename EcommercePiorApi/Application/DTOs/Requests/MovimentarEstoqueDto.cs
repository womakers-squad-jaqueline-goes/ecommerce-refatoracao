using System.ComponentModel.DataAnnotations;

namespace EcommercePiorApi.Application.DTOs.Requests;

public class MovimentarEstoqueDto
{
    [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser maior que zero.")]
    public int Quantidade { get; set; }
}