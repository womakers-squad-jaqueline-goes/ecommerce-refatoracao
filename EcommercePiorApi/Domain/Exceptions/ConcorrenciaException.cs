namespace EcommercePiorApi.Domain.Exceptions;// <summary>Outra requisição alterou o mesmo registro ao mesmo tempo (HTTP 409).</summary>
public class ConcorrenciaException : DomainException
{
    public ConcorrenciaException(Exception innerException)
        : base("O recurso foi alterado por outra requisição. Tente novamente.")
    {
    }
}