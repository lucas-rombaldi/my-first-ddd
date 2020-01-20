using MyDDDStore.Core.DomainObjects;
using System;

namespace MyDDDStore.Catalogo.Domain
{
    public class Produto : Entity, IAggregateRoot
    {
        public Guid CategoriaId { get; private set; }
        public string Nome { get; private set; }
        public string Descricao { get; private set; }
        public bool Ativo { get; private set; }
        public decimal Valor { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public string Imagem { get; private set; }
        public decimal QuantidadeEstoque { get; private set; }
        public Categoria Categoria { get; private set; }

        public Dimensoes Dimensoes { get; private set; }

        protected Produto()
        { }

        public Produto(string nome, string descricao, bool ativo, decimal valor, Guid categoriaId, DateTime dataCadastro, string imagem, Dimensoes dimensoes)
        {
            CategoriaId = categoriaId;
            Nome = nome;
            Descricao = descricao;
            Ativo = ativo;
            Valor = valor;
            DataCadastro = dataCadastro;
            Imagem = imagem;
            Dimensoes = dimensoes;

            Validate();
        }


        public void Ativar() => Ativo = true;

        public void Desativar() => Ativo = false;

        public void AlterarDescricao(string descricao)
        {
            AssertConcern.CheckIfEmpty(descricao, "O campo Descricao do produto não pode estar vazio");
            Descricao = descricao;
        }
        

        public void AlterarCategoria(Categoria categoria)
        {
            Categoria = categoria;
            CategoriaId = categoria.Id;
        }

        public void DebitarEstoque(int quantidade)
        {
            if (quantidade < 0) quantidade *= -1;
            if (!PossuiEstoque(quantidade)) throw new DomainException("Estoque insuficiente");
            QuantidadeEstoque -= quantidade;
        }

        public void ReporEstoque(int quantidade)
        {
            if (quantidade < 0) quantidade *= -1;
            QuantidadeEstoque += quantidade;
        }

        public bool PossuiEstoque(int quantidade)
        {
            return QuantidadeEstoque >= quantidade;
        }

        public void Validate()
        {
            AssertConcern.CheckIfEmpty(Nome, "O campo Nome do produto não pode estar vazio");
            AssertConcern.CheckIfEmpty(Descricao, "O campo Descricao do produto não pode estar vazio");
            AssertConcern.CheckIfEqual(CategoriaId, Guid.Empty, "O campo CategoriaId do produto não pode estar vazio");
            AssertConcern.CheckLeastThan(Valor, 1, "O campo Valor do produto não pode se menor igual a 0");
            AssertConcern.CheckIfEmpty(Imagem, "O campo Imagem do produto não pode estar vazio");
        }
    }
}
