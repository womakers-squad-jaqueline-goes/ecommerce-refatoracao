namespace EcommercePiorApi.Domain.Exceptions;

public class NotFoundException : DomainException
{
    public NotFoundException(string mensagem) : base(mensagem)
    {
    }
}