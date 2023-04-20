using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercicio.Models
{
    public class Movimentacao
    {
        public int Id { get; set; }
        public int VeiculoId { get; set; }
        public DateTime Entrada { get; set; }
        public DateTime? Saida { get; set; }
        public double? Valor { get; set; }

        public Movimentacao(int Id, int VeiculoId, DateTime Entrada, DateTime Saida, double Valor)
        {
            this.Id = Id;
            this.VeiculoId = VeiculoId;
            this.Entrada = Entrada;
            this.Saida = Saida;
            this.Valor = Valor;
        }

        public Movimentacao(int VeiculoId, DateTime Entrada, DateTime? Saida, double? Valor)
        {
            this.Id = Id;
            this.VeiculoId = VeiculoId;
            this.Entrada = Entrada;
            this.Saida = Saida ?? null;
            this.Valor = Valor;
        }

        public Movimentacao(int Id)
        {
            this.Id = Id;
        }

        public Movimentacao() { }
    }
}
