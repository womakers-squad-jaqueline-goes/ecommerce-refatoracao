using EcommercePiorApi.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace EcommercePiorApi.Middleware;

/// <summary>Traduz exceções em respostas ProblemDetails (RFC 7807).</summary>
public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        var status = exception switch
        {
            NotFoundException => StatusCodes.Status404NotFound,
            PagamentoRecusadoException => StatusCodes.Status402PaymentRequired,
            ConcorrenciaException => StatusCodes.Status409Conflict,
            DomainException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        var problem = new ProblemDetails { Status = status, Instance = context.Request.Path };

        if (exception is DomainException)
        {
            _logger.LogWarning("Requisição rejeitada ({Status}): {Mensagem}", status, exception.Message);
            problem.Title = "Requisição não atendida";
            problem.Detail = exception.Message;
        }
        else
        {
            _logger.LogError(exception, "Erro não tratado em {Metodo} {Caminho}",
                context.Request.Method, context.Request.Path);
            problem.Title = "Erro interno";
            problem.Detail = "Ocorreu um erro inesperado. Tente novamente mais tarde.";
        }

        if (exception is PagamentoRecusadoException recusado)
            problem.Extensions["pedidoId"] = recusado.PedidoId; // permite reprocessar

        context.Response.StatusCode = status;
        await context.Response.WriteAsJsonAsync(problem, cancellationToken);
        return true;
    }
}