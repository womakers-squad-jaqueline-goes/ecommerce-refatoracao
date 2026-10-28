using PiorEcommerceDoMundo;

namespace EcommercePiorApi.Domain.Entities;

public class Produto
{
    public int Id { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public string Descricao { get; private set; } = string.Empty;
    public decimal Preco { get; private set; }
    public int QuantidadeEstoque { get; private set; }
    public bool Ativo { get; private set; } = true;

    private Produto() { }

    internal Produto(string nome, string descricao, decimal preco, int quantidadeEstoque)
    {
        ValidarNome(nome);
        ValidarDescricao(descricao);
        ValidarPreco(preco);

        if (quantidadeEstoque < 0)
            throw new DomainException("A quantidade em estoque não pode ser negativa.");

        Nome = nome;
        Descricao = descricao;
        Preco = preco;
        QuantidadeEstoque = quantidadeEstoque;
    }

    public void Atualizar(string nome, string descricao, decimal preco)
    {
        ValidarNome(nome);
        ValidarDescricao(descricao);
        ValidarPreco(preco);

        Nome = nome;
        Descricao = descricao;
        Preco = preco;
    }

    public void ReabastecerEstoque(int quantidade)
    {
        if (quantidade <= 0)
            throw new DomainException("A quantidade a ser adicionada deve ser maior que zero.");

        QuantidadeEstoque += quantidade;
    }

    public void DiminuirEstoque(int quantidade)
    {
        if (quantidade <= 0)
            throw new DomainException("A quantidade a ser removida deve ser maior que zero.");

        if (QuantidadeEstoque < quantidade)
            throw new DomainException("Estoque insuficiente para a operação.");

        QuantidadeEstoque -= quantidade;
    }

    public void Ativar() => Ativo = true;

    public void Desativar() => Ativo = false;

    private void ValidarNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new DomainException("O nome do produto é obrigatório.");
    }

    private void ValidarDescricao(string descricao)
    {
        if (string.IsNullOrWhiteSpace(descricao))
            throw new DomainException("A descrição do produto é obrigatória.");
    }

    private void ValidarPreco(decimal preco)
    {
        if (preco <= 0)
            throw new DomainException("O preço do produto deve ser maior que zero.");
    }
}
