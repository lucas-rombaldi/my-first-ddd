using MyDDDStore.Core.DomainObjects;
using System;
using System.Collections.Generic;

namespace MyDDDStore.Catalogo.Domain
{
    public class Categoria : Entity
    {
        public string Nome { get; set; }

        public int Codigo { get; set; }

        // EF Relation
        public ICollection<Produto> Produtos { get; set; }

        protected Categoria()
        { }

        public Categoria(string nome, int codigo)
        {
            Nome = nome;
            Codigo = codigo;

            Validate();
        }

        public void Validate()
        {
            AssertConcern.CheckIfEmpty(Nome, "O campo Nome da categoria não pode estar vazio");
            AssertConcern.CheckIfEqual(Codigo, 0, "O campo Codigo não pode ser 0"); 
        }

        public override string ToString()
        {
            return $"{Nome} - {Codigo}";
        }
    }
}
