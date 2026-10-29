// Application/Services/PedidoService.cs
using EcommercePiorApi.Application.DTOs.Requests;
using EcommercePiorApi.Application.DTOs.Responses;
using EcommercePiorApi.Application.Interfaces;
using EcommercePiorApi.Application.Mappings;
using EcommercePiorApi.Domain.Entities;
using EcommercePiorApi.Domain.Exceptions;

namespace EcommercePiorApi.Application.Services;

public class PedidoService : IPedidoService
{
    private const int MaxTentativasEstorno = 3;

    private readonly IPedidoRepository _pedidos;
    private readonly IProdutoRepository _produtos;
    private readonly IEnumerable<IRegraDesconto> _regrasDesconto;
    private readonly IPagamentoService _pagamento;
    private readonly ILogisticaService _logistica;
    private readonly ILogger<PedidoService> _logger;

    public PedidoService(
        IPedidoRepository pedidos,
        IProdutoRepository produtos,
        IEnumerable<IRegraDesconto> regrasDesconto,
        IPagamentoService pagamento,
        ILogisticaService logistica,
        ILogger<PedidoService> logger)
    {
        _pedidos = pedidos;
        _produtos = produtos;
        _regrasDesconto = regrasDesconto;
        _pagamento = pagamento;
        _logistica = logistica;
        _logger = logger;
    }

    public async Task<PedidoResponseDto> CriarAsync(PedidoRequest request)
    {
        var pedido = Pedido.Criar(request.ClienteNome, request.ClienteEmail, request.FormaPagamento);

        foreach (var item in request.Itens)
        {
            var produto = await _produtos.BuscarPorIdAsync(item.ProdutoId)
                          ?? throw new NotFoundException($"Produto {item.ProdutoId} não encontrado.");

            pedido.AdicionarItem(produto, item.Quantidade); // valida e baixa o estoque
        }

        pedido.AplicarDesconto(CalcularDesconto(pedido));
        _pedidos.Adicionar(pedido);

        await _pedidos.SalvarAsync();
        _logger.LogInformation("Pedido {PedidoId} criado. Total: {Total}", pedido.Id, pedido.ValorTotal);

        await ProcessarPagamentoAsync(pedido);
        await EnviarAsync(pedido);

        return pedido.ToResponse();
    }

    public async Task<PedidoResponseDto> BuscarPorIdAsync(int id)
        => (await ObterOuFalharAsync(id)).ToResponse();

    public async Task<List<PedidoResponseDto>> ListarAsync()
        => (await _pedidos.ListarAsync()).Select(p => p.ToResponse()).ToList();

    public async Task<PedidoResponseDto> ReprocessarPagamentoAsync(int pedidoId)
    {
        var pedido = await ObterOuFalharAsync(pedidoId);

        pedido.ReabrirParaPagamento(); 
        await _pedidos.SalvarAsync();

        await ProcessarPagamentoAsync(pedido);
        await EnviarAsync(pedido);

        return pedido.ToResponse();
    }

    public async Task<PedidoResponseDto> ReenviarLogisticaAsync(int pedidoId)
    {
        var pedido = await ObterOuFalharAsync(pedidoId);

        pedido.GarantirPodeSerEnviado(); // valida ANTES de chamar a transportadora
        await EnviarAsync(pedido);

        return pedido.ToResponse();
    }

        private decimal CalcularDesconto(Pedido pedido)
        => _regrasDesconto.Select(r => r.Calcular(pedido)).FirstOrDefault(d => d > 0);

    private async Task ProcessarPagamentoAsync(Pedido pedido)
    {
        var aprovado = await _pagamento.ProcessarPagamentoAsync(pedido.ValorTotal, pedido.FormaPagamento);

        if (!aprovado)
        {
            await MarcarComoFalhouAsync(pedido);

            _logger.LogWarning("Pagamento do pedido {PedidoId} recusado", pedido.Id);
            throw new PagamentoRecusadoException(pedido.Id);
        }

        pedido.MarcarComoPago();
        await _pedidos.SalvarAsync();
    }

    private async Task MarcarComoFalhouAsync(Pedido pedido)
    {
        for (var tentativa = 1; ; tentativa++)
        {
            try
            {
                pedido.MarcarComoFalhou();
                await _pedidos.SalvarAsync();
                return;
            }
            catch (ConcorrenciaException) when (tentativa < MaxTentativasEstorno)
            {
                _logger.LogWarning(
                    "Conflito ao estornar o pedido {PedidoId} (tentativa {Tentativa}). Tentando de novo",
                    pedido.Id, tentativa);

                await _pedidos.RecarregarAsync(pedido);
            }
        }
    }


    private async Task EnviarAsync(Pedido pedido)
    {
        var codigo = await _logistica.EnviarAsync(pedido);

        pedido.RegistrarEnvio(codigo);
        await _pedidos.SalvarAsync();

        _logger.LogInformation("Pedido {PedidoId} enviado. Rastreio: {Codigo}", pedido.Id, codigo);
    }

    private async Task<Pedido> ObterOuFalharAsync(int id)
        => await _pedidos.BuscarComItensAsync(id)
           ?? throw new NotFoundException($"Pedido {id} não encontrado.");
}