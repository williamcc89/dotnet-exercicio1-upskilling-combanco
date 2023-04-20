using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercicio.Models
{
    public class Veiculo
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Placa { get; set; }

        public Veiculo(int Id, int ClienteId, string Marca, string Modelo, string Placa)
        {
            this.Id = Id;
            this.ClienteId = ClienteId;
            this.Marca = Marca;
            this.Modelo = Modelo;
            this.Placa = Placa;
        }

        public Veiculo(int ClienteId, string Marca, string Modelo, string Placa)
        {
            this.Id = Id;
            this.ClienteId = ClienteId;
            this.Marca = Marca;
            this.Modelo = Modelo;
            this.Placa = Placa;
        }

        public Veiculo(int Id)
        {
            this.Id = Id;
        }

        public Veiculo() { }
    }
}
