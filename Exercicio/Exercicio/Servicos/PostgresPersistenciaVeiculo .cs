using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exercicio.Models;
using Npgsql;

namespace Exercicio.Servicos
{
    public class PostgresPersistenciaVeiculo
    {
        private static readonly string connString = "Server=localhost;User Id=postgres;Password=postgres;Database=upskilling1;";

        public static void Incluir(Veiculo veiculo)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            using var cmd = new NpgsqlCommand("INSERT INTO veiculos (marca, modelo, placa, id_cliente) VALUES (@marca, @modelo, @placa, @id_cliente);", conn);
            cmd.Parameters.AddWithValue("marca", veiculo.Marca);
            cmd.Parameters.AddWithValue("modelo", veiculo.Modelo);
            cmd.Parameters.AddWithValue("placa", veiculo.Placa);
            cmd.Parameters.AddWithValue("id_cliente", veiculo.ClienteId);
            cmd.ExecuteNonQuery();
        }

        public static void Atualizar(Veiculo veiculo)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            using var cmd = new NpgsqlCommand("UPDATE veiculos SET marca = @marca, modelo = @modelo, placa = @placa, id_cliente = @id_cliente WHERE id = @id;", conn);
            cmd.Parameters.AddWithValue("marca", veiculo.Marca);
            cmd.Parameters.AddWithValue("modelo", veiculo.Modelo);
            cmd.Parameters.AddWithValue("placa", veiculo.Placa);
            cmd.Parameters.AddWithValue("id_cliente", veiculo.ClienteId);
            cmd.Parameters.AddWithValue("id", veiculo.Id);
            cmd.ExecuteNonQuery();
        }

        public static List<Veiculo> Listar()
        {
            List<Veiculo> veiculos = new List<Veiculo>();
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            using var cmd = new NpgsqlCommand("SELECT * FROM veiculos;", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                veiculos.Add(new Veiculo(reader.GetInt32(0))
                {
                    Marca = reader.GetString(1),
                    Modelo = reader.GetString(2),
                    Placa = reader.GetString(3),
                    ClienteId = reader.GetInt32(4),
                });
            }
            return veiculos;
        }

        public static void Apagar(int id)
        {
            using var conn = new NpgsqlConnection(connString);
            conn.Open();
            using var cmd = new NpgsqlCommand("DELETE FROM veiculos WHERE id = @id;", conn);
            cmd.Parameters.AddWithValue("id", id);
            cmd.ExecuteNonQuery();
        }
    }
}
