using MyDDDStore.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyDDDStore.Catalogo.Domain
{
    public class Dimensoes
    {
        public decimal Altura { get; private set; }

        public decimal Largura { get; private set; }
        
        public decimal Profundidade { get; private set; }

        public Dimensoes(decimal altura, decimal largura, decimal profundidade)
        {
            AssertConcern.CheckLeastThan(altura, 1, "O campo Altura não pode ser menor ou igual a 0");
            AssertConcern.CheckLeastThan(largura, 1, "O campo Largura não pode ser menor ou igual a 0");
            AssertConcern.CheckLeastThan(profundidade, 1, "O campo Profundidade não pode ser menor ou igual a 0");

            Altura = altura;
            Largura = largura;
            Profundidade = profundidade;
        }

        public string DescricaoFormatada()
        {
            return $"LxAxP: {Largura} x {Altura} x {Profundidade}";
        }

        public override string ToString()
        {
            return DescricaoFormatada();
        }
    }
}
