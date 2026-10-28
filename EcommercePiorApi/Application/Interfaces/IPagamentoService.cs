// Application/Interfaces/IPagamentoService.cs
using EcommercePiorApi.Domain.Enums;

namespace EcommercePiorApi.Application.Interfaces;
public interface IPagamentoService
{
    Task<bool> ProcessarPagamentoAsync(decimal valor, FormaPagamento formaPagamento);
}